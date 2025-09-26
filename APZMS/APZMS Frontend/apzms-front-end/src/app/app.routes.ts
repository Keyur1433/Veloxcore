import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { authGuard } from './core/guards/auth.guard';
import { ViewBookingComponent } from './components/bookings/view-booking/view-booking.component';
import { EditBookingComponent } from './components/bookings/edit-booking/edit-booking.component';
import { BookingsComponent } from './components/bookings/bookings-dashboard/bookings.component';
import { AddActivityComponent } from './components/activities/add-activity/add-activity.component';
import { AddBookingComponent } from './components/bookings/add-booking/add-booking.component';
import { ActivityDashboardComponent } from './components/activities/activity-dashboard/activity-dashboard.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
        pathMatch: 'full'
    },
    {
        path: 'register',
        component: RegistrationComponent,
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        component: DashboardComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'activities',
        component: ActivityDashboardComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'activities/add',
        component: AddActivityComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'bookings',
        component: BookingsComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'bookings/add',
        component: AddBookingComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'bookings/:id',
        component: ViewBookingComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
    {
        path: 'bookings/:id/edit',
        component: EditBookingComponent,
        pathMatch: 'full',
        canActivate: [authGuard]
    },
];
