import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AddPointRequest,
  AddPointResponse,
  CreditPointTransactionRequest,
  CreditPointTransactionResponse,
  DebitPointTransactionRequest,
  DebitPointTransactionResponse,
  GetPointTransactionsResponse,
  RedeemPointRequest,
  TotalBalancePointTransactionRequest,
  TotalBalancePointTransactionResponse,
  VoidOfCreditPointTransactionRequest,
  VoidOfCreditPointTransactionResponse,
  VoidOfDebitPointTransactionRequest,
  VoidOfDebitPointTransactionResponse,
  CustomerPointTransactionsResponse,
  GetExpiryPointsRequest,
  GetExpiryPointsResponse
} from '../models/pointTransaction.model';
import { GetTransactionsByOutletRequest } from '../models/outlet.model';
import { APIsURL } from '../constants/apis-url.constant';
import { HttpService } from './http.service';
import HttpParamsHelper from '../helper/http-params.helper';
import { CustomerTransactionRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class PointTransactionService {

  private apiGatewayUri = environment.apiGatewayUri;

  constructor(private httpClient: HttpClient, private http: HttpService) {
  }

  public addPointTransaction(userToken: string, request: AddPointRequest): Observable<any | AddPointResponse> {
    const apiURL = this.apiGatewayUri + '/api/PointTransactions/addpoint';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpClient.post(apiURL, request, { headers });
  }

  public redeemPointTransaction(userToken: string, request: RedeemPointRequest): Observable<any> {
    const apiURL = this.apiGatewayUri + '/api/PointTransactions/redeempoint';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpClient.post(apiURL, request, { headers });
  }

  public getPointTransactions(userToken: string, request: GetTransactionsByOutletRequest): Observable<any | GetPointTransactionsResponse> {
    const apiUrl = this.apiGatewayUri + '/api/pointTransactions/getsbyoutlet' + '?OutletId=' +
      request.outletId + '&PageNumber=' + request.pageNumber + '&PageSize=' + request.pageSize;
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpClient.get(apiUrl, { headers });
  }

  public getTotalBalancePointTransaction(request: TotalBalancePointTransactionRequest): Observable<TotalBalancePointTransactionResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_TOTAL_BALANCE_POINT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getCreditValuePointTransaction(request: CreditPointTransactionRequest): Observable<CreditPointTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_POINT_CREDIT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getDebitValuePointTransaction(request: DebitPointTransactionRequest): Observable<DebitPointTransactionResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_POINT_DEBIT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getVoidOfCreditValuePointTransaction(request: VoidOfCreditPointTransactionRequest):
    Observable<VoidOfCreditPointTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_VOID_OF_CREDIT_POINT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getVoidOfDebitValuePointTransaction(request: VoidOfDebitPointTransactionRequest): Observable<VoidOfDebitPointTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_VOID_OF_DEBIT_POINT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getCustomerPointTransactions(request: CustomerTransactionRequest): Observable<CustomerPointTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${APIsURL.GET_CUSTOMER_POINT_TRANSACTION}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getExpiryPoints(request: GetExpiryPointsRequest): Observable<GetExpiryPointsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${APIsURL.GET_EXPIRY_POINT}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }
}
