import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        loadChildren: () => import('./modules/login/login.module').then(m => m.LoginModule)
    },
    {
        path: 'register',
        loadChildren: () => import('./modules/registration/registration.module').then(m => m.RegistrationModule)
    },
    {
        path: 'dashboard',
        loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule),
        canActivate: [authGuard]
    },
    {
        path: 'activities',
        loadChildren: () => import('./modules/activities/activities.module').then(m => m.ActivitiesModule),
        canActivate: [authGuard]
    },
    {
        path: 'bookings',
        loadChildren: () => import('./modules/bookings/bookings.module').then(m => m.BookingsModule),
        canActivate: [authGuard]
    },
    {
        path: 'charts',
        loadChildren: () => import('./modules/practice-high-chart/practice-high-chart.module').then(m => m.PracticeHighChartModule),
        canActivate: [authGuard]
    },
];