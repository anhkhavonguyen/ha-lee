import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GetTransactionsByOutletRequest } from '../models/outlet.model';
import {
  CreditWalletTransactionRequest,
  CreditWalletTransactionResponse,
  DebitWalletTransactionRequest,
  DebitWalletTransactionResponse,
  GetWalletTransactionsResponse,
  SpendWalletRequest,
  SpendWalletResponse,
  TopUpResponse,
  TopUpWalletRequest,
  TotalBalanceWalletTransactionRequest,
  TotalBalanceWalletTransactionResponse,
  VoidOfCreditWalletTransactionRequest,
  VoidOfCreditWalletTransactionResponse,
  VoidOfDebitWalletTransactionRequest,
  VoidOfDebitWalletTransactionResponse,
  CustomerWalletTransactionsResponse
} from '../models/walletTransaction.model';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';
import { APIsURL } from '../constants/apis-url.constant';
import HttpParamsHelper from '../helper/http-params.helper';
import { CustomerTransactionRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class WalletTransactionService {

  private apiGatewayUri = environment.apiGatewayUri;

  constructor(private httpService: HttpClient, private http: HttpService) {
  }

  public getWalletTransactions(userToken: string, request: GetTransactionsByOutletRequest):
    Observable<any | GetWalletTransactionsResponse> {
    const apiUrl = this.apiGatewayUri + '/api/walletTransactions/getsbyoutlet' + '?OutletId=' + request.outletId +
      '&PageNumber=' + request.pageNumber + '&PageSize=' + request.pageSize;
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpService.get(apiUrl, { headers });
  }

  public topUpWallet(userToken: string, request: TopUpWalletRequest): Observable<any | TopUpResponse> {
    const apiURL = this.apiGatewayUri + '/api/walletTransactions/topupwallet';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpService.post(apiURL, request, { headers });
  }

  public spendWallet(userToken: string, request: SpendWalletRequest): Observable<any | SpendWalletResponse> {
    const apiURL = this.apiGatewayUri + '/api/walletTransactions/spendingwallet';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.httpService.post(apiURL, request, { headers });
  }

  public getTotalBalanceWalletTransaction(request: TotalBalanceWalletTransactionRequest):
    Observable<TotalBalanceWalletTransactionResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_TOTAL_BALANCE_WALLET_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getCreditValueWalletTransaction(request: CreditWalletTransactionRequest): Observable<CreditWalletTransactionResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_WALLET_CREDIT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getDebitValueWalletTransaction(request: DebitWalletTransactionRequest): Observable<DebitWalletTransactionResponse> {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_WALLET_DEBIT_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getVoidOfCreditValueWalletTransaction(request: VoidOfCreditWalletTransactionRequest):
    Observable<VoidOfCreditWalletTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_VOID_OF_CREDIT_WALLET_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getVoidOfDebitValueWalletTransaction(request: VoidOfDebitWalletTransactionRequest):
    Observable<VoidOfDebitWalletTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_VOID_OF_DEBIT_WALLET_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getCustomerWalletTransactions(request: CustomerTransactionRequest): Observable<CustomerWalletTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${APIsURL.GET_CUSTOMER_WALLET_TRANSACTION}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }
}
