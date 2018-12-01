import * as moment from 'moment';
import { MembershipActionType } from 'src/app/containers/customer/customer.model';

export class VoidPointRequest {
    public userId = '';
    public ipAddress = '';
    public pointTransactionId = '';
    public voidByName = '';
}

export class VoidWalletRequest {
    public userId = '';
    public ipAddress = '';
    public walletTransactionId = '';
    public voidByName = '';
}

export class VoidMembershipRequest {
    public userId = '';
    public ipAddress = '';
    public membershipTransactionId = '';
    public voidByName = '';
    public membershipActionType = 0;
}

export class CustomerInfoRequest {
    constructor(customerInfoRequest?: any) {
        if (!customerInfoRequest) { return; }
        this.customerId = customerInfoRequest.customerId;
    }
    public customerId = '';
}

export class CustomerTransactionRequest {
    constructor(customerTransactionRequest?: any) {
        if (!customerTransactionRequest) { return; }
        this.customerId = customerTransactionRequest.customerId;
        this.pageNumber = customerTransactionRequest.pageNumber;
        this.pageSize = customerTransactionRequest.pageSize;
    }
    public customerId = '';
    public pageNumber = 0;
    public pageSize = 0;
}

export class CustomerMembershipTransactionsResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.listMembershipTransaction = response.listMembershipTransaction;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public listMembershipTransaction!: Array<MembershipTransaction>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;

}

export class CustomerPointTransactionsResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.listPointTransaction = response.listPointTransaction;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public listPointTransaction!: Array<PointTransaction>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;

}

export class MembershipTransaction {
    constructor(membershipTransaction?: any) {
        if (!membershipTransaction) { return; }
        this.id = membershipTransaction.id;
        this.staffName = membershipTransaction.staffName;
        this.customerName = membershipTransaction.customerName;
        this.outletName = membershipTransaction.outletName;
        this.membershipType = membershipTransaction.membershipType;
        this.comment = membershipTransaction.comment;
        this.createdDate = membershipTransaction.createdDate;
        this.phoneCustomer = membershipTransaction.phoneCustomer;
        this.phoneCountryCode = membershipTransaction.phoneCountryCode;
        this.ipAddress = membershipTransaction.ipAddress;
        this.allowVoid = membershipTransaction.allowVoid;
        this.voided = membershipTransaction.voided;
        this.doneBy = membershipTransaction.doneBy;
        this.voidedBy = membershipTransaction.voidedBy;
        this.voidedDate = membershipTransaction.voidedDate;
        this.customerId = membershipTransaction.customerId;
        this.customerCode = membershipTransaction.customerCode;
        this.membershipActionType = membershipTransaction.membershipActionType;
    }
    public id = '';
    public staffName = '';
    public customerName = '';
    public outletName = '';
    public membershipType = '';
    public expiredDate = '';
    public comment = '';
    public createdDate = '';
    public phoneCustomer = '';
    public phoneCountryCode = '';
    public ipAddress = '';
    public allowVoid = false;
    public voided = false;
    public doneBy = '';
    public voidedBy = '';
    public voidedDate = '';
    public customerId = '';
    public customerCode = '';
    public membershipActionType = 0;

    static buildMembershipTransaction(item: MembershipTransaction): MembershipTransaction {
        const membershipTransaction = new MembershipTransaction();
        membershipTransaction.id = item.id;
        membershipTransaction.staffName = item.staffName ? item.staffName : '-';
        membershipTransaction.customerName = item.customerName ? item.customerName : '-';
        membershipTransaction.outletName = item.outletName ? item.outletName : '-';
        membershipTransaction.membershipType = item.membershipType ? item.membershipType : '-';
        membershipTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
            ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        membershipTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
            ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        membershipTransaction.comment = item.comment ? item.comment : '-';
        membershipTransaction.phoneCustomer = item.phoneCountryCode && item.phoneCustomer
            ? `+${item.phoneCountryCode} ${item.phoneCustomer}` : '-';
        membershipTransaction.ipAddress = item.ipAddress ? item.ipAddress : '-';
        membershipTransaction.allowVoid = item.allowVoid;
        membershipTransaction.voided = item.voided;
        membershipTransaction.doneBy = item.doneBy;
        membershipTransaction.voidedBy = item.voidedBy;
        membershipTransaction.voidedDate = item.voidedDate && new Date(item.voidedDate).getFullYear() !== 1
                                            ? moment.utc(item.voidedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        membershipTransaction.customerId = item.customerId;
        membershipTransaction.customerCode = item.customerCode;
        membershipTransaction.membershipActionType = item.membershipActionType;
        return membershipTransaction;
    }
}

export class PointTransaction {
    public id = '';
    public staffName = '';
    public customerName = '';
    public outletName = '';
    public expiredDate = '';
    public comment = '';
    public createdDate = '';
    public phoneCustomer = '';
    public debit = 0;
    public credit = 0;
    public voided = false;
    public voidedBy = '';
    public balanceTotal = 0;
    public phoneCountryCode = '';
    public ipAddress = '';
    public pointTransactionReferenceId = '';
    public customerId = '';
    public customerCode = '';
    public allowVoid = true;

