import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import {
    FilterCustomerRequest,
    FilterCustomerResponse,
    DebitPointTransactionResponse,
    DebitPointTransactionRequest,
    CreditPointTransactionResponse,
    CreditPointTransactionRequest,
    TotalBalancePointTransactionResponse,
    TotalBalancePointTransactionRequest,
    TotalBalanceWalletTransactionResponse,
    TotalBalanceWalletTransactionRequest,
    CreditWalletTransactionRequest,
    CreditWalletTransactionResponse,
    DebitWalletTransactionRequest,
    DebitWalletTransactionResponse,
    VoidOfCreditWalletTransactionRequest,
    VoidOfDebitWalletTransactionRequest,
    VoidOfCreditWalletTransactionResponse,
    VoidOfDebitWalletTransactionResponse,
    VoidOfDebitPointTransactionResponse,
    VoidOfCreditPointTransactionResponse,
    VoidOfCreditPointTransactionRequest,
    VoidOfDebitPointTransactionRequest,
    VoidMembershipTransactionRequest,
    VoidMembershipTransactionResponse,
    GetVisitorsStatisticsRequest,
    GetVisitorsStatisticsResponse,
    GetPointsStatisticsRequest,
    GetPointsStatisticsResponse,
    GetWalletStatisticsRequest,
    GetWalletStatisticsResponse
} from 'src/app/containers/homepage/homepage.model';
import { Subject } from 'rxjs';

const NewBasicCustomersResourceUrl = 'api/customers/getnewcustomers';
const ExpiredCustomersResourceUrl = 'api/customers/getexpiredcustomers';
const DowngradedCustomersResourceUrl = 'api/customers/getdowngradedcustomers';
const DebitPointTransactionResourceUrl = 'api/pointTransactions/getsdebitsummary';
const CreditPointTransactionResourceUrl = 'api/pointTransactions/getscreditsummary';
const TotalBalancePointTransactionResourceUrl = 'api/pointTransactions/getstotalbalance';
const TotalBalanceWalletTransactionResourceUrl = 'api/walletTransactions/getstotalbalance';
const CreditWalletTransactionResourceUrl = 'api/walletTransactions/getscreditsummary';
const DebitWalletTransactionResourceUrl = 'api/walletTransactions/getsdebitsummary';
const UpgradedCustomerResourceUrl = 'api/customers/getupgradedcustomers';
const RenewedCustomerResourceUrl = 'api/customers/getrenewedcustomers';
const ExtendedCustomerResourceUrl = 'api/customers/getextendedcustomers';
const VoidOfCreditPointTransactionResourceUrl = 'api/pointTransactions/getsvoidofcredit';
const VoidOfDebitPointTransactionResourceUrl = 'api/pointTransactions/getsvoidofdebit';
const VoidOfCreditWalletTransactionResourceUrl = 'api/walletTransactions/getsvoidofcredit';
const VoidOfDebitWalletTransactionResourceUrl = 'api/walletTransactions/getsvoidofdebit';
const VoidMembershipTransactionResourceUrl = 'api/membershipTransactions/getvoidmembershiptransactions';
const GetVisitorsStatistics = 'api/Activity/getVisitorsStatistics';
const GetPointsStatistics = 'api/pointTransactions/getPointsStatistics';
const GetWalletStatistics = 'api/walletTransactions/getWalletSatistics';
@Injectable()
export class HomepageService {
    constructor(private httpService: HttpService) {
    }

    public GetNewBasicCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${NewBasicCustomersResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetUpgradedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${UpgradedCustomerResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetRenewedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${RenewedCustomerResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetExtendedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${ExtendedCustomerResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetExpiredCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${ExpiredCustomersResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetDowngradedCustomers(request: FilterCustomerRequest): Observable<FilterCustomerResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${DowngradedCustomersResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetDebitValuePointTransaction(request: DebitPointTransactionRequest): Observable<DebitPointTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${DebitPointTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetCreditValuePointTransaction(request: CreditPointTransactionRequest): Observable<CreditPointTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${CreditPointTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetTotalBalancePointTransaction(request: TotalBalancePointTransactionRequest): Observable<TotalBalancePointTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${TotalBalancePointTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetTotalBalanceWalletTransaction(request: TotalBalanceWalletTransactionRequest):
        Observable<TotalBalanceWalletTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${TotalBalanceWalletTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetCreditValueWalletTransaction(request: CreditWalletTransactionRequest): Observable<CreditWalletTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${CreditWalletTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetDebitValueWalletTransaction(request: DebitWalletTransactionRequest): Observable<DebitWalletTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${DebitWalletTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVoidOfCreditValuePointTransaction(request: VoidOfCreditPointTransactionRequest):
        Observable<VoidOfCreditPointTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${VoidOfCreditPointTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVoidOfDebitValuePointTransaction(request: VoidOfDebitPointTransactionRequest):
        Observable<VoidOfDebitPointTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${VoidOfDebitPointTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVoidOfCreditValueWalletTransaction(request: VoidOfCreditWalletTransactionRequest):
        Observable<VoidOfCreditWalletTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${VoidOfCreditWalletTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVoidOfDebitValueWalletTransaction(request: VoidOfDebitWalletTransactionRequest):
        Observable<VoidOfDebitWalletTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${VoidOfDebitWalletTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVoidMembershipTransaction(request: VoidMembershipTransactionRequest):
        Observable<VoidMembershipTransactionResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${VoidMembershipTransactionResourceUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetVisitorsStatistics(request: GetVisitorsStatisticsRequest): Observable<GetVisitorsStatisticsResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${GetVisitorsStatistics}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetPointsStatistics(request: GetPointsStatisticsRequest): Observable<GetPointsStatisticsResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${GetPointsStatistics}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
    }

    public GetWalletStatistics(request: GetWalletStatisticsRequest): Observable<GetWalletStatisticsResponse> {
        const apiUrl = `${environment.apiGatewayUri}/${GetWalletStatistics}`;
        return this.httpService.get(apiUrl, {params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }
}
