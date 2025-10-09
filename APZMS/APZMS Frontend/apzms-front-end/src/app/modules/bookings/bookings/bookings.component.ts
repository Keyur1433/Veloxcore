import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { BookingFilteredItemResponseDto } from '../../../core/models/booking.model';
import { BookingService } from '../../../core/services/booking/booking-service';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'app-bookings',
  standalone: false,
  templateUrl: './bookings-ag-grid.html',
  // templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.css'],
  encapsulation: ViewEncapsulation.None // Disable view encapsulation to enable global style overrides
})
export class BookingsComponent implements OnInit, OnDestroy {
  error: string = "";
  filterForm!: FormGroup;
  pageSize: number = 5;
  pageIndex: number = 1;
  pageSizeArr: number[] = [5, 10, 15, 20];

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
      
      switchMap(filters => bookingService.getFilteredBookings(filters)),

      tap(bookings => {
        // debugger
        console.log(bookings)
      })
    )
  }
  ngOnInit(): void {
    this.filterForm.get('pageSize')?.valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.filterForm.get('pageNumber')?.setValue(1);
      this.onFilter();
    });

    this.onFilter();
  }

  createFilterForm(): FormGroup {
    return new FormGroup({
      customerName: new FormControl(""),
      activityName: new FormControl(""),
      BookingDateFrom: new FormControl(""),
      BookingDateTo: new FormControl(""),
      pageNumber: new FormControl(1),
      // pageSize: new FormControl(5),
      pageSize: new FormControl(),
    })
  }

  nextPage() {
    if (this.filterForm.get('pageNumber')?.value >= 1) {
      this.pageIndex = this.filterForm.get('pageNumber')?.value + 1
      this.filterForm.get('pageNumber')?.setValue(this.pageIndex)
      this.filterSubject.next(this.filterForm.value)
    }
  }

  prevPage() {
    if (this.filterForm.get('pageNumber')?.value > 1) {
      this.pageIndex = Math.max(1, this.filterForm.get('pageNumber')?.value - 1)
      this.filterForm.get('pageNumber')?.setValue(this.pageIndex)
      this.filterSubject.next(this.filterForm.value)
    }
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
          this.filterSubject.next({ ...this.filterSubject.value, pageNumber: this.pageIndex })
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

  colDefs: ColDef[] = [
    {
      field: "id",
      headerName: "ID",
      width: 70,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "activityId",
      headerName: "Activity ID",
      width: 120,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "activityName",
      headerName: "Activity Name",
      width: 150,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "customerId",
      headerName: "Customer ID",
      width: 140,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "customerName",
      headerName: "Customer Name",
      width: 160,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "price",
      headerName: "Price",
      width: 100,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "finalPrice",
      headerName: "Final Price",
      width: 130,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "bookingDate",
      headerName: "Booking Date",
      width: 150,
      sortable: true,
      // filter: 'agNumberColumnFilter'
    },
    {
      field: "timeSlot",
      headerName: "Time Slot",
      width: 150,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "participants",
      headerName: "Participants",
      width: 130,
      sortable: true,
      filter: 'agNumberColumnFilter'
    }
  ]

  defaultColDef = {
    resizable: true,
    // sortable: false,
    // filter: false,
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
