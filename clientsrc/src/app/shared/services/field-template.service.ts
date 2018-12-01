import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FieldTemplateModel } from 'src/app/ims/field-templates/field-template.model';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';
import { ServiceBase } from './service-base';


@Injectable({ providedIn: 'root' })
export class FieldTemplateService extends ServiceBase {

    getAllFieldTemplates() {
        return this.list(`${environment.apiUrl}/api/fields/alltemplates`);
    }

    getAll(page: number = 1, numberItemsPerPage: number = 10) {
        // tslint:disable-next-line:max-line-length
        return this.page<FieldTemplateModel>(`${environment.apiUrl}/api/fields/templates`, new PagingFilterCriteria(page, numberItemsPerPage));
    }

    getBy(id: string): Observable<FieldTemplateModel> {
        return this.get(`${environment.apiUrl}/api/fields/templates/${id}`);
    }

    add(field: FieldTemplateModel): Observable<FieldTemplateModel> {
        return this.post<FieldTemplateModel>(`${environment.apiUrl}/api/fields/templates`, field);
    }

    update(field: FieldTemplateModel): Observable<FieldTemplateModel> {
        return this.put<FieldTemplateModel>(`${environment.apiUrl}/api/fields/templates/${field.id}`, field);
    }

    remove(id: string) {
        return this.delete(`${environment.apiUrl}/api/fields/templates/${id}`);
    }

    getTypes(): Observable<Array<string>> {
        return this.get<Array<string>>(`${environment.apiUrl}/api/fields/templates/types`);
    }
}
