import { Injectable } from '@angular/core';
import {
  CheckPINRequest,
  CheckPINResponse,
  Customer,
  CustomersRequest,
  CustomersResponse,
  FilterCustomerRequest,
  FilterCustomerResponse,
  NewCustomerRequest,
  ResendPINRequest,
  ResendResetPasswordRequest,
  ResendSignUpRequest,
  GetBasicCustomerInfoRequest,
  GetBasicCustomerInfoResponse,
  UpdateCustomerProfileRequest,
  CheckValidCustomerServing,
  ChangePhoneNumberRequest
} from '../models/customer.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpService } from './http.service';
import { APIsURL } from '../constants/apis-url.constant';
import HttpParamsHelper from '../helper/http-params.helper';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private apiGatewayUri = environment.apiGatewayUri;

  constructor(private httpService: HttpClient,
    private http: HttpService) {
  }

  public initNewCustomer(userToken: string, request: NewCustomerRequest): Observable<any> {
    const apiUrl = `${this.apiGatewayUri}/api/Customers/initcustomerprofile`;
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpService.post(apiUrl, request, { headers });
  }

  public getBalanceRewardPoint(userToken: string, request: string): Observable<any> {
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    const apiUrl = `${this.apiGatewayUri}/api/PointTransactions/getpointbalance?customerId=${request}`;
    return this.httpService.get(apiUrl, { headers });
  }

  public getBalanceWallet(userToken: string, request: string): Observable<any> {
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    const apiUrl = `${this.apiGatewayUri}/api/walletTransactions/getwalletbalance?request=${request}`;
    return this.httpService.get(apiUrl, { headers });
  }

  public resendPIN(request: ResendPINRequest): Observable<any | CustomersResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.RESEND_PIN}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public checkPIN(request: CheckPINRequest): Observable<any | CheckPINResponse> {
    const headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    const apiUrl = this.apiGatewayUri + `/api/check-pin?userId=${request.userId}&PIN=${request.PIN}`;
    return this.httpService.get(apiUrl, { headers });
  }

  public getByCustomerId(userToken: string, request: string): Observable<any | Customer> {
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    const apiUrl = this.apiGatewayUri + `/api/customers/getcustomerbyid?customerId=${request}`;
    return this.httpService.get(apiUrl, { headers });
  }

  public getCustomers(request: CustomersRequest): Observable<CustomersResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getnewCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_NEW_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getExpiredCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_EXPIRED_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getDowngradedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_DOWNGRADED_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public resendSignUpLink(request: ResendSignUpRequest): Observable<any> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.RESEND_SIGN_UP_LINK}`;
    return this.http.post(apiUrl, request);
  }

  public getUpgradedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_UPGRADED_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getRenewedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_RENEWED_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getExtendedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_EXTENDED_CUSTOMERS}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public resendResetPasswordLink(request: ResendResetPasswordRequest): Observable<any> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.RESEND_RESET_PASSWORD}`;
    return this.http.post(apiUrl, request);
  }

  public getBasicCustomerInfo(request: GetBasicCustomerInfoRequest): Observable<GetBasicCustomerInfoResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_BASIC_CUSTOMER_INFO}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public updateCustomerProfile(request: UpdateCustomerProfileRequest) {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.UPDATE_CUSTOMER_PROFILE}`;
    return this.http.post(apiUrl, request);
  }

  public checkValidCustomerServing(request: CheckValidCustomerServing): Observable<Customer> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_CUSTOMER_INFO}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public changePhoneNumber(request: ChangePhoneNumberRequest) {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.CHANGE_PHONE_NUMBER}`;
    return this.http.post(apiUrl, request);
  }
}
