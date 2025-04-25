import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { RouteService } from './route.service';
import { environment } from 'environments/environment';
import { FormsModule } from '@angular/forms';
import { RouteModel } from './models/RouteModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    FormsModule,
    CommonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  swaggerUrl = `${environment.apiUrlBase}/swagger/index.html`;

  showedSection = {
    searchRoutes: true,
    listRoutes: false,
    newRoute: false,
    editRoute: false,
    searchBestRoute: false,
    resultBestRoute: false,
  }

  searchFormData = {
    origin: '',
    destination: '',
  };

  routeFormData = {
    origin: '',
    destination: '',
    value: 0,
  };

  routeListResult: RouteModel[] = [];

  routeIdInEdition?: string;

  bestRouteResult?: { routes: string[], value: number };

  constructor(private routeService: RouteService) { }

  selectSection(section: string) {
    Object.keys(this.showedSection).forEach(key => this.showedSection[key as keyof typeof this.showedSection] = false);
    this.showedSection[section as keyof typeof this.showedSection] = true;
  }

  searchRoutes() {
    return this.routeService.list(this.searchFormData.origin, this.searchFormData.destination)
      .subscribe(routes => {
        this.routeListResult = routes;
        this.selectSection('listRoutes');

        this.searchFormData = {
          origin: '',
          destination: '',
        };
      });
  }

  newRoute() {
    return this.routeService.create(this.routeFormData)
      .subscribe(() => {
        this.selectSection('searchRoutes');
        this.routeFormData = {
          origin: '',
          destination: '',
          value: 0,
        };
      });
  }

  openEditRoute(route: RouteModel) {
    this.routeIdInEdition = route.id;
    this.routeFormData = {
      origin: route.origin,
      destination: route.destination,
      value: route.value,
    };
    this.selectSection('editRoute');
  }

  editRoute() {
    return this.routeService.update(this.routeIdInEdition!, this.routeFormData)
      .subscribe(() => {
        this.routeIdInEdition = undefined;
        this.selectSection('searchRoutes');
        this.routeFormData = {
          origin: '',
          destination: '',
          value: 0,
        };
      });
  }

  deleteRoute(id: string) {
    this.routeService.delete(id)
      .subscribe(() => this.selectSection('searchRoutes'));
  }

  searchBestRoute() {
    this.routeService.searchBestRoute(this.searchFormData.origin, this.searchFormData.destination)
      .subscribe(result => {
        this.bestRouteResult = result;
        this.selectSection('resultBestRoute');
        this.routeFormData = {
          origin: '',
          destination: '',
          value: 0,
        };
      });
  }
}
