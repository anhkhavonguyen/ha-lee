import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Customer } from 'src/app/containers/customer/customer.model';
import { Observable } from 'rxjs/internal/Observable';
import {
    CustomerInfoRequest,
    CustomerTransactionRequest,
    CustomerMembershipTransactionsResponse,
    CustomerPointTransactionsResponse,
    CustomerWalletTransactionsResponse,
    VoidPointRequest,
    VoidWalletRequest,
    VoidMembershipRequest,
    ChangeStatusCustomerRequest,
    AddBlankMembershipTransactionRequest,
    SubtractWalletRequest
} from 'src/app/containers/customer/customer-detail/customer-detail.model';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';

const CustomerInfoResourceUrl = 'api/customers/getcustomerbyid';
const MembershipTransactionsResourceUrl = 'api/membershipTransactions/gets';
const PointTransactionsResourceUrl = 'api/pointTransactions/getsbycustomer';
const WalletTransactionsResourceUrl = 'api/walletTransactions/getsbycustomer';
const VoidPointTransactionsResourceUrl = 'api/pointTransactions/voidpoint';
const VoidWalletTransactionsResourceUrl = 'api/walletTransactions/voidwallet';
const VoidMembershipTransactionsResourceUrl = 'api/membershipTransactions/void';
const ChangeStatusMemberResourceUrl = 'api/customers/activeCustomer';
const AddBlankMembershipTransactionResourceUrl = 'api/MembershipTransactions/add';

@Injectable()
export class CustomerDetailService {
    constructor(private httpService: HttpService) {
    }

    public getCustomerInfo(request: CustomerInfoRequest): Observable<Customer> {
        const urlAPI = `${environment.apiGatewayUri}/${CustomerInfoResourceUrl}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public getMembershipTransactions(request: CustomerTransactionRequest): Observable<CustomerMembershipTransactionsResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${MembershipTransactionsResourceUrl}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public getPointTransactions(request: CustomerTransactionRequest): Observable<CustomerPointTransactionsResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${PointTransactionsResourceUrl}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }
    public getWalletTransactions(request: CustomerTransactionRequest): Observable<CustomerWalletTransactionsResponse> {
        const urlAPI = `${environment.apiGatewayUri}/${WalletTransactionsResourceUrl}`;
        return this.httpService.get(urlAPI, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public voidPointTransaction(request: VoidPointRequest): Observable<any> {
        const urlAPI = `${environment.apiGatewayUri}/${VoidPointTransactionsResourceUrl}`;
        return this.httpService.post(urlAPI, request);
    }

    public voidWalletTransaction(request: VoidWalletRequest): Observable<any> {
        const urlAPI = `${environment.apiGatewayUri}/${VoidWalletTransactionsResourceUrl}`;
        return this.httpService.post(urlAPI, request);
    }

    public voidMembershipTransaction(request: VoidMembershipRequest): Observable<any> {
        const urlAPI = `${environment.apiGatewayUri}/${VoidMembershipTransactionsResourceUrl}`;
        return this.httpService.post(urlAPI, request);
    }

    public changeStatusMember(request: ChangeStatusCustomerRequest): Observable<any> {
        const urlAPI = `${environment.apiGatewayUri}/${ChangeStatusMemberResourceUrl}`;
        return this.httpService.post(urlAPI, request);
    }

    public AddBlankMembershipTransaction(request: AddBlankMembershipTransactionRequest): Observable<any> {
        const urlAPI = `${environment.apiGatewayUri}/${AddBlankMembershipTransactionResourceUrl}`;
        return this.httpService.post(urlAPI, request);
    }
}
