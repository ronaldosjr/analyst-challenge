import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { SensorEvent } from '../models/sensor-event.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private readonly ENDPOINT = environment.api + '/sensors';
  private readonly TRY_AGAIN_TIME = 3000;

  private hubConnection: signalR.HubConnection;
  private _newSensorEvent = new Subject<SensorEvent>();

  constructor() { }

  public newSensorEvent() {
    return this._newSensorEvent.asObservable();
  }

  public buildConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.ENDPOINT)
      .build();

    this.hubConnection.onclose(() => {
      this.tryAgainStart();
    });

    this.startConnection();
  }

  public startConnection = () => {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => {
        console.log('Error while starting connection: ' + err + '. Trying again in ' + this.TRY_AGAIN_TIME + 'ms');
        this.tryAgainStart();
      });
  }

  private tryAgainStart = () => {
    setTimeout(() => {
      this.startConnection()
    }, this.TRY_AGAIN_TIME);
  }

  public addTransferSensorEventDataListener = () => {
    this.hubConnection.on('sensoreventdata', (data) => {
      this._newSensorEvent.next( data );
    });
  }
}
