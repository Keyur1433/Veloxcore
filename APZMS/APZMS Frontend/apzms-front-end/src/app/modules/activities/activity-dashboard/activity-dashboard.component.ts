import { Component } from '@angular/core';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { ActivityFilters, ActivityResponseDto } from '../../../core/models/activity.model';
import { ActivityService } from '../../../core/services/activity/activity.service';

@Component({
  selector: 'app-activity-dashboard',
  standalone: false,
  templateUrl: './activity-dashboard.component.html',
  styleUrls: ['./activity-dashboard.component.css']
})

export class ActivityDashboardComponent {
  // The main data stream that components will display
  activities$: Observable<ActivityResponseDto[]>;

  // Private subject to manage filter state internally
  private filterSubject = new BehaviorSubject<ActivityFilters>({})

  // Public observable for template binding (if needed)
  filters$ = this.filterSubject.asObservable()

  constructor(private activityService: ActivityService) {
    // Create the reactive pipeline: filters → API call → activities
    this.activities$ = this.filters$.pipe(
      switchMap(filters => activityService.getActivities(filters))
    );
  }

  currentFilters: any

  // Update individual filter properties
  updateAgeGroup(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const ageGroup = selectElement.value;
    this.currentFilters = this.filterSubject.value
    this.filterSubject.next({ ...this.currentFilters, ageGroup })
  }

  updateSafetyLevel(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const safetyLevel = selectElement.value;
    this.currentFilters = this.filterSubject.value
    this.filterSubject.next({ ...this.currentFilters, safetyLevel })
  }

  // Reset all filters to empty state
  clearFilters() {
    this.filterSubject.next({})
  }
}