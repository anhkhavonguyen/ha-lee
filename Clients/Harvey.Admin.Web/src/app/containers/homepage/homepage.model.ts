import { Customer, CustomersRequest } from 'src/app/containers/customer/customer.model';
import {
    PointTransaction,
    WalletTransaction,
    MembershipTransaction
} from 'src/app/containers/customer/customer-detail/customer-detail.model';

export class FilterCustomerRequest {
    constructor(Customersrequest?: any) {
        if (!Customersrequest) { return; }
        this.pageNumber = Customersrequest.pageNumber;
        this.pageSize = Customersrequest.pageSize;
        this.fromDateFilter = Customersrequest.fromDateFilter;
        this.toDateFilter = Customersrequest.toDateFilter;
        this.outletId = Customersrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
    public testDate?: string;
}

export class FilterCustomerResponse {
    constructor(CustomersResponse?: any) {
        if (!CustomersResponse) { return; }
        this.customerListResponse = CustomersResponse.customerListResponse;
        this.pageNumber = CustomersResponse.pageNumber;
        this.pageSize = CustomersResponse.pageSize;
        this.totalItem = CustomersResponse.totalItem;
    }
    public customerListResponse!: Array<Customer>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class DebitPointTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class DebitPointTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listPointTransaction = TransResponse.listPointTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalDebitValue = TransResponse.totalDebitValue;
    }
    public listPointTransaction!: Array<PointTransaction>;
    public totalDebitValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class CreditPointTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class CreditPointTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listPointTransaction = TransResponse.listPointTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalCreditValue = TransResponse.totalCreditValue;
    }
    public listPointTransaction!: Array<PointTransaction>;
    public totalCreditValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class TotalBalancePointTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class TotalBalancePointTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.totalBalance = TransResponse.totalBalance;
    }
    public totalBalance = 0;

}

export class TotalBalanceWalletTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class TotalBalanceWalletTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.totalBalance = TransResponse.totalBalance;
    }
    public totalBalance = 0;

}

export class CreditWalletTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class CreditWalletTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listWalletTransaction = TransResponse.listWalletTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalCreditValue = TransResponse.totalCreditValue;
    }
    public listWalletTransaction!: Array<WalletTransaction>;
    public totalCreditValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class DebitWalletTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class DebitWalletTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listWalletTransaction = TransResponse.listWalletTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalDebitValue = TransResponse.totalDebitValue;
    }
    public listWalletTransaction!: Array<WalletTransaction>;
    public totalDebitValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class VoidOfCreditPointTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class VoidOfCreditPointTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listPointTransaction = TransResponse.listPointTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalVoidOfCreditValue = TransResponse.totalVoidOfCreditValue;
    }
    public listPointTransaction!: Array<PointTransaction>;
    public totalVoidOfCreditValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class VoidOfDebitPointTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class VoidOfDebitPointTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listPointTransaction = TransResponse.listPointTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalVoidOfDebitValue = TransResponse.totalVoidOfDebitValue;
    }
    public listPointTransaction!: Array<PointTransaction>;
    public totalVoidOfDebitValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class VoidOfCreditWalletTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class VoidOfCreditWalletTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listWalletTransaction = TransResponse.listWalletTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalVoidOfCreditValue = TransResponse.totalVoidOfCreditValue;
    }
    public listWalletTransaction!: Array<WalletTransaction>;
    public totalVoidOfCreditValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class VoidOfDebitWalletTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class VoidOfDebitWalletTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listWalletTransaction = TransResponse.listWalletTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
        this.totalVoidOfDebitValue = TransResponse.totalVoidOfDebitValue;
    }
    public listWalletTransaction!: Array<WalletTransaction>;
    public totalVoidOfDebitValue = 0;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class VoidMembershipTransactionRequest {
    constructor(Transrequest?: any) {
        if (!Transrequest) { return; }
        this.pageNumber = Transrequest.pageNumber;
        this.pageSize = Transrequest.pageSize;
        this.fromDateFilter = Transrequest.fromDateFilter;
        this.toDateFilter = Transrequest.toDateFilter;
        this.outletId = Transrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
    public outletId?: string;
}

export class VoidMembershipTransactionResponse {
    constructor(TransResponse?: any) {
        if (!TransResponse) { return; }
        this.listMembershipTransaction = TransResponse.listMembershipTransaction;
        this.pageNumber = TransResponse.pageNumber;
        this.pageSize = TransResponse.pageSize;
        this.totalItem = TransResponse.totalItem;
    }
    public listMembershipTransaction!: Array<MembershipTransaction>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class GetCustomersByCustomerCodesRequest {
    constructor(Customersrequest?: any) {
        if (!Customersrequest) { return; }
        this.customerCodes = Customersrequest.customerCodes;
    }

    public customerCodes: string[] = [];
}

export class GetCustomersByCustomerCodesResponse {
    constructor(CustomersResponse?: any) {
        if (!CustomersResponse) { return; }
        this.customerListResponse = CustomersResponse.customerListResponse;
    }

    public customerListResponse: Array<Customer> = [];
}

export class GetVisitorsStatisticsRequest {
    public fromDate: Date | undefined;
    public toDate: Date | undefined;
    public outletId?: string;
}

export class GetVisitorsStatisticsResponse {
    public dataVisitorsStatistic: Array<DataVisitorsPerDay> = [];
}

export class DataVisitorsPerDay {
    public time: Date | undefined;
    public value: number | undefined;
    public uniqueValue: number | undefined;
}

export class GetPointsStatisticsRequest {
    public fromDate: Date | undefined;
    public toDate: Date | undefined;
    public outletId?: string;
}

export class GetPointsStatisticsResponse {
    public dataPointsStatistics: Array<DataPointsPerDay> = [];
}

export class DataPointsPerDay {
    public time: Date | undefined;
    public totalAdd: number | undefined;
    public totalRedeem: number | undefined;
}

export class GetWalletStatisticsRequest {
    public fromDate: Date | undefined;
    public toDate: Date | undefined;
    public outletId?: string;
}

export class GetWalletStatisticsResponse {
    public dataWalletStatistics: Array<DataWalletPerDay> = [];
}

export class DataWalletPerDay {
    public time: Date | undefined;
    public totalTopup: number | undefined;
    public totalSpend: number | undefined;
}
