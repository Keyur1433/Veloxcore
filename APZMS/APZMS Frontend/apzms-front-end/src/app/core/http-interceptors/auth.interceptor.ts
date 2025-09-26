import { inject } from '@angular/core';
import { HttpRequest, HttpHandlerFn, HttpEventType } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { catchError, Observable, throwError, filter, tap } from 'rxjs';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<any> {
  const auth = inject(AuthService);
  const token = auth.getToken();
  
  let request = req;

  if (token) {
    request = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  
  return next(request).pipe(
    tap(event => {
      // Only log actual responses, not progress events
      if (event.type === HttpEventType.Response) {
      } else {
      }
    }),
    catchError((error) => {
      if (error.status === 401) {
        auth.logout();
      }
      return throwError(() => error);
    })
  );
}
