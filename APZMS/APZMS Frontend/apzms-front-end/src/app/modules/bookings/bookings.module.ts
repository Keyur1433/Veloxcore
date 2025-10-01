import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddBookingFormOverlayComponent } from './add-booking-form-overlay/add-booking-form-overlay.component';
import { ViewBookingComponent } from './view-booking/view-booking.component';
import { BookingsRoutingModule } from './bookings-routing.module';
import { BookingsComponent } from './bookings/bookings.component';
import { AddBookingComponent } from './add-booking/add-booking.component';
import { EditBookingComponent } from './edit-booking/edit-booking.component';

@NgModule({
  declarations: [
    BookingsComponent,
    AddBookingComponent,
    EditBookingComponent,
    ViewBookingComponent,
    AddBookingFormOverlayComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BookingsRoutingModule,
    FormsModule
  ]
})
export class BookingsModule { }