import * as moment from 'moment';

export class MembershipTransaction {
  public id: string;
  public membershipType: string;
  public expiredDate: string;
  public comment: string;
  public phoneCustomer: string;
  public customerName: string;
  public outletName: string;
  public staffName: string;
  public createdDate: string;
  public phoneCountryCode: string;
  public customerCode: string;

  constructor(data?: any) {
    if (!data) {
      return;
    }
    this.id = data.id;
    this.membershipType = data.membershipType;
    this.expiredDate = data.expiredDate;
    this.comment = data.comment;
    this.phoneCustomer = data.phoneCustomer;
    this.customerName = data.customerName;
    this.outletName = data.outletName;
    this.staffName = data.staffName;
    this.createdDate = data.createdDate;
    this.phoneCountryCode = data.phoneCountryCode;
    this.customerCode = data.customerCode;
  }

  static buildMembershipTransaction(item: MembershipTransaction): MembershipTransaction {
    const membershipTransaction = new MembershipTransaction();
    membershipTransaction.id = item.id;
    membershipTransaction.staffName = item.staffName ? item.staffName : '-';
    membershipTransaction.customerName = item.customerName ? item.customerName : 'Unknown';
    membershipTransaction.outletName = item.outletName ? item.outletName : '-';
    membershipTransaction.membershipType = item.membershipType ? item.membershipType : '-';
    membershipTransaction.expiredDate = item.expiredDate ?
      (new Date(item.expiredDate).getFullYear() !== 1 ?
        moment.utc(item.expiredDate).local().format('DD/MM/YYYY HH:mm A') : '-') : '-';
    membershipTransaction.createdDate = new Date(item.createdDate).getFullYear() !== 1 ?
      moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
    membershipTransaction.comment = item.comment ? item.comment : '-';
    membershipTransaction.phoneCustomer = `+${item.phoneCountryCode}${item.phoneCustomer}`;
    membershipTransaction.customerCode = item.customerCode;
    return membershipTransaction;
  }
}


export class AddMembershipCommand {
  public userId: string;
  public customerId: string;
  public outletId: string;
  public membershipTypeId: string;
  public expiredDate?: Date;
  public comment: string;
  public ipAddress: string;
  public membershipActionType: number;
}

export class GetMembershipTransactionsResponse {
  constructor(response?: any) {
    if (!response) {
      return;
    }
    this.listMembershipTransaction = response.listMembershipTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }

  public listMembershipTransaction: Array<MembershipTransaction>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;

}

export class VoidMemberTransactionRequest {
  constructor(Transrequest?: any) {
    if (!Transrequest) {
      return;
    }
    this.pageNumber = Transrequest.pageNumber;
    this.pageSize = Transrequest.pageSize;
    this.fromDateFilter = Transrequest.fromDateFilter;
    this.toDateFilter = Transrequest.toDateFilter;
    this.outletId = Transrequest.outletId;
  }

  public pageNumber: number;
  public pageSize: number;
  public fromDateFilter: string;
  public toDateFilter: string;
  public outletId?: string;
}

export class VoidMemberTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listMembershipTransaction = TransResponse.listMembershipTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
  }

  public listMembershipTransaction: Array<MembershipTransaction>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class AnnounceNewMembership {
  public expired: Date;
  public membershipName: string;
  public comment: string;
  public totalTransaction: number;
}

export class AnnounceEditComment {
  public comment: string;
  public totalTransaction: number;
}

export class CustomerMembershipTransaction {
  public id = '';
  public outletName = '';
  public membershipType = '';
  public expiredDate = '';
  public createdDate = '';
  public voided = false;
  public voidedBy = '';

  constructor(membershipTransaction?: any) {
    if (!membershipTransaction) { return; }
    this.id = membershipTransaction.id;
    this.outletName = membershipTransaction.outletName;
    this.membershipType = membershipTransaction.membershipType;
    this.createdDate = membershipTransaction.createdDate;
    this.voided = membershipTransaction.voided;
    this.voidedBy = membershipTransaction.voidedBy;
  }

  static buildMembershipTransaction(item: CustomerMembershipTransaction): CustomerMembershipTransaction {
    const membershipTransaction = new CustomerMembershipTransaction();
    membershipTransaction.id = item.id;
    membershipTransaction.outletName = item.outletName ? item.outletName : '-';
    membershipTransaction.membershipType = item.membershipType ? item.membershipType : '-';
    membershipTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    membershipTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    membershipTransaction.voided = item.voided;
    membershipTransaction.voidedBy = item.voidedBy;
    return membershipTransaction;
  }
}

export class CustomerMembershipTransactionsResponse {
  public listMembershipTransaction!: Array<CustomerMembershipTransaction>;
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

export enum MembershipActionType {
  Migration,
  New,
  Upgrade,
  Renew,
  Extend,
  Downgrade,
  Void,
  ChangeExpiredDate,
  Comment
}

export class OptionExpiryDate {
  public periodTime: number;
  public topupValue: number;

  constructor(res?: any) {
    this.periodTime = res.periodTime;
    this.topupValue = res.topupValue;
  }
}

