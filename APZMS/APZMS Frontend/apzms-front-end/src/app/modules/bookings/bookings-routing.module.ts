import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewBookingComponent } from './view-booking/view-booking.component';
import { EditBookingComponent } from './edit-booking/edit-booking.component';
import { AddBookingComponent } from './add-booking/add-booking.component';
import { BookingsComponent } from './bookings/bookings.component';

const routes: Routes = [
  { path: '', component: BookingsComponent },
  { path: 'add', component: AddBookingComponent },
  { path: ':id/edit', component: EditBookingComponent },
  { path: ':id', component: ViewBookingComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookingsRoutingModule { }