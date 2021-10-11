import { Component, OnInit } from '@angular/core';
import { SignalrService } from './services/signalr.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { auditTime } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Radix';

  constructor(
    private signalrService: SignalrService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.signalrService.buildConnection();
    this.signalrService.addTransferSensorEventDataListener();
    this.signalrService.newSensorEvent().pipe(
      auditTime(1000),
    ).subscribe(() => {
      this.snackBar.open('Carregando...', 'Fechar', { duration: 500 });
    });
  }
}
