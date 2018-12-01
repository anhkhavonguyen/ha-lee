import * as moment from 'moment';

export class AddPointRequest {
  public customerId: string;
  public value: number;
  public outletId: string;
  public comment: string;
  public userId: string;
  public ipAddress: string;

  constructor(request?: any) {
    if (!request) {
      return;
    }
    this.customerId = request.customerId;
    this.value = request.value;
    this.outletId = request.outletId;
    this.comment = request.comment;
    this.userId = request.userId;
    this.ipAddress = request.ipAddress;
  }
}

export class AddPointResponse {
  public balance: number;
}


export class RedeemPointRequest {
  public customerId: string;
  public value: number;
  public outletId: string;
  public userId: string;
  public ipAddress: string;
}

export class PointTransaction {
  public id: string;
  public staffName: string;
  public customerName: string;
  public outletName: string;
  public expiredDate: string;
  public comment: string;
  public createdDate: string;
  public phoneCustomer: string;
  public debit: number;
  public credit: number;
  public voided: boolean;
  public voidedBy: string;
  public balanceTotal: number;
  public phoneCountryCode: string;
  public customerCode: string;

  constructor(pointTransaction?: any) {
    if (!pointTransaction) {
      return;
    }
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
    this.customerCode = pointTransaction.customerCode;
  }

  static buildPointTransaction(item: PointTransaction): PointTransaction {
    const pointTransaction = new PointTransaction();
    pointTransaction.id = item.id;
    pointTransaction.staffName = item.staffName ? item.staffName : '-';
    pointTransaction.outletName = item.outletName ? item.outletName : '-';
    pointTransaction.comment = item.comment ? item.comment : '-';
    pointTransaction.expiredDate = item.expiredDate ?
      (new Date(item.expiredDate).getFullYear() !== 1 ?
        moment.utc(item.expiredDate).local().format('DD/MM/YYYY HH:mm A') : '-') : '-';
    pointTransaction.createdDate = new Date(item.createdDate).getFullYear() !== 1 ?
      moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
    pointTransaction.debit = item.debit ? item.debit : 0;
    pointTransaction.credit = item.credit ? item.credit : 0;
    pointTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    pointTransaction.voided = item.voided;
    pointTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
    pointTransaction.phoneCustomer = `+${item.phoneCountryCode}${item.phoneCustomer}`;
    pointTransaction.customerName = item.customerName ? item.customerName : 'Unknown';
    pointTransaction.customerCode = item.customerCode;
    return pointTransaction;
  }
}

export class GetPointTransactionsResponse {
  constructor(response?: any) {
    if (!response) {
      return;
    }
    this.listPointTransaction = response.listPointTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }

  public listPointTransaction: Array<PointTransaction>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;

}

export class DebitPointTransactionRequest {
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

export class DebitPointTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listPointTransaction = TransResponse.listPointTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalDebitValue = TransResponse.totalDebitValue;
  }

  public listPointTransaction: Array<PointTransaction>;
  public totalDebitValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class TotalBalancePointTransactionRequest {
  constructor(Transrequest?: any) {
    if (!Transrequest) {
      return;
    }
    this.fromDateFilter = Transrequest.fromDateFilter;
    this.toDateFilter = Transrequest.toDateFilter;
    this.outletId = Transrequest.outletId;
  }

  public fromDateFilter: string;
  public toDateFilter: string;
  public outletId?: string;
}

export class TotalBalancePointTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.totalBalance = TransResponse.totalBalance;
  }

  public totalBalance: number;

}

export class CreditPointTransactionRequest {
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

export class CreditPointTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listPointTransaction = TransResponse.listPointTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalCreditValue = TransResponse.totalCreditValue;
  }

  public listPointTransaction: Array<PointTransaction>;
  public totalCreditValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class VoidOfCreditPointTransactionRequest {
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

export class VoidOfCreditPointTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listPointTransaction = TransResponse.listPointTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalVoidOfCreditValue = TransResponse.totalVoidOfCreditValue;
  }

  public listPointTransaction: Array<PointTransaction>;
  public totalVoidOfCreditValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class VoidOfDebitPointTransactionRequest {
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

export class VoidOfDebitPointTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listPointTransaction = TransResponse.listPointTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalVoidOfDebitValue = TransResponse.totalVoidOfDebitValue;
  }

  public listPointTransaction: Array<PointTransaction>;
  public totalVoidOfDebitValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class CustomerPointTransaction {
  public id = '';
  public outletName = '';
  public expiredDate = '';
  public createdDate = '';
  public debit = 0;
  public credit = 0;
  public voided = false;
  public voidedBy = '';
  public balanceTotal = 0;

  constructor(pointTransaction?: any) {
    if (!pointTransaction) { return; }
    this.id = pointTransaction.id;
    this.outletName = pointTransaction.outletName;
    this.createdDate = pointTransaction.createdDate;
    this.debit = pointTransaction.debit;
    this.credit = pointTransaction.credit;
    this.balanceTotal = pointTransaction.balanceTotal;
    this.voided = pointTransaction.voided;
    this.voidedBy = pointTransaction.voidedBy;
  }

  static buildPointTransaction(item: CustomerPointTransaction): CustomerPointTransaction {
    const pointTransaction = new CustomerPointTransaction();
    pointTransaction.id = item.id;
    pointTransaction.outletName = item.outletName ? item.outletName : '-';
    pointTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    pointTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    pointTransaction.debit = item.debit ? item.debit : 0;
    pointTransaction.credit = item.credit ? item.credit : 0;
    pointTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    pointTransaction.voided = item.voided;
    pointTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
    return pointTransaction;
  }
}

export class CustomerPointTransactionsResponse {
  public listPointTransaction!: Array<CustomerPointTransaction>;
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
