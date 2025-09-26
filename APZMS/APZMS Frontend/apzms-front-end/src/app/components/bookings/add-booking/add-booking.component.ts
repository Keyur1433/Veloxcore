import { Component, OnInit, ChangeDetectorRef, ViewChild, OnDestroy } from '@angular/core';
import { FormGroup, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { BookingService } from '../../../core/services/booking/booking-service';
import { BookingDto } from '../../../core/models/booking.model';
import { Router } from '@angular/router';
import { CurrencyPipe, NgForOf } from '@angular/common';
import { ActivityResponseDto } from '../../../core/models/activity.model';
import { ActivityService } from '../../../core/services/activity/activity.service';
import { AuthService } from '../../../core/services/auth.service';
import { AddBookingFormOverlayComponent } from '../add-booking-form-overlay/add-booking-form-overlay.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-add-booking',
  imports: [ReactiveFormsModule, NgForOf, CurrencyPipe, AddBookingFormOverlayComponent],
  templateUrl: './add-booking.component.html',
  styleUrls: ['./add-booking.component.css']
})

export class AddBookingComponent implements OnInit, OnDestroy {
  @ViewChild(AddBookingFormOverlayComponent) overlay!: AddBookingFormOverlayComponent

  addBookingForm!: FormGroup
  isSubmitting: boolean = false
  isActivitiesLoading: boolean = false
  formData!: BookingDto
  activities: ActivityResponseDto[] = []
  isModalOpen: boolean = false

  private destroy$ = new Subject<void>()

  constructor(private bookingService: BookingService,
    private router: Router,
    private activityService: ActivityService,
    private auth: AuthService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit() {
    this.formData = {} as BookingDto;
    this.loadAllActivities()
  }

  openModal(): void {
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  private loadAllActivities() {
    this.isActivitiesLoading = true

    this.activityService.getActivities()
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (activityData) => {
        this.isActivitiesLoading = false

        this.overlay.handleSubmissionResponse(true)

        this.activities = activityData;
        this.cdr.detectChanges();

        console.log('ACTIVITY DATA FROM BACKEND: ', activityData);
      },
      error: (err) => {
        this.isActivitiesLoading = false
        this.overlay.handleSubmissionResponse(false, 'Failed to add activity. Please try again.')

        console.error('ERROR WHILE LOADING ACTIVITIES', err);
      }
    })
  }

  setActivityId(id: number): void {
    this.formData.activityId = id
    console.log('Activity ID set to:', id); // For debugging
  }

  onBookingSubmit(overlayBookingFormData: any) {
    // Merge form values into formData to preserve activityId
    Object.assign(this.formData, overlayBookingFormData);

    this.formData.customerId = Number(this.auth.loadUserFromStorage()?.id);
    this.isSubmitting = true

    this.bookingService.addBooking(this.formData)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (data) => {
        console.log('DATA FROM BACKEND: ', data);
        this.isSubmitting = false
        alert("Booking added successfully, redirecting to booking dashboard")
        this.router.navigate(['/bookings', data.bookingId])
      },
      error: (err) => {
        console.error('ERROR WHILE BOOKING: ', err);
        this.isSubmitting = false;
      }
    })
  }

  // Custom validator to check for future date
  futureDateValidator(control: AbstractControl) {
    const currentDate = new Date();
    const bookingDate = new Date(control.value);
    return bookingDate <= currentDate ? { futureDate: true } : null
  }

  getFieldError(fieldName: string, errorType: string): boolean {
    if (!this.addBookingForm) {
      return false;
    }
    const field = this.addBookingForm.get(fieldName);
    return !!(field && field.hasError(errorType) && (field.dirty || field.touched));
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
