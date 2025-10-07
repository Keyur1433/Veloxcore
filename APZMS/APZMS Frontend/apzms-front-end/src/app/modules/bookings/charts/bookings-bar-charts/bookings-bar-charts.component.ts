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

  chartOptions: any = {
    chart: { type: "column" },
    title: { text: "Booking volume by activity" },
    xAxis: { categories: [] },
    yAxis: {
      min: 0,
      title: {
        text: 'Number of Bookings'
      }
    },
    series: [{
      name: 'Bookings',
      data: []
    }]
  };

  constructor(private bookingService: BookingService) { }

  ngOnInit(): void {
    this.loadBookingData();
  }

  private loadBookingData(): void {
    this.chartOptions$ = this.bookingService.getFilteredBookings({}).pipe(
       tap(bookings => {
        console.log(bookings);
      }),
      map((bookings: BookingFilteredItemResponseDto[]) => this.processBookingData(bookings)),
      tap(bookings => {
        console.log(bookings);
      })
    )
  }

  // EXPLANATION: Go to Angular Notes/Code Logic Explanation/Code Explanations/#APZMS_1
  private processBookingData(bookings: BookingFilteredItemResponseDto[]) {
    // debugger
    const activityCounts = new Map<string, number>();
    bookings.forEach(booking => {
      const count = activityCounts.get(booking.activityName) ?? 0;
      activityCounts.set(booking.activityName, count + 1);
    });

    const categories = Array.from(activityCounts.keys());
    const data = Array.from(activityCounts.values());

    return {
      chart: { type: 'column' },
      title: { text: 'Booking volume by activity' },
      xAxis: { categories },
      yAxis: {
        min: 0,
        title: { text: 'Number of bookings' }
      },
      series: [{ name: 'Bookings', data }]
    }
  }
}