import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartDashboardComponent } from './components/chartDashboard/chartDashboard.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { LineChartComponent } from './components/line-chart/line-chart.component';
import { PracticeHighChartRoutingModule } from './practice-high-chart.routes';

@NgModule({
  imports: [
    CommonModule,
    HighchartsChartModule,
    PracticeHighChartRoutingModule
  ],
  declarations: [
    ChartDashboardComponent,
    LineChartComponent
  ]
})
export class PracticeHighChartModule { }
