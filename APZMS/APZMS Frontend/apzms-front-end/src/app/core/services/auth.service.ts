import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { LoginResponseDto, User, UserRegisterDto } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  constructor(private http: HttpClient, private router: Router) { }

  // Holds the current user state and allows subscribers to react to login/logout
  private currentUserSubject = new BehaviorSubject<User | null>(this.loadUserFromStorage())
  currentUser$ = this.currentUserSubject.asObservable()

  // LOGIN: call backend and map response into our frontend `User` model
  login(credentials: { email: string; password: string }): Observable<User> {
    // NOTE: endpoint path matches backend pattern: {apiBaseUrl}/v1/auth/login
    return this.http.post<LoginResponseDto>(`${environment.apiBaseUrl}/auth/login`, credentials).pipe(
      map((res) => this.mapLoginResponseToUser(res)),
      tap((user) => this.persistUser(user))
    )
  }

  // Map backend DTO to the frontend `User` object. Keep mapping in one place so changes in backend fields only require changes here.
  private mapLoginResponseToUser(res: LoginResponseDto): User {
    const token = res.data.accessToken as string

    // Parse token payload to extract expiry and possibly claims like name/role.
    const payload = this.parseJwt(token)

    // Claim names vary; try common claim keys then fallback to DTO values
    const expiresAt = payload && payload.exp ? payload.exp : Math.floor(Date.now() / 1000) + 3600
    const name = payload?.unique_name || payload?.name || res.data.customerName || 'Unknown'
    const role = payload?.role || res.data.role || 'customer'
    const id = payload?.sub || res.data.customerId || ''

    return {
      id,
      name,
      role,
      token,
      expiresAt
    }
  }

  // Read user from localStorage during app bootstrap
  public loadUserFromStorage(): User | null {
    try {
      const raw = localStorage.getItem('currentUser')

      if (!raw) return null

      const parsed: User = JSON.parse(raw)

      // if token expired — don't keep it
      if (this.isTokenExpired(parsed.token)) {
        this.clearStorage()
        return null
      }

      return parsed
    } catch (error) {
      return null
    }
  }

  private persistUser(user: User) {
    localStorage.setItem('token', user.token) // quick access by interceptor
    localStorage.setItem('currentUser', JSON.stringify(user))
    this.currentUserSubject.next(user)
  }

  // Public getter used by interceptor to attach token
  getToken(): string | null {
    return localStorage.getItem('token')
  }

  isLoggedIn(): boolean {
    const token = this.getToken()

    return !!token && !this.isTokenExpired(token)
  }

  // Check expiry using JWT payload `exp` claim (in seconds)
  isTokenExpired(token: string | null): boolean {
    if (!token) return true

    const payload = this.parseJwt(token)

    if (!payload || !payload.exp) return true

    const nowSeconds = Math.floor(Date.now() / 1000)

    return payload.exp < nowSeconds
  }

  private parseJwt(token: string | null): any | null {
    try {
      const parts = token?.split('.')

      if (parts?.length !== 3) return null

      const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/');

      const json = decodeURIComponent(atob(payload)
        .split('').map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
      );
      return JSON.parse(json)
    } catch (error) {
      return null
    }
  }

  private clearStorage() {
    localStorage.removeItem('token')
    localStorage.removeItem('currentUser')
  }

  logout(redirectUrl = '/login') {
    this.clearStorage()
    this.currentUserSubject.next(null)
    this.router.navigate([redirectUrl])
  }

  // Registration
  register(userData: UserRegisterDto): Observable<any> {
    return this.http.post(`${environment.apiBaseUrl}/auth/register`, userData)
  }
}
