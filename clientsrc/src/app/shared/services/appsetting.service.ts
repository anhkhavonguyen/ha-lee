import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AppSettingModel } from '../base-model/appsetting.model';
import { ServiceBase } from './service-base';


@Injectable({ providedIn: 'root' })
export class AppSettingService extends ServiceBase {
    getAll() {
        return this.list(`${environment.apiUrl}/api/appsettings`);
    }

    getBy(id: string): Observable<AppSettingModel> {
        return this.get(`${environment.apiUrl}/api/appsettings/${id}`);
    }

    add(appsetting: AppSettingModel): Observable<AppSettingModel> {
        return this.post(`${environment.apiUrl}/api/appsettings`, appsetting);
    }

    update(appsetting: AppSettingModel): Observable<AppSettingModel> {
        return this.put(`${environment.apiUrl}/api/appsettings/${appsetting.key}`, appsetting);
    }

    remove(id: string) {
        return this.delete(`${environment.apiUrl}/api/appsettings/${id}`);
    }
}
