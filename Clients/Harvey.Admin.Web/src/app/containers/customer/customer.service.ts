import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { CustomersRequest, CustomersResponse } from 'src/app/containers/customer/customer.model';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import { Subject } from 'rxjs';
import { ReactiveCustomerRequest } from 'src/app/containers/customer/reactive-customer/reactive-customer.model';
import { ChangeCustomerMobileRequest, ChangeCustomerInfoRequest } from './edit-customer-info/edit-customer-info.model';
import { GetCustomersByCustomerCodesRequest, GetCustomersByCustomerCodesResponse } from '../homepage/homepage.model';

const ReactiveCustomerResourceUrl = 'api/Customers/reactiveCustomer';
const CustomersResourceUrl = 'api/customers/gets';
const CustomersByCustomerCodesResourceUrl = 'api/customers/getcustomersbyCustomerCodes';
const ChangeMobileCustomer = 'api/account/UpdateFullCustomerInfomation';
const UpdateCustomerProfile = 'api/account/UpdateFullCustomerInfomation';
const UpdateGenderValue = 'api/account/updateGenderValue';


@Injectable()
export class CustomerService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${CustomersResourceUrl}`;
    public navigateCustomerDetail = new Subject<any>();

    public GetCustomers(request: CustomersRequest): Observable<CustomersResponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetCustomersByCustomerCodes(request: GetCustomersByCustomerCodesRequest): Observable<GetCustomersByCustomerCodesResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${CustomersByCustomerCodesResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }

    navigateCustomerDetailPage(customerId: String) {
        return this.navigateCustomerDetail.next(customerId);
    }
    subcribeNavigateCustomerDetailEvent(): Observable<any> {
        return this.navigateCustomerDetail.asObservable();
    }

    public ChangeMobileCustomer(request: ChangeCustomerMobileRequest): Observable<string> {
        const urlAPI = `${environment.apiGatewayUri}/${ChangeMobileCustomer}`;
        return this.httpService.post(urlAPI, request);
    }

    public ReactiveCustomer(request: ReactiveCustomerRequest): Observable<any> {
        const apiUrl = `${environment.apiGatewayUri}/${ReactiveCustomerResourceUrl}`;
        return this.httpService.post(apiUrl, request);
      }
    public UpdateCustomerProfile(request: ChangeCustomerInfoRequest): Observable<string> {
        const urlAPI = `${environment.apiGatewayUri}/${UpdateCustomerProfile}`;
        return this.httpService.post(urlAPI, request);
    }

    public UpdateGender(request: null): Observable<number> {
        const urlAPI = `${environment.apiGatewayUri}/${UpdateGenderValue}`;
        return this.httpService.post(urlAPI, request);
    }
}
