import { RouterModule, Routes } from "@angular/router";
import { ChartDashboardComponent } from "./components/chartDashboard/chartDashboard.component";
import { LineChartComponent } from "./components/line-chart/line-chart.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
  {
    path: '',
    component: ChartDashboardComponent,
    children:
      [
        {
          path: 'line',
          component: LineChartComponent
        }
      ]
  },
  { path: '**', redirectTo: '' }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PracticeHighChartRoutingModule { }