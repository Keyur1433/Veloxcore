import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingResponseDto, BookingRequestDto } from '../../../core/models/booking.model';
import { BookingService } from '../../../core/services/booking/booking-service';

@Component({
  selector: 'app-edit-booking',
  standalone: false,
  templateUrl: './edit-booking.component.html',
  styleUrls: ['./edit-booking.component.css']
})
export class EditBookingComponent implements OnInit {
  editForm!: FormGroup;
  bookingId!: number;
  booking?: BookingResponseDto;
  loading = false;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.editForm = this.fb.group({
      customerId: ['', Validators.required],
      activityId: ['', Validators.required],
      bookingDate: ['', [Validators.required]],
      participants: ['', [Validators.required, Validators.min(1)]],
      timeSlot: ['', Validators.required],
      status: ['pending', Validators.required]
    });
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.bookingId = idParam ? parseInt(idParam, 10) : 0;
    if (this.bookingId) {
      this.loadBooking();
    }
  }

  private loadBooking(): void {
    this.loading = true;
    this.bookingService.getBookingById(this.bookingId).subscribe({
      next: (booking) => {
        this.booking = booking;
        this.patchForm(booking);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading booking:', error);
        this.loading = false;
        // Navigate back or show error
      }
    });
  }

  private patchForm(booking: BookingResponseDto): void {
    this.editForm.patchValue({
      customerId: booking.customerId,
      activityId: booking.activityId,
      bookingDate: booking.bookingDate,
      participants: booking.participants,
      timeSlot: booking.timeSlot || '',
      status: booking.status || 'pending'
    });
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.editForm.valid && this.booking) {
      this.loading = true;
      const updateData: BookingRequestDto = {
        ...this.editForm.value,
        id: this.booking.bookingId
      };

      this.bookingService.updateBooking(this.bookingId, updateData).subscribe({
        next: (response) => {
          this.loading = false;
          this.router.navigate(['/bookings']);
        },
        error: (error) => {
          console.error('Error updating booking:', error);
          this.loading = false;
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/bookings']);
  }
}