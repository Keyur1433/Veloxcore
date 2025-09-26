import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BookingDto, BookingFilteredItemResponseDto, BookingResponseDto, BookingUpdateDto } from '../../models/booking.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class BookingService {
  private base = `${environment.apiBaseUrl}/bookings`

  constructor(private http: HttpClient) { }

  getFilteredBookings(paramsObj: Record<string, any>): Observable<BookingFilteredItemResponseDto[]> {
    let params = new HttpParams();

    if (paramsObj) {
      Object.keys(paramsObj).forEach(key => {
        if (paramsObj[key] !== undefined && paramsObj[key] !== null) {
          params = params.set(key, paramsObj[key].toString())
        }
      })
    }
    return this.http.get<BookingFilteredItemResponseDto[]>(this.base, { params })
  }

  getBookingById(id: number): Observable<BookingResponseDto> {
    return this.http.get<BookingResponseDto>(`${this.base}/${id}`)
  }

  addBooking(data: BookingDto): Observable<BookingResponseDto> {
    return this.http.post<BookingResponseDto>(`${this.base}`, data)
  }

  updateBooking(id: number, data: BookingUpdateDto): Observable<BookingResponseDto> {
    return this.http.put<BookingResponseDto>(`${this.base}/${id}`, data)
  }

  deleteBooking(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`)
  }
}