import * as moment from 'moment';

export class WalletTransaction {
  constructor(walletTransaction?: any) {
    if (!walletTransaction) {
      return;
    }
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
    this.customerCode = walletTransaction.customerCode;
  }

  public id: string;
  public staffName: string;
  public customerName: string;
  public outletName: string;
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

  static buildWalletTransaction(item: WalletTransaction): WalletTransaction {
    const walletTransaction = new WalletTransaction();
    walletTransaction.id = item.id;
    walletTransaction.staffName = item.staffName ? item.staffName : '-';
    walletTransaction.outletName = item.outletName ? item.outletName : '-';
    walletTransaction.comment = item.comment ? item.comment : '-';
    walletTransaction.createdDate = new Date(item.createdDate).getFullYear() !== 1 ?
      moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
    walletTransaction.debit = item.debit ? item.debit : 0;
    walletTransaction.credit = item.credit ? item.credit : 0;
    walletTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    walletTransaction.voided = item.voided;
    walletTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
    walletTransaction.phoneCustomer = `+${item.phoneCountryCode}${item.phoneCustomer}`;
    walletTransaction.customerName = item.customerName ? item.customerName : 'Unknown';
    walletTransaction.customerCode = item.customerCode;
    return walletTransaction;
  }
}

export class GetWalletTransactionsResponse {
  constructor(response?: any) {
    if (!response) {
      return;
    }
    this.listWalletTransaction = response.listWalletTransaction;
    this.pageNumber = response.pageNumber;
    this.pageSize = response.pageSize;
    this.totalItem = response.totalItem;
  }

  public listWalletTransaction: Array<WalletTransaction>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;

}

export class TopUpWalletRequest {
  public userId: string;
  public customerId: string;
  public outletId: string;
  public value: number;
  public ipAddress: string;
}

export class TopUpResponse {
  public balance: number;
}

export class SpendWalletRequest {
  public userId: string;
  public customerId: string;
  public outletId: string;
  public value: number;
  public ipAddress: string;
  public staffId: string;
  public createdByName: string;
}

export class SpendWalletResponse {
  public balance: number;
}

export class TotalBalanceWalletTransactionRequest {
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

export class TotalBalanceWalletTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.totalBalance = TransResponse.totalBalance;
  }

  public totalBalance: number;

}

export class CreditWalletTransactionRequest {
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

export class CreditWalletTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listWalletTransaction = TransResponse.listWalletTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalCreditValue = TransResponse.totalCreditValue;
  }

  public listWalletTransaction: Array<WalletTransaction>;
  public totalCreditValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class DebitWalletTransactionRequest {
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

export class DebitWalletTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listWalletTransaction = TransResponse.listWalletTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalDebitValue = TransResponse.totalDebitValue;
  }

  public listWalletTransaction: Array<WalletTransaction>;
  public totalDebitValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class VoidOfCreditWalletTransactionRequest {
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

export class VoidOfCreditWalletTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listWalletTransaction = TransResponse.listWalletTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalVoidOfCreditValue = TransResponse.totalVoidOfCreditValue;
  }

  public listWalletTransaction: Array<WalletTransaction>;
  public totalVoidOfCreditValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class VoidOfDebitWalletTransactionRequest {
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

export class VoidOfDebitWalletTransactionResponse {
  constructor(TransResponse?: any) {
    if (!TransResponse) {
      return;
    }
    this.listWalletTransaction = TransResponse.listWalletTransaction;
    this.pageNumber = TransResponse.pageNumber;
    this.pageSize = TransResponse.pageSize;
    this.totalItem = TransResponse.totalItem;
    this.totalVoidOfDebitValue = TransResponse.totalVoidOfDebitValue;
  }

  public listWalletTransaction: Array<WalletTransaction>;
  public totalVoidOfDebitValue: number;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class CustomerWalletTransaction {
  public id = '';
  public outletName = '';
  public expiredDate = '';
  public createdDate = '';
  public debit = 0;
  public credit = 0;
  public voided = false;
  public voidedBy = '';
  public balanceTotal = 0;

  constructor(walletTransaction?: any) {
    if (!walletTransaction) { return; }
    this.id = walletTransaction.id;
    this.outletName = walletTransaction.outletName;
    this.createdDate = walletTransaction.createdDate;
    this.debit = walletTransaction.debit;
    this.credit = walletTransaction.credit;
    this.balanceTotal = walletTransaction.balanceTotal;
    this.voided = walletTransaction.voided;
    this.voidedBy = walletTransaction.voidedBy;
  }

  static buildWalletTransaction(item: CustomerWalletTransaction): CustomerWalletTransaction {
    const walletTransaction = new CustomerWalletTransaction();
    walletTransaction.id = item.id;
    walletTransaction.outletName = item.outletName ? item.outletName : '-';
    walletTransaction.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
      ? moment.utc(item.expiredDate).local().format('DD/MM/YYYY') : '-';
    walletTransaction.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
      ? moment.utc(item.createdDate).local().format('DD/MM/YYYY') : '-';
    walletTransaction.debit = item.debit ? item.debit : 0;
    walletTransaction.credit = item.credit ? item.credit : 0;
    walletTransaction.balanceTotal = item.balanceTotal ? item.balanceTotal : 0;
    walletTransaction.voided = item.voided;
    walletTransaction.voidedBy = item.voidedBy ? item.voidedBy : '-';
    return walletTransaction;
  }
}

export class CustomerWalletTransactionsResponse {
  public listWalletTransaction!: Array<CustomerWalletTransaction>;
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
