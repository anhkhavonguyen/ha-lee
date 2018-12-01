import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FieldModel, EntityRefModel } from 'src/app/ims/fields/field.model';
import { ServiceBase } from './service-base';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';


@Injectable({ providedIn: 'root' })
export class FieldService extends ServiceBase {

    getAllFields() {
        return this.list(`${environment.apiUrl}/api/fields/all`);
    }

    getAll(page: number = 1, numberItemsPerPage: number = 10) {
        return this.page(`${environment.apiUrl}/api/fields`, new PagingFilterCriteria(page, numberItemsPerPage));
    }

    getEntityRefList() {
        return this.list<EntityRefModel>(`${environment.apiUrl}/api/fields/ref`);
    }

    getBy(id: string): Observable<FieldModel> {
        return this.get(`${environment.apiUrl}/api/fields/${id}`);
    }

    add(field: FieldModel): Observable<FieldModel> {
        return this.post(`${environment.apiUrl}/api/fields`, field);
    }

    update(field: FieldModel): Observable<FieldModel> {
        return this.put(`${environment.apiUrl}/api/fields/${field.id}`, field);
    }

    remove(id: string) {
        return this.delete(`${environment.apiUrl}/api/fields/${id}`);
    }
}
