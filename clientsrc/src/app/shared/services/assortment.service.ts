import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AssortmentModel } from 'src/app/ims/assortments/assortment.model';
import { environment } from 'src/environments/environment';
import { ServiceBase } from './service-base';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';

@Injectable({ providedIn: 'root' })
export class AssortmentService extends ServiceBase {
  getAll(page: number = 1, numberItemsPerPage: number = 10) {
    return this.page(`${environment.apiUrl}/api/assortments`, new PagingFilterCriteria(page, numberItemsPerPage));
  }

  getBy(id: string): Observable<AssortmentModel> {
    return this.get(`${environment.apiUrl}/api/assortments/${id}`);
  }

  add(assortment: AssortmentModel): Observable<AssortmentModel> {
    return this.post(`${environment.apiUrl}/api/assortments`, assortment);
  }

  update(assortment: AssortmentModel): Observable<AssortmentModel> {
    return this.put(`${environment.apiUrl}/api/assortments/${assortment.id}`, assortment);
  }

  remove(id: string) {
    return this.delete(`${environment.apiUrl}/api/assortments/${id}`);
  }
}
