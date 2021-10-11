import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { SensorTableComponent } from './sensor-table/sensor-table.component';
import { SensorChartComponent } from './sensor-chart/sensor-chart.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'table'
      },
      {
        path: 'table',
        component: SensorTableComponent
      },
      {
        path: 'chart',
        component: SensorChartComponent,
      },
      {
        path: '**',
        redirectTo: 'table'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
