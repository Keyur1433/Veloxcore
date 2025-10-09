import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { BookingService } from '../../../../core/services/booking/booking-service';
import { BookingFilteredItemResponseDto } from '../../../../core/models/booking.model';
import { map, Observable, tap } from 'rxjs';

@Component({
  selector: 'app-bookings-bar-charts',
  standalone: false,
  templateUrl: './bookings-bar-charts.component.html',
  styleUrls: ['./bookings-bar-charts.component.css']
})
export class BookingsBarChartsComponent implements OnInit {
  highCharts = Highcharts;
  chartOptions$!: Observable<any>;

  chartOptions: Highcharts.Options = {
    chart: { type: "column" },
    title: { text: "Booking volume by activity and month" },
    yAxis: [
      {
        title: { text: 'Activity Bookings' }
      }
    ]
  };

  constructor(private bookingService: BookingService) { }

  ngOnInit(): void {
    this.loadBookingData();
  }

  private loadBookingData(): void {
    this.chartOptions$ = this.bookingService.getFilteredBookings({}).pipe(
      map((bookings: BookingFilteredItemResponseDto[]) => this.processBookingData(bookings)),
      tap(bookings => {
        console.log(bookings);
      })
    )
  }

  // EXPLANATION: Go to Angular Notes/Code Logic Explanation/Code Explanations/#APZMS_1
  private processBookingData(bookings: BookingFilteredItemResponseDto[]) {
    // debugger
    const activities = new Map<string, number>();

    bookings.forEach(booking => {
      const count = activities.get(booking.activityName) ?? 0;
      activities.set(booking.activityName, count + 1);
    });

    const barData = Array.from(activities.values());
    const activityNames = Array.from(activities.keys());

    // Line Chart - Bookings per month
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
      xAxis: [
        {
          categories: activityNames,
          title: { text: "Activities" }
        }
      ],

      series: [
        { name: 'Bookings by activity', data: barData, type: "column",  },
        { name: 'Bookings per month', data: bookingsPerMonths, type: "line",  },
      ]
    }
  }
}