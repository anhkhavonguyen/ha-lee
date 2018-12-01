import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import {
    ActivitiesRequest, ActivitiesResponse,
    GetHistoryChangeNumberCustomerRequest,
    GetHistoryChangeNumberCustomerResponse,
    GetHistoryCustomerActivitiesRequest,
    GetHistoryCustomerActivitiesResponse,
    FilterHistoryCustomerActivitiesResponse,
    FilterHistoryCustomerActivitiesRequest
} from './activity.model';

const resourceUrl = 'api/Activity/gets';
const getHistoryChangeMobileUrl = 'api/Activity/getHistoryChangeNumberCustomer';
const getHistoryCustomerActivities = 'api/Activity/getHistoryCustomerActivities';
const getHistoryActivatedCustomerActivities = 'api/Activity/getHistoryActivatedCustomerActivities';
const getHistoryDeactivatedCustomerActivities = 'api/Activity/getHistoryDeactivatedCustomerActivities';

@Injectable()
export class ActivityService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${resourceUrl}`;

    public getActivities(request: ActivitiesRequest): Observable<ActivitiesResponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public getHistoryChangeNumberCustomer(request: GetHistoryChangeNumberCustomerRequest)
        : Observable<GetHistoryChangeNumberCustomerResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${getHistoryChangeMobileUrl}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public getHistoryCustomerActivities(request: GetHistoryCustomerActivitiesRequest)
        : Observable<GetHistoryCustomerActivitiesResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${getHistoryCustomerActivities}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public getHistoryActivatedCustomerActivities(request: FilterHistoryCustomerActivitiesRequest)
        : Observable<FilterHistoryCustomerActivitiesResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${getHistoryActivatedCustomerActivities}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }

    public getHistoryDeactivatedCustomerActivities(request: FilterHistoryCustomerActivitiesRequest)
        : Observable<FilterHistoryCustomerActivitiesResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${getHistoryDeactivatedCustomerActivities}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }
}
