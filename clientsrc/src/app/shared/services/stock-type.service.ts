import { Injectable } from '@angular/core';
import { ServiceBase } from './service-base';
import { Observable } from 'rxjs';
import { StockTypeModel } from 'src/app/ims/allocations/allocation.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StockTypeService extends ServiceBase {

  getAllWithoutPaging(): Observable<Array<StockTypeModel>> {
    return this.list(`${environment.apiUrl}/api/{stocktypes}`);
  }
}
