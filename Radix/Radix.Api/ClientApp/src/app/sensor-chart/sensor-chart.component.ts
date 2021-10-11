import { Component, OnDestroy, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { SensorEventService } from '../services/sensor-event.service';
import { SignalrService } from '../services/signalr.service';
import { auditTime, startWith } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sensor-chart',
  templateUrl: './sensor-chart.component.html',
  styleUrls: ['./sensor-chart.component.scss']
})
export class SensorChartComponent implements OnInit, OnDestroy {

  private POLLING_TIME = 3000;

  public barChartOptions: ChartOptions = {
    responsive: true,
  };
  public barChartLabels: Label[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [];
  public barChartData: ChartDataSets[] = [
    { data: [], label: 'Total de eventos' }
  ];

  private getChartDataSubscription: Subscription;
  private pollingSubscription: Subscription;

  constructor(
    private sensorEventService: SensorEventService,
    private signalrService: SignalrService
  ) { }

  ngOnInit(): void {
    this.startPolling();
  }

  ngOnDestroy(): void {
    this.pollingSubscription?.unsubscribe();
    this.getChartDataSubscription?.unsubscribe();
  }

  startPolling(): void {
    this.pollingSubscription = this.signalrService.newSensorEvent().pipe(
      auditTime(this.POLLING_TIME),
      startWith({}),
    ).subscribe(() => this.getChartData());
  }

  getChartData(): void {
    this.getChartDataSubscription?.unsubscribe();
    this.getChartDataSubscription = this.sensorEventService.getAggregate().subscribe(data => {
      this.barChartLabels = data.map(x => x.tag);
      this.barChartData[0].data = data.map(x => x.total);
    });
  }

}
