import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit, signal } from '@angular/core'; // Removed ChangeDetectorRef
import { FormGroup, AbstractControl } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';
import { ActivityResponseDto } from '../../../core/models/activity.model';
import { BookingDto } from '../../../core/models/booking.model';
import { ActivityService } from '../../../core/services/activity/activity.service';
import { AuthService } from '../../../core/services/auth.service';
import { BookingService } from '../../../core/services/booking/booking-service';
import { AddBookingFormOverlayComponent } from '../add-booking-form-overlay/add-booking-form-overlay.component';

@Component({
  selector: 'app-add-booking',
  standalone: false,
  templateUrl: './add-booking.component.html',
  styleUrls: ['./add-booking.component.css']
})

export class AddBookingComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(AddBookingFormOverlayComponent) overlay!: AddBookingFormOverlayComponent

  addBookingForm!: FormGroup
  isSubmitting = signal(false)
  isActivitiesLoading = signal(false)
  formData!: BookingDto
  activities = signal<ActivityResponseDto[]>([]) 
  isModalOpen = signal(false) 

  private destroy$ = new Subject<void>()

  constructor(
    private bookingService: BookingService,
    private router: Router,
    private activityService: ActivityService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.formData = {} as BookingDto;
  }

  ngAfterViewInit() {
    this.loadAllActivities();
  }

  openModal(): void {
    this.isModalOpen.set(true);
  }

  closeModal(): void {
    this.isModalOpen.set(false);
  }

  private loadAllActivities() {
    this.isActivitiesLoading.set(true)

    this.activityService.getActivities()
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (activityData) => {
        this.isActivitiesLoading.set(false)

        this.overlay.handleSubmissionResponse(true)

        this.activities.set(activityData);

        console.log('ACTIVITY DATA FROM BACKEND: ', activityData);
      },
      error: (err) => {
        this.isActivitiesLoading.set(false)
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
    this.isSubmitting.set(true)

    this.bookingService.addBooking(this.formData)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (data) => {
        console.log('DATA FROM BACKEND: ', data);
        this.isSubmitting.set(false)
        alert("Booking added successfully, redirecting to booking dashboard")
        this.router.navigate(['/bookings', data.bookingId])
      },
      error: (err) => {
        console.error('ERROR WHILE BOOKING: ', err);
        this.isSubmitting.set(false)
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