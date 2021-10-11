import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { SensorEvent } from "../models/sensor-event.model";
import { environment } from "../../environments/environment";
import { SensorEventChartDataModel } from "../models/sensor-event-chart-data.model";

@Injectable({
  providedIn: 'root'
})
export class SensorEventService {

  private readonly API_URL = environment.api + '/api/SensorEvent';

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<SensorEvent[]>(this.API_URL);
  }

  getAggregate() {
    return this.http.get<SensorEventChartDataModel[]>(this.API_URL + '/aggregate');
  }
}
