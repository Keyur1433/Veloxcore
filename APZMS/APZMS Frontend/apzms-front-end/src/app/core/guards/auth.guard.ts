import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (auth.isLoggedIn()) return true;

    // Save attempted url for redirecting after login
    alert('You are not authorized to access this page or the token is expired or invalid');
    return router.createUrlTree(['/login']);
};