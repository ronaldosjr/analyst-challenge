import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { SensorEventService } from '../services/sensor-event.service';
import { SensorEvent } from '../models/sensor-event.model';
import { SignalrService } from '../services/signalr.service';
import { auditTime, map, scan, tap } from 'rxjs/operators';
import { concat, Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-sensor-table',
  templateUrl: './sensor-table.component.html',
  styleUrls: ['./sensor-table.component.scss']
})
export class SensorTableComponent implements OnInit, OnDestroy {
  dataSource: MatTableDataSource<SensorEvent>;
  displayedColumns = ['id', 'timestamp', 'tag', 'processed', 'valor'];

  private readonly MAX_ITEMS = 1000;
  private readonly POLLING_TIME = 1000;
  private pollingSubscription: Subscription;

  constructor(
    private sensorEventService: SensorEventService,
    private signalrService: SignalrService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource<SensorEvent>([]);
    this.startPolling();
  }

  ngOnDestroy(): void {
    this.pollingSubscription?.unsubscribe();
  }

  startPolling(): void {
    const ref = this.snackBar.open('Carregando dados...', 'Fechar', { duration: 2000 });
    const getApi$ = this.sensorEventService.getAll();
    const getSignalR$ = this.signalrService.newSensorEvent().pipe(
      map(x => [x])
    );
    this.pollingSubscription = concat(getApi$, getSignalR$).pipe(
      scan((acc, cur) => cur.concat( acc ).slice(0, this.MAX_ITEMS), []),
      auditTime(this.POLLING_TIME),
    ).subscribe(x => this.dataSource = new MatTableDataSource<SensorEvent>(x));
  }
}
