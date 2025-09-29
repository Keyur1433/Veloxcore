import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import { BookingFilteredItemResponseDto } from '../../../core/models/booking.model';
import { BookingService } from '../../../core/services/booking/booking-service';

@Component({
  selector: 'app-bookings',
  standalone: false,
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.css'],
})

export class BookingsComponent implements OnDestroy {
  error: string = ""
  filterForm!: FormGroup
  isLoading!: boolean

  bookings$!: Observable<BookingFilteredItemResponseDto[]>
  private filterSubject = new BehaviorSubject<any>({})

  filters$ = this.filterSubject.asObservable()
  private destroy$ = new Subject<void>()

  constructor(
    private bookingService: BookingService,
    private router: Router,
  ) {
    this.filterForm = this.createFilterForm()

    this.bookings$ = this.filters$.pipe(
      switchMap(filters => bookingService.getFilteredBookings(filters))
    )
  }

  createFilterForm(): FormGroup {
    return new FormGroup({
      customerName: new FormControl(""),
      activityName: new FormControl(""),
      BookingDateFrom: new FormControl(""),
      BookingDateTo: new FormControl(""),
    })
  }

  onFilter() {
    this.filterSubject.next(this.filterForm.value)
  }

  performDeleteBooking(id: number) {
    if (!confirm("Are you sure do you want to delete booking")) return

    this.bookingService.deleteBooking(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          alert("Booking deleted successfully")
          this.filterSubject.next(this.filterSubject.value)
        },
        error: (err) => {
          console.error('ERROR: ', err);
        }
      })
  }

  viewBooking(id: number) {
    this.router.navigate(['/bookings', id])
  }

  redirectToEdit(id: number) {
    this.router.navigate(['/bookings', id, 'edit'])
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}