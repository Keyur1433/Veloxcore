import { Component, ViewEncapsulation } from '@angular/core';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { ActivityFilters, ActivityResponseDto } from '../../../core/models/activity.model';
import { ActivityService } from '../../../core/services/activity/activity.service';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'app-activity-dashboard',
  standalone: false,
  templateUrl: './activity-ag-grid.html',
  styleUrls: ['./activity-dashboard.component.css'],
  encapsulation: ViewEncapsulation.None // Disable view encapsulation to enable global style overrides
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

  // Column Definitions: Instructions for what columns to show
  colDefs: ColDef[] = [
    {
      field: "id",
      headerName: "ID",
      width: 70,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "name",
      headerName: "Name",
      width: 150,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "description",
      headerName: "Description",
      width: 200,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "price",
      headerName: "Price",
      width: 100,
      sortable: true,
      filter: 'agNumberColumnFilter',
      valueFormatter: (params) => {
        return `$${params.value}`
      }
    },
    {
      field: "capacity",
      headerName: "Capacity",
      width: 100,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "minAge",
      headerName: "Min Age",
      width: 100,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "maxAge",
      headerName: "Max Age",
      width: 100,
      sortable: true,
      filter: 'agNumberColumnFilter'
    },
    {
      field: "safetyLevel",
      headerName: "Safety Level",
      width: 120,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      field: "photoUrl",
      headerName: "Photo",
      width: 150,
      cellRenderer: (params: any) => {
        if (params.value) {
          return `<img src="${params.value}" style="width: 40px; height: 40px; object-fit: cover; border-radius: 4px;" />`;
        }
        return 'No image';
      },
      autoHeight: true
    }
  ];

  defaultColDef = {
    resizable: true,
    sortable: true,
    filter: true,
  }

  // Reset all filters to empty state
  clearFilters() {
    this.filterSubject.next({})
  }
}