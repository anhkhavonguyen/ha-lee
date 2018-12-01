import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Gender } from '../../shared/models/user-profile.model';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpService } from '../../shared/services/http.service';
import HttpParamsHelper from '../../shared/helper/http-params.helper';
import * as moment from 'moment';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';

const MembershipTransactionsResourceUrl = 'api/membershipTransactions/gets';
const PointTransactionsResourceUrl = 'api/pointTransactions/getsbymember';
const WalletTransactionsResourceUrl = 'api/walletTransactions/getsbymember';
const GetExpiryPointUrl = 'api/pointTransactions/getexpirypoints';

@Injectable({
  providedIn: 'root'
})
export class MyAccountService {

  constructor(private httpClient: HttpClient,
    private http: HttpService,
    private appSettingService: AppSettingService) {
  }

  getUserProfile() {
    const apiURL = `${environment.apiGatewayUri}/api/account/get-user-profile`;
    return this.httpClient.get(apiURL);
  }

  getBalancePointAndWallet() {
    const apiURL = `${environment.apiGatewayUri}/api/customers/getpointscurrentuser`;
    return this.httpClient.get(apiURL);
  }

  getCurrentMembership(): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/customers/getmembershipmcurrentuser`;
    return this.httpClient.get(apiURL);
  }

  getAllOutlets() {
    const apiURL = `${environment.apiGatewayUri}/api/outlets/getsWithoutPaging`;
    return this.httpClient.get(apiURL);
  }

  updateProfile(request: UpdateUserProfileRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/update-profile`;
    return this.httpClient.post(apiURL, request);
  }

  changePassword(request: ChangePasswordRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/change-password`;
    return this.httpClient.put(apiURL, request);
  }

  changePin(request: ChangePinRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/change-pin`;
    return this.httpClient.post(apiURL, request);
  }

  sendPINToPhone(countryCode: string, phoneNumber: string) {
    const acronymBrandName = this.appSettingService.getTitleFromAppSettingsData(
      this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue);

    let apiURL = `${environment.apiGatewayUri}/api/send-pin?countryCode=${countryCode}&numberPhone=${phoneNumber}`;
    apiURL += `&acronymBrandName=${acronymBrandName}`;
    return this.httpClient.get(apiURL);
  }

  changePhoneNumber(request: ChangePhoneNumberRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/change-phone-current-user`;
    return this.httpClient.post(apiURL, request);
  }

  public getMembershipTransactions(request: CustomerTransactionRequest): Observable<CustomerMembershipTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${MembershipTransactionsResourceUrl}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getPointTransactions(request: CustomerTransactionRequest): Observable<CustomerPointTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${PointTransactionsResourceUrl}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getWalletTransactions(request: CustomerTransactionRequest): Observable<CustomerWalletTransactionsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${WalletTransactionsResourceUrl}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getExpiryPoints(request: GetExpiryPointsRequest): Observable<GetExpiryPointsResponse> {
    const urlAPI = `${environment.apiGatewayUri}/${GetExpiryPointUrl}`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  public getUserCodeById() {
    const urlAPI = `${environment.apiGatewayUri}/api/Customers/getcustomercodebyid`;
    return this.http.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams() });
  }
}

export class ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export class ChangePinRequest {
  oldPin: string;
  newPin: string;
  confirmPin: string;
}

export class UpdateUserProfileRequest {
  firstName: string;
  lastName: string;
  email: string;
  gender: Gender;
  zipCode?: string;
  avatar: string;
  dateOfBirth: Date;
}

export class GetBalancePointAndWalletResponse {
  pointTransactionBalance: string;
  walletTransactionBalance: string;
}

export class MembershipResponse {
  membership: string;
  expiredDate: Date;
}

export class ChangePhoneNumberRequest {
  phoneNumber: string;
  pin: string;
  phoneCountryCode: string;
}

export class CustomerTransactionRequest {
  public customerId = '';
  public pageNumber = 0;
  public pageSize = 0;

  constructor(customerTransactionRequest?: any) {
    if (!customerTransactionRequest) { return; }
    this.customerId = customerTransactionRequest.customerId;
    this.pageNumber = customerTransactionRequest.pageNumber;
    this.pageSize = customerTransactionRequest.pageSize;
  }
}

export class CustomerMembershipTransactionsResponse {
  public listMembershipTransaction!: Array<MembershipTransaction>;
  public pageNumber = 0;
  public pageSize = 0;
  public totalItem = 0;

  constructor(response?: any) {
    if (!response) { return; }
    this.listMembershipTransaction = response.listMembershipTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }
}

export class CustomerPointTransactionsResponse {
  public listPointTransaction!: Array<PointTransaction>;
  public pageNumber = 0;
  public pageSize = 0;
  public totalItem = 0;

  constructor(response?: any) {
    if (!response) { return; }
    this.listPointTransaction = response.listPointTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }
}

export class CustomerWalletTransactionsResponse {
  public listWalletTransaction!: Array<WalletTransaction>;
  public pageNumber = 0;
  public pageSize = 0;
  public totalItem = 0;

  constructor(response?: any) {
    if (!response) { return; }
    this.listWalletTransaction = response.listWalletTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }
}

export class MembershipTransaction {
  public id = '';
  public outletName = '';
  public membershipType = '';
  public expiredDate = '';
  public createdDate = '';

  constructor(membershipTransaction?: any) {
    if (!membershipTransaction) { return; }
    this.id = membershipTransaction.id;
    this.outletName = membershipTransaction.outletName;
    this.membershipType = membershipTransaction.membershipType;
    this.createdDate = membershipTransaction.createdDate;
  }

  static buildMembershipTransaction(item: MembershipTransaction): MembershipTransaction {
    const membershipTransaction = new MembershipTransaction();
    membershipTransaction.id = item.id;
    membershipTransaction.outletName = item.outletName ? item.outletName : '-';
    membershipTransaction.membershipType = item.membershipType ? item.membershipType : '-';
    membershipTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    membershipTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    return membershipTransaction;
  }
}

export class PointTransaction {
  public id = '';
  public outletName = '';
  public expiredDate = '';
  public createdDate = '';
  public debit = 0;
  public credit = 0;
  public balanceTotal = 0;

  constructor(pointTransaction?: any) {
    if (!pointTransaction) { return; }
    this.id = pointTransaction.id;
    this.outletName = pointTransaction.outletName;
    this.createdDate = pointTransaction.createdDate;
    this.debit = pointTransaction.debit;
    this.credit = pointTransaction.credit;
    this.balanceTotal = pointTransaction.balanceTotal;
  }

  static buildPointTransaction(item: PointTransaction): PointTransaction {
    const pointTransaction = new PointTransaction();
    pointTransaction.id = item.id;
    pointTransaction.outletName = item.outletName ? item.outletName : '-';
    pointTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    pointTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    pointTransaction.debit = item.debit ? item.debit : 0;
    pointTransaction.credit = item.credit ? item.credit : 0;
    pointTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    return pointTransaction;
  }
}

export class WalletTransaction {
  public id = '';
  public outletName = '';
  public expiredDate = '';
  public createdDate = '';
  public debit = 0;
  public credit = 0;
  public balanceTotal = 0;

  constructor(walletTransaction?: any) {
    if (!walletTransaction) { return; }
    this.id = walletTransaction.id;
    this.outletName = walletTransaction.outletName;
    this.createdDate = walletTransaction.createdDate;
    this.debit = walletTransaction.debit;
    this.credit = walletTransaction.credit;
    this.balanceTotal = walletTransaction.balanceTotal;
  }

  static buildWalletTransaction(item: WalletTransaction): WalletTransaction {
    const walletTransaction = new WalletTransaction();
    walletTransaction.id = item.id;
    walletTransaction.outletName = item.outletName ? item.outletName : '-';
    walletTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    walletTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    walletTransaction.debit = item.debit ? item.debit : 0;
    walletTransaction.credit = item.credit ? item.credit : 0;
    walletTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    return walletTransaction;
  }
}

export class GetExpiryPointsRequest {
  public customerId: string;
  public fromDate: string;
  public toDate: string;
}

export class GetExpiryPointsResponse {
  public totalAvailablePoint: number;
  public totalExpirypoint: number;
  public expiryPoints: Array<ExpiryPoint>;
}

export class ExpiryPoint {
  public expiry: Date;
  public pointValue: number;
}