    constructor(pointTransaction?: any) {
        if (!pointTransaction) { return; }
        this.id = pointTransaction.id;
        this.staffName = pointTransaction.staffName;
        this.customerName = pointTransaction.customerName;
        this.outletName = pointTransaction.outletName;
        this.comment = pointTransaction.comment;
        this.createdDate = pointTransaction.createdDate;
        this.phoneCustomer = pointTransaction.phoneCustomer;
        this.debit = pointTransaction.debit;
        this.credit = pointTransaction.credit;
        this.balanceTotal = pointTransaction.balanceTotal;
        this.voided = pointTransaction.voided;
        this.voidedBy = pointTransaction.voidedBy;
        this.phoneCountryCode = pointTransaction.phoneCountryCode;
        this.ipAddress = pointTransaction.ipAddress;
        this.customerId = pointTransaction.customerId;
        this.customerCode = pointTransaction.customerCode;
        this.allowVoid = pointTransaction.allowVoid;
    }

    static buildPointTransaction(item: PointTransaction): PointTransaction {
        const pointTransaction = new PointTransaction();
        pointTransaction.id = item.id;
        pointTransaction.staffName = item.staffName ? item.staffName : '-';
        pointTransaction.outletName = item.outletName ? item.outletName : '-';
        pointTransaction.comment = item.comment ? item.comment : '-';
        pointTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
            ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        pointTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
            ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        pointTransaction.debit = item.debit ? item.debit : 0;
        pointTransaction.credit = item.credit ? item.credit : 0;
        pointTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
        pointTransaction.voided = item.voided;
        pointTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
        pointTransaction.phoneCustomer = item.phoneCountryCode && item.phoneCustomer
            ? `+${item.phoneCountryCode} ${item.phoneCustomer}` : '-';
        pointTransaction.customerName = item.customerName ? item.customerName : '-';
        pointTransaction.ipAddress = item.ipAddress ? item.ipAddress : '-';
        pointTransaction.pointTransactionReferenceId = item.pointTransactionReferenceId;
        pointTransaction.customerId = item.customerId;
        pointTransaction.customerCode = item.customerCode;
        pointTransaction.allowVoid = item.allowVoid;
        return pointTransaction;
    }
}

export class WalletTransaction {
    constructor(walletTransaction?: any) {
        if (!walletTransaction) { return; }
        this.id = walletTransaction.id;
        this.staffName = walletTransaction.staffName;
        this.customerName = walletTransaction.customerName;
        this.outletName = walletTransaction.outletName;
        this.comment = walletTransaction.comment;
        this.createdDate = walletTransaction.createdDate;
        this.phoneCustomer = walletTransaction.phoneCustomer;
        this.debit = walletTransaction.debit;
        this.credit = walletTransaction.credit;
        this.balanceTotal = walletTransaction.balanceTotal;
        this.voided = walletTransaction.voided;
        this.voidedBy = walletTransaction.voidedBy;
        this.phoneCountryCode = walletTransaction.phoneCountryCode;
        this.ipAddress = walletTransaction.ipAddress;
        this.walletTransactionReferenceId = walletTransaction.walletTransactionReferenceId;
        this.customerId = walletTransaction.customerId;
        this.customerCode = walletTransaction.customerCode;
    }
    public id = '';
    public staffName = '';
    public customerName = '';
    public outletName = '';
    public expiredDate = '';
    public comment = '';
    public createdDate = '';
    public phoneCustomer = '';
    public debit = 0;
    public credit = 0;
    public voided = false;
    public voidedBy = '';
    public balanceTotal = 0;
    public phoneCountryCode = '';
    public ipAddress = '';
    public walletTransactionReferenceId = '';
    public customerId = '';
    public customerCode = '';

    static buildWalletTransaction(item: WalletTransaction): WalletTransaction {
        const walletTransaction = new WalletTransaction();
        walletTransaction.id = item.id;
        walletTransaction.staffName = item.staffName ? item.staffName : '-';
        walletTransaction.outletName = item.outletName ? item.outletName : '-';
        walletTransaction.comment = item.comment ? item.comment : '-';
        walletTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
            ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        walletTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
            ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        walletTransaction.debit = item.debit ? item.debit : 0;
        walletTransaction.credit = item.credit ? item.credit : 0;
        walletTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
        walletTransaction.voided = item.voided;
        walletTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
        walletTransaction.phoneCustomer = item.phoneCountryCode && item.phoneCustomer
            ? `+${item.phoneCountryCode} ${item.phoneCustomer}` : '-';
        walletTransaction.customerName = item.customerName ? item.customerName : '-';
        walletTransaction.ipAddress = item.ipAddress ? item.ipAddress : '-';
        walletTransaction.walletTransactionReferenceId = item.walletTransactionReferenceId;
        walletTransaction.customerId = item.customerId;
        walletTransaction.customerCode = item.customerCode;
        return walletTransaction;
    }
}

export class CustomerWalletTransactionsResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.listWalletTransaction = response.listWalletTransaction;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public listWalletTransaction!: Array<WalletTransaction>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;

}

export enum TypeConfirm {
    VoidPoint = 1,
    VoidWallet = 2,
    VoidMembership = 3,
    ChangeStatusCustomer = 4
}

export enum MembershipType {
    basic = 'Basic',
    premium = 'Premium+'
}

export enum Status {
    Activate = 0,
    Deactivate = 1
}

export class ChangeStatusCustomerRequest {
    public userId = '';
    public phoneCountryCode = '';
    public phoneNumber = '';
    public customerId = '';
    public  isActive = +Status.Activate;
    public createdByName = '';
}

export class AddBlankMembershipTransactionRequest {
    public userId = '';
    public customerId = '';
    public membershipTypeId = '';
    public expiredDate = null;
    public comment = null;
    public ipAddress = '';
    public outletId = '';
    public staffId = '';
  }

export class SubtractWalletRequest {
    public userId = '';
    public customerId = '';
    public value = 0;
    public ipAddress = '';
    public createdByName = '';
}
