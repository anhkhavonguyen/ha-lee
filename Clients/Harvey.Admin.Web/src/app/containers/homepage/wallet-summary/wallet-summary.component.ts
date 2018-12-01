import { Component, OnInit, Input, SimpleChange } from '@angular/core';
import { WalletTransaction } from 'src/app/containers/customer/customer-detail/customer-detail.model';
import { HomepageService } from 'src/app/containers/homepage/homepage.service';
import { DebitWalletTransactionRequest,
  VoidOfDebitWalletTransactionRequest,
  VoidOfCreditWalletTransactionRequest,
  CreditPointTransactionRequest,
  CreditWalletTransactionRequest} from 'src/app/containers/homepage/homepage.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-wallet-summary',
  templateUrl: './wallet-summary.component.html',
  styleUrls: ['./wallet-summary.component.scss']
})
export class WalletSummaryComponent implements OnInit {

  constructor(
    private homepageService: HomepageService
  ) { }

  public debitWalletTransactionList: Array<WalletTransaction> = [];
  public totalItemDebitWalletTransaction = 0;
  public isCollapsedDebitWalletTrans = false;
  public pageNumberDebitWallet = 0;
  public loadingIndicatorDebitWalletTransaction = true;
  public isLoadingDebitWalletTransaction = false;

  public creditWalletTransactionList: Array<WalletTransaction> = [];
  public totalItemCreditWalletTransaction = 0;
  public isCollapsedCreditWalletTrans = false;
  public pageNumberCreditWallet = 0;
  public loadingIndicatorCreditWalletTransaction = true;
  public isLoadingCreditWalletTransaction = false;

  public voidOfDebitWalletTransactionList: Array<WalletTransaction> = [];
  public totalItemVoidOfDebitWalletTransaction = 0;
  public isCollapsedVoidOfDebitWalletTrans = false;
  public pageNumberVoidOfDebitWallet = 0;
  public loadingIndicatorVoidOfDebitWalletTransaction = true;
  public isLoadingVoidOfDebitWalletTransaction = false;

  public voidOfCreditWalletTransactionList: Array<WalletTransaction> = [];
  public totalItemVoidOfCreditWalletTransaction = 0;
  public isCollapsedVoidOfCreditWalletTrans = false;
  public pageNumberVoidOfCreditWallet = 0;
  public loadingIndicatorVoidOfCreditWalletTransaction = true;
  public isLoadingVoidOfCreditWalletTransaction = false;

  public totalBalanceWalletTrans = 0;
  public totalCreditWalletTrans = 0;
  public totalDebitWalletTrans = 0;
  public totalVoidOfCreditWalletTrans = 0;
  public totalVoidOfDebitWalletTrans = 0;

  @Input() outletRequest = undefined;
  @Input() fromDateRequest: any;
  @Input() toDateRequest: any;
  @Input() appSummaryLabelTitle = '';
  public pageSize = 10;
  public loadingTotalBalancePointTransaction = true;
  @Input() eventUpdateWalletData: Subject<any> = new Subject();

  ngOnInit() {
    this.setDataSummary();
    this.eventUpdateWalletData.subscribe(event => {
      this.setDataSummary();
    });
  }

  setIsLoadingData() {
    this.isLoadingCreditWalletTransaction = true;
    this.isLoadingDebitWalletTransaction = true;
    this.isLoadingVoidOfCreditWalletTransaction = true;
    this.isLoadingVoidOfDebitWalletTransaction = true;
  }

  public loadDebitValueWalletTransaction(request: DebitWalletTransactionRequest) {
    this.loadingIndicatorDebitWalletTransaction = true;
    this.homepageService.GetDebitValueWalletTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberDebitWallet = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemDebitWalletTransaction = temp.totalItem;
      this.loadingIndicatorDebitWalletTransaction = false;
      this.isLoadingDebitWalletTransaction = false;
      this.totalDebitWalletTrans = temp.totalDebitValue;
      if (temp.listWalletTransaction != null) {
        this.debitWalletTransactionList = temp.listWalletTransaction.map((result: any) => {
          const pointTransactionModel = WalletTransaction.buildWalletTransaction(result);
          return pointTransactionModel;
        });
      }
      this.loadTotalBalanceWalletTransaction();
    });
  }

  public loadCreditValueWalletTransaction(request: CreditPointTransactionRequest) {
    this.loadingIndicatorCreditWalletTransaction = true;
    this.homepageService.GetCreditValueWalletTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberCreditWallet = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemCreditWalletTransaction = temp.totalItem;
      this.totalCreditWalletTrans = temp.totalCreditValue;
      this.loadingIndicatorCreditWalletTransaction = false;
      this.isLoadingCreditWalletTransaction = false;
      if (temp.listWalletTransaction != null) {
        this.creditWalletTransactionList = temp.listWalletTransaction.map((result: any) => {
          const walletTransactionModel = WalletTransaction.buildWalletTransaction(result);
          return walletTransactionModel;
        });
      }
      this.loadTotalBalanceWalletTransaction();
    });
  }

  public loadVoidOfCreditValueWalletTransaction(request: VoidOfCreditWalletTransactionRequest) {
    this.loadingIndicatorVoidOfCreditWalletTransaction = true;
    this.homepageService.GetVoidOfCreditValueWalletTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberVoidOfCreditWallet = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemVoidOfCreditWalletTransaction = temp.totalItem;
      this.totalVoidOfCreditWalletTrans = temp.totalVoidOfCreditValue;
      this.loadingIndicatorVoidOfCreditWalletTransaction = false;
      this.isLoadingVoidOfCreditWalletTransaction = false;
      if (temp.listWalletTransaction != null) {
        this.voidOfCreditWalletTransactionList = temp.listWalletTransaction.map((result: any) => {
          const walletTransactionModel = WalletTransaction.buildWalletTransaction(result);
          return walletTransactionModel;
        });
      }
      this.loadTotalBalanceWalletTransaction();
    });
  }

  public loadVoidOfDebitValueWalletTransaction(request: VoidOfDebitWalletTransactionRequest) {
    this.loadingIndicatorVoidOfDebitWalletTransaction = true;
    this.homepageService.GetVoidOfDebitValueWalletTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberVoidOfDebitWallet = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemVoidOfDebitWalletTransaction = temp.totalItem;
      this.loadingIndicatorVoidOfDebitWalletTransaction = false;
      this.isLoadingVoidOfDebitWalletTransaction = false;
      this.totalVoidOfDebitWalletTrans = temp.totalVoidOfDebitValue;
      if (temp.listWalletTransaction != null) {
        this.voidOfDebitWalletTransactionList = temp.listWalletTransaction.map((result: any) => {
          const walletTransactionModel = WalletTransaction.buildWalletTransaction(result);
          return walletTransactionModel;
        });
      }
      this.loadTotalBalanceWalletTransaction();
    });
  }

  setDebitValueWalletTransactionPage(pageInfo: { offset: number; }) {
    const request: DebitWalletTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadDebitValueWalletTransaction(request);
  }

  setCreditValueWalletTransactionPage(pageInfo: { offset: number; }) {
    const request: CreditWalletTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadCreditValueWalletTransaction(request);
  }

  setVoidOfCreditValueWalletTransactionPage(pageInfo: { offset: number; }) {
    const request: VoidOfCreditWalletTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadVoidOfCreditValueWalletTransaction(request);
  }

  setVoidOfDebitValueWalletTransactionPage(pageInfo: { offset: number; }) {
    const request: VoidOfDebitWalletTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadVoidOfDebitValueWalletTransaction(request);
  }

  public loadTotalBalanceWalletTransaction() {
    const floatNumber =
      this.calculateFloatNumber([
        this.totalCreditWalletTrans,
        this.totalVoidOfDebitWalletTrans,
        this.totalDebitWalletTrans,
        this.totalVoidOfCreditWalletTrans]);
    this.totalBalanceWalletTrans =
      (this.totalCreditWalletTrans * floatNumber
        + this.totalVoidOfDebitWalletTrans * floatNumber
        - this.totalDebitWalletTrans * floatNumber
        - this.totalVoidOfCreditWalletTrans * floatNumber) / floatNumber;
  }

  private calculateFloatNumber(arrNumber: number[]) {
    if (arrNumber.length === 0) {
      return 0;
    }
    let maxDecimalPlace = this.getDecimalPlace(arrNumber[0]);
    for (let i = 0; i < arrNumber.length; i++) {
      if (this.getDecimalPlace(arrNumber[i]) > maxDecimalPlace) {
        maxDecimalPlace = this.getDecimalPlace(arrNumber[i]);
      }
    }
    return Math.pow(10, maxDecimalPlace);
  }

  private getDecimalPlace(number: number) {
    const valueNumbers = number.toString().split('.');
    return valueNumbers.length > 1 ? valueNumbers[1].length : 0;
  }

  setDataSummary() {
    this.setIsLoadingData();
    this.setCreditValueWalletTransactionPage({
      offset: 0
    });
    this.setDebitValueWalletTransactionPage({
      offset: 0
    });
    this.setVoidOfCreditValueWalletTransactionPage({
      offset: 0
    });
    this.setVoidOfDebitValueWalletTransactionPage({
      offset: 0
    });
    this.loadTotalBalanceWalletTransaction();
  }
}

