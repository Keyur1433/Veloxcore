import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityDashboardComponent } from './activity-dashboard/activity-dashboard.component';
import { AddActivityComponent } from './add-activity/add-activity.component';

const routes: Routes = [
  { path: '', component: ActivityDashboardComponent },
  { path: 'add', component: AddActivityComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ActivitiesRoutingModule { }