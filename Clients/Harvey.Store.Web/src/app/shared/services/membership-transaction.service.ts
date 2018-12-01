import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  AddMembershipCommand,
  CustomerMembershipTransactionsResponse,
  VoidMemberTransactionRequest,
  VoidMemberTransactionResponse
} from '../models/membershipTransaction.model';
import { HttpClient } from '@angular/common/http';
import { GetTransactionsByOutletRequest } from '../models/outlet.model';
import { HttpService } from './http.service';
import { APIsURL } from '../constants/apis-url.constant';
import HttpParamsHelper from '../helper/http-params.helper';
import { CustomerTransactionRequest } from '../models/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembershipTransactionService {

  private apiGatewayUri = environment.apiGatewayUri;

  constructor(private httpService: HttpClient, private http: HttpService) {
  }

  public addMembershipTransaction(transaction: AddMembershipCommand) {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.ADD_MEMBERSHIP_TRANSACTION}`;
    return this.http.post(apiUrl, transaction);
  }

  public getMembershipTransactions(request: GetTransactionsByOutletRequest) {
    const apiUrl = `${this.apiGatewayUri}/${APIsURL.GET_MEMBERSHIP_TRANSACTION}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getCustomerMembershipTransactions(request: CustomerTransactionRequest): Observable<CustomerMembershipTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${APIsURL.GET_CUSTOMER_MEMBERSHIP_TRANSACTION}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getVoidMembershipTransaction(request: VoidMemberTransactionRequest): Observable<VoidMemberTransactionResponse> {
    const apiUrl = `${environment.apiGatewayUri}/${APIsURL.GET_VOID_MEMBERSHIP_SUMMARY}`;
    return this.http.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }
}
