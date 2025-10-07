import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { map, Observable, tap } from 'rxjs';
import { BookingService } from '../../../../core/services/booking/booking-service';
import { BookingFilteredItemResponseDto } from '../../../../core/models/booking.model';

@Component({
  selector: 'app-booking-line-chart',
  standalone: false,
  templateUrl: './booking-line-chart.component.html',
  styleUrls: ['./booking-line-chart.component.css']
})
export class BookingLineChartComponent implements OnInit {
  highCharts = Highcharts
  bookings$!: Observable<any>

  chartOptions: Highcharts.Options = {
    chart: {
      type: "line"
    },
    title: {
      text: "Monthly Booking Counts"
    },
    xAxis: {
      title: {
        text: "Months"
      },
      categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    },
    yAxis: {
      title: {
        text: "Bookings"
      }
    },
    series: [{
      type: "line",
      data: []
    }]
  }

  constructor(private bookingService: BookingService) { }

  ngOnInit() {
    this.loadBookingData()
  }

  private loadBookingData() {
    this.bookings$ = this.bookingService.getFilteredBookings({}).pipe(
      map((bookings: BookingFilteredItemResponseDto[]) => this.processBookingMonths(bookings)),
      tap(booking => {
        // debugger;
      })
    )
  }

  private processBookingMonths(bookings: BookingFilteredItemResponseDto[]) {
    const monthWithBookingCount = new Map<number, number>()

    bookings.forEach(booking => {
      const count = monthWithBookingCount.get(new Date(booking.bookingDate).getMonth()) ?? 0
      monthWithBookingCount.set(new Date(booking.bookingDate).getMonth(), count + 1)
    });

    // debugger
    const monthWithBookingCountEntries = [...monthWithBookingCount]
    monthWithBookingCountEntries.sort((a, b) => a[0] - b[0])
    const monthWithBookingCountSorted = monthWithBookingCountEntries

    const bookingsPerMonths = Array.from(monthWithBookingCountSorted.values())

    return {
      ...this.chartOptions,
      series: [{
        type: "line",
        data: bookingsPerMonths
      }]
    }
  }
}