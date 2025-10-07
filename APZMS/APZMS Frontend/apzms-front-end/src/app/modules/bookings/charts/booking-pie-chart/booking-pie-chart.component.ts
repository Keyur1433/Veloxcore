import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../../../core/services/booking/booking-service';
import { map, Observable, tap } from 'rxjs';
import { BookingFilteredItemResponseDto } from '../../../../core/models/booking.model';
import * as Highcharts from 'highcharts'

@Component({
  selector: 'app-booking-pie-chart',
  standalone: false,
  templateUrl: './booking-pie-chart.component.html',
  styleUrls: ['./booking-pie-chart.component.css']
})
export class BookingPieChartComponent implements OnInit {
  highCharts = Highcharts
  bookings$!: Observable<any>
  totalBookingCounts: number = 0
  timeSlotTypeData: timeSlot[] = []

  constructor(private bookingService: BookingService) { }

  ngOnInit() {
    this.loadBookingData()
  }

  chartOptions: Highcharts.Options = {
    chart: {
      type: "pie"
    },
    title: {
      text: "Time slot type distribution"
    },
    series: [{
      type: "pie",
      name: "Time slot type division",
      data: []
    }],
     tooltip: {
      pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,  // Allow users to click on slices
        // cursor: 'pointer',
        dataLabels: {
          // enabled: true,  // Show labels on each slice
          format: '<b>{point.name}</b>: {point.percentage:.1f}%'
        }
      }
    },
  }

  private loadBookingData() {
    this.bookings$ = this.bookingService.getFilteredBookings({}).pipe(
      map((bookings: BookingFilteredItemResponseDto[]) => this.processTimeSlotType(bookings)),
      tap(booking => {
        // debugger;
      })
    )
  }

  private processTimeSlotType(bookings: BookingFilteredItemResponseDto[]) {
    const timeSlotType = new Map<string, number>()
    bookings.forEach(bookings => {
      this.totalBookingCounts++
      const count = timeSlotType.get(bookings.timeSlotType) ?? 0
      timeSlotType.set(bookings.timeSlotType, count + 1)
    })

    // debugger;
    timeSlotType.forEach((value, key) => {
      const percentage = (value / this.totalBookingCounts) * 100
      timeSlotType.set(key, percentage)
    })

    // EXPLANATION: Go to Angular Notes/Code Logic Explanation/Code Explanations/#APZMS_2
    this.timeSlotTypeData = Array.from(timeSlotType, ([name, y]) => ({
      name,
      y
    }))

    return {
      ...this.chartOptions,
      series: [{
        data: this.timeSlotTypeData
      }]
    }
  }
}

interface timeSlot {
  name: string
  y: number
}