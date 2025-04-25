import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { RouteModel } from './models/RouteModel';

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private apiUrlBase = `${environment.apiUrlBase}/Route`;

  constructor(private httpClient: HttpClient) { }

  list(origin: string, destination: string) {
    const params = new HttpParams();
    if (origin) params.set('origin', origin);
    if (destination) params.set('destination', destination);

    return this.httpClient.get<RouteModel[]>(this.apiUrlBase, { params });
  }

  create(route: RouteModel) {
    return this.httpClient.post(this.apiUrlBase, route);
  }

  update(id: string, route: RouteModel) {
    return this.httpClient.put(`${this.apiUrlBase}/${id}`, route);
  }

  delete(id: string) {
    return this.httpClient.delete(`${this.apiUrlBase}/${id}`);
  }

  searchBestRoute(origin: string, destination: string) {
    return this.httpClient.get<{ routes: string[], value: number }>(`${this.apiUrlBase}/SearchBestRoute/${origin}/${destination}`);
  }
}
