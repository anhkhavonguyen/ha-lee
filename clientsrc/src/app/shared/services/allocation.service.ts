import { Injectable } from '@angular/core';
import { ServiceBase } from './service-base';
import { StockAllocationRequest } from 'src/app/ims/allocations/allocation.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class StockAllocationService extends ServiceBase {

    add(request: StockAllocationRequest) {
        return this.post<any>(`${environment.apiUrl}/api/stockallocations`, request);
    }
}
