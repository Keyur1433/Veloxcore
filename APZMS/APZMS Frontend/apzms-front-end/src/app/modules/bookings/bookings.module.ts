import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddBookingFormOverlayComponent } from './add-booking-form-overlay/add-booking-form-overlay.component';
import { ViewBookingComponent } from './view-booking/view-booking.component';
import { BookingsRoutingModule } from './bookings-routing.module';
import { BookingsComponent } from './bookings/bookings.component';
import { AddBookingComponent } from './add-booking/add-booking.component';
import { EditBookingComponent } from './edit-booking/edit-booking.component';
import { AgGridModule } from 'ag-grid-angular';
import { HighchartsChartModule } from 'highcharts-angular';
import { BookingsBarChartsComponent } from './charts/bookings-bar-charts/bookings-bar-charts.component';
import { BookingLineChartComponent } from './charts/booking-line-chart/booking-line-chart.component';
import { BookingPieChartComponent } from './charts/booking-pie-chart/booking-pie-chart.component';

@NgModule({
  declarations: [
    BookingsComponent,
    AddBookingComponent,
    EditBookingComponent,
    ViewBookingComponent,
    AddBookingFormOverlayComponent,
    BookingsBarChartsComponent,
    BookingLineChartComponent,
    BookingPieChartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BookingsRoutingModule,
    AgGridModule,
    CurrencyPipe,
    HighchartsChartModule
  ],
  providers: []
})
export class BookingsModule { }