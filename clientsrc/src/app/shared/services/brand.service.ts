import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ServiceBase } from './service-base';
import { BrandModel } from 'src/app/ims/brands/brand.model';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';

@Injectable({ providedIn: 'root' })
export class BrandService extends ServiceBase {
    getAll(page: number = 1, numberItemsPerPage: number = 10) {
        return this.page(`${environment.apiUrl}/api/brands`, new PagingFilterCriteria(page, numberItemsPerPage));
    }

    getBy(id: string): Observable<BrandModel> {
        return this.get(`${environment.apiUrl}/api/brands/${id}`);
    }

    add(brand: BrandModel): Observable<BrandModel> {
        return this.post(`${environment.apiUrl}/api/brands`, brand);
    }

    update(brand: BrandModel): Observable<BrandModel> {
        return this.put(`${environment.apiUrl}/api/brands/${brand.id}`, brand);
    }

    remove(id: string) {
        return this.delete(`${environment.apiUrl}/api/brands/${id}`);
    }
}
