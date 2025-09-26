import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../../core/services/booking/booking-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, switchMap } from 'rxjs';
import { BookingFilteredItemResponseDto, BookingPatchDto, BookingResponseDto } from '../../../core/models/booking.model';
import { AsyncPipe, CurrencyPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-edit-booking',
  templateUrl: './view-booking.component.html',
  styleUrls: ['./view-booking.component.css'],
  imports: [CurrencyPipe, AsyncPipe, NgIf]
})

export class ViewBookingComponent implements OnInit {
  bookings$: Observable<BookingResponseDto> | undefined
  error: string = ""
  activityId!: number
  isSubmitting!: boolean
  successMessage!: string
  bookingsList: BookingFilteredItemResponseDto[] = []
  filters: Record<string, any> = {}

  constructor(
    private bookingService: BookingService,
    private route: ActivatedRoute, private router: Router
  ) { }

  ngOnInit() {
    // A reactive way to get the booking details when the route parameter changes
    this.bookings$ = this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id')

        if (id) {
          return this.bookingService.getBookingById(+id)
        } else {
          // Handle case where ID is missing, maybe redirect
          this.error = 'No booking ID provided.';
          this.router.navigate(['/bookings']);
          return []; // return an empty observable
        }
      })
    )

    // Get activity ID from route
    const id = this.route.snapshot.paramMap.get('id')

    if (id) {
      this.activityId = +id
    }
  }

  onSubmit() {
    // Create update object with only non-empty fields
    const dataToUpdate: BookingPatchDto = {}

    // Check if there's anything to update
    if (Object.keys(dataToUpdate).length === 0) {
      this.error = 'Please provide at least one field to update.';
      return;
    }
  }

  cancel(): void {
    this.router.navigate(['/bookings']);
  }

  goBack(): void {
    this.router.navigate(['/bookings']);
  }
}
