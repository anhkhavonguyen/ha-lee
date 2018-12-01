import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ServiceBase } from './service-base';
import { LocationModel, LocationType } from 'src/app/ims/locations/location.model';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';

@Injectable({ providedIn: 'root' })
export class LocationService extends ServiceBase {

  getAllWithoutPaging(): Observable<Array<LocationModel>> {
    return this.list(`${environment.apiUrl}/api/locations/all`);
  }
  getAll(page: number = 1, numberItemsPerPage: number = 10) {
    return this.page(`${environment.apiUrl}/api/locations`, new PagingFilterCriteria(page, numberItemsPerPage));
  }

  getBy(id: string): Observable<LocationModel> {
    return this.get(`${environment.apiUrl}/api/locations/${id}`);
  }

  getByType(type: LocationType): Observable<Array<LocationModel>> {
    return this.list(`${environment.apiUrl}/api/locations/type/${type}`);
  }

  add(location: LocationModel): Observable<LocationModel> {
    return this.post(`${environment.apiUrl}/api/locations`, location);
  }

  update(location: LocationModel): Observable<LocationModel> {
    return this.put(`${environment.apiUrl}/api/locations/${location.id}`, location);
  }

  remove(id: string) {
    return this.delete(`${environment.apiUrl}/api/locations/${id}`);
  }
}
