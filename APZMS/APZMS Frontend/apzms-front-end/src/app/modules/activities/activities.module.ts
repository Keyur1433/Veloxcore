import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivitiesRoutingModule } from './activities-routing.module';
import { ActivityDashboardComponent } from './activity-dashboard/activity-dashboard.component';
import { AddActivityComponent } from './add-activity/add-activity.component';

@NgModule({
  declarations: [
    ActivityDashboardComponent,
    AddActivityComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ActivitiesRoutingModule
  ],
  exports: [
    ActivityDashboardComponent,
    AddActivityComponent
  ]
})
export class ActivitiesModule { }