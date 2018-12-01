import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ServiceBase } from './service-base';

@Injectable({ providedIn: 'root' })
export class AccountService extends ServiceBase {

    getAll() {
        return this.get(`${environment.idsApiUrl}/api/users`);
    }
}
