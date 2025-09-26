import { Component, OnDestroy, OnInit } from '@angular/core';
import { BookingService } from '../../../core/services/booking/booking-service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookingUpdateDto } from '../../../core/models/booking.model';
import { NgIf } from '@angular/common';
import { Subject, take, takeUntil } from 'rxjs';

@Component({
  selector: 'app-edit-booking',
  templateUrl: './edit-booking.component.html',
  styleUrls: ['./edit-booking.component.css'],
  imports: [ReactiveFormsModule, NgIf]
})
export class EditBookingComponent implements OnInit, OnDestroy {
  updateForm!: FormGroup
  error!: string
  isLoading!: boolean
  bookingId!: number

  private destroy$ = new Subject<void>()

  constructor(
    private bookingService: BookingService,
    private route: ActivatedRoute, 
    private router: Router,
  ) {
    this.updateForm = this.createUpdateForm()
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')

    if (id) {
      this.bookingId = +id
    }
  }

  createUpdateForm(): FormGroup {
    return new FormGroup({
      customerId: new FormControl("", Validators.required),
      activityId: new FormControl("", Validators.required),
      bookingDate: new FormControl("", Validators.required),
      participants: new FormControl("", Validators.required),
    })
  }

  onUpdate() {
    const formValues: BookingUpdateDto = this.updateForm.value
    this.isLoading = true

    this.bookingService.updateBooking(this.bookingId, formValues)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (data) => {
        console.log('DATA FROM THE BACKEND: ', data);
        alert("Booking updated, redirecting to view booking")
        this.router.navigate(['/bookings', this.bookingId])
        this.isLoading = false
      },
      error: (err) => {
        console.error('ERROR: ', err);
        this.isLoading = false
      }
    })

    this.updateForm.reset()
  }

  getFieldError(errorType: string, fieldName: string): boolean{
    const field = this.updateForm.get(fieldName)

    return !!(field?.hasError(errorType) && (field.dirty || field.touched))
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
