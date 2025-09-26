import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ActivityFilters, ActivityResponseDto, AddActivityDto } from '../../models/activity.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  addActivity(formData: FormData): Observable<any> {
    return this.http.post(`${environment.apiBaseUrl}/activities`, formData);
  }

  getActivities(filters?: ActivityFilters): Observable<ActivityResponseDto[]> {
    const params = this.buildQueryParams(filters);
    
    return this.http.get<{ activities: ActivityResponseDto[] }>(`${environment.apiBaseUrl}/activities`, { params })
      .pipe(
        map(response => {
          console.log('Service: HTTP response:', response);
          return response.activities;
        })
      );
  } 

  private buildQueryParams(filters?: ActivityFilters): HttpParams {
    let params = new HttpParams();
    
    if (!filters) return params;

    // Build parameters conditionally - only add if value exists
    if (filters.ageGroup) {
      params = params.set('ageGroup', filters.ageGroup);
    } 

    if (filters.safetyLevel) {
      params = params.set('safetyLevel', filters.safetyLevel);
    } 

    return params;
  } 
} 
