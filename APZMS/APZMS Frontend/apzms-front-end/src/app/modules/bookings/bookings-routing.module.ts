import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewBookingComponent } from './view-booking/view-booking.component';
import { EditBookingComponent } from './edit-booking/edit-booking.component';
import { AddBookingComponent } from './add-booking/add-booking.component';
import { BookingsComponent } from './bookings/bookings.component';
import { BookingsBarChartsComponent } from './charts/bookings-bar-charts/bookings-bar-charts.component';
import { BookingLineChartComponent } from './charts/booking-line-chart/booking-line-chart.component';
import { BookingPieChartComponent } from './charts/booking-pie-chart/booking-pie-chart.component';

const routes: Routes = [
  { path: '', component: BookingsComponent },
  { path: 'bookingBarChart', component: BookingsBarChartsComponent },
  { path: 'bookingLineChart', component: BookingLineChartComponent },
  { path: 'bookingPieChart', component: BookingPieChartComponent },
  { path: 'add', component: AddBookingComponent },
  { path: ':id/edit', component: EditBookingComponent },
  { path: ':id', component: ViewBookingComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookingsRoutingModule { }