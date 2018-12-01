import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '../../shared/services/http.service';
import { environment } from '../../../environments/environment';
import HttpParamsHelper from '../../shared/helper/http-params.helper';
import { GetOutletsRequest, GetOutletsReponse, OutletModel } from './outlet.model';

const OutletResourceUrl = 'api/outlets/getOutletsWithStoreAccount';
const UpdateOutletResourceUrl = 'api/outlets/updateOutlet';

@Injectable()
export class OutletService {

    private apiUrl = `${environment.apiGatewayUri}/${OutletResourceUrl}`;

    constructor(private httpService: HttpService) { }

    getOutlets(request: GetOutletsRequest): Observable<GetOutletsReponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }

    updateOutlets(request: OutletModel): Observable<string> {
        const apiUrl = `${environment.apiGatewayUri}/${UpdateOutletResourceUrl}`;
        return this.httpService.post(apiUrl, request);
    }
}
