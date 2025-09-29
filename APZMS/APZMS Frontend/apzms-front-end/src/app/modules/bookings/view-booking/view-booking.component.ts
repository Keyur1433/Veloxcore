import { Component, OnInit } from '@angular/core';
// import { BookingService } from '../../core/services/booking/booking.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, switchMap } from 'rxjs';
import { BookingResponseDto } from '../../../core/models/booking.model';
import { BookingService } from '../../../core/services/booking/booking-service';
// import { BookingResponseDto } from '../../core/models/booking/booking-response-dto.model';

@Component({
  selector: 'app-view-booking',
  standalone: false,
  templateUrl: './view-booking.component.html',
  styleUrls: ['./view-booking.component.css']
})
export class ViewBookingComponent implements OnInit {
  booking$?: Observable<BookingResponseDto>;
  error: string = '';
  bookingId!: string;

  constructor(
    private bookingService: BookingService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.bookingId = this.route.snapshot.paramMap.get('id') || '';
    if (this.bookingId) {
      this.loadBooking();
    } else {
      this.error = 'No booking ID provided.';
      this.router.navigate(['/bookings']);
    }
  }

  private loadBooking(): void {
    this.booking$ = this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');
        if (id) {
          return this.bookingService.getBookingById(+id);
        } else {
          this.error = 'No booking ID provided.';
          this.router.navigate(['/bookings']);
          return new Observable<never>();
        }
      })
    );
  }

  goBack(): void {
    this.router.navigate(['/bookings']);
  }
}