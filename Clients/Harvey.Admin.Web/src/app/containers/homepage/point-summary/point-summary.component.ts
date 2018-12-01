import { Component, OnInit, Input, SimpleChange } from '@angular/core';
import { PointTransaction } from 'src/app/containers/customer/customer-detail/customer-detail.model';
import { HomepageService } from 'src/app/containers/homepage/homepage.service';
import { DebitPointTransactionRequest,
  CreditPointTransactionRequest,
  VoidOfDebitPointTransactionRequest,
  VoidOfCreditPointTransactionRequest } from 'src/app/containers/homepage/homepage.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-point-summary',
  templateUrl: './point-summary.component.html',
  styleUrls: ['./point-summary.component.scss']
})
export class PointSummaryComponent implements OnInit {

  constructor(
    private homepageService: HomepageService
  ) { }

  public debitValuePointTransactionList: Array<PointTransaction> = [];
  public totalItemDebitValuePointTransaction = 0;
  public isCollapseDebitValuePointTransaction = false;
  public pageNumberDebitValuePointTransaction = 0;
  public loadingIndicatorDebitValuePointTransaction = true;
  public isLoadingDebitValuePointTransaction = false;

  public creditPointTransactionList: Array<PointTransaction> = [];
  public totalItemCreditPointTransaction = 0;
  public isCollapsedCreditPointTransaction = false;
  public pageNumberCreditPointTransaction = 0;
  public loadingIndicatorCreditPointTransaction = true;
  public isLoadingCreditPointTransaction = false;

  public voidOfDebitValuePointTransactionList: Array<PointTransaction> = [];
  public totalItemVoidOfDebitValuePointTransaction = 0;
  public isCollapseVoidOfDebitValuePointTransaction = false;
  public pageNumberVoidOfDebitValuePointTransaction = 0;
  public loadingIndicatorVoidOfDebitValuePointTransaction = true;
  public isLoadingVoidOfDebitValuePointTransaction = false;

  public voidOfCreditPointTransactionList: Array<PointTransaction> = [];
  public totalItemVoidOfCreditPointTransaction = 0;
  public isCollapsedVoidOfCreditPointTransaction = false;
  public pageNumberVoidOfCreditPointTransaction = 0;
  public loadingIndicatorVoidOfCreditPointTransaction = true;
  public isLoadingVoidOfCreditPointTransaction = false;

  @Input() outletRequest = undefined;
  @Input() fromDateRequest: any;
  @Input() toDateRequest: any;
  @Input() appSummaryLabelTitle = '';
  public pageSize = 10;
  public loadingTotalBalancePointTransaction = true;

  public totalBalancePointTrans = 0;
  public totalCreditPointTrans = 0;
  public totalDebitPointTrans = 0;
  public totalVoidOfCreditPointTrans = 0;
  public totalVoidOfDebitPointTrans = 0;
  @Input() eventUpdatePointData: Subject<any> = new Subject();

  ngOnInit() {
    this.setDataSummary();
    this.eventUpdatePointData.subscribe(event => {
      this.setDataSummary();
    });
  }

  setIsLoadingData() {
    this.isLoadingCreditPointTransaction = true;
    this.isLoadingDebitValuePointTransaction = true;
    this.isLoadingVoidOfCreditPointTransaction = true;
    this.isLoadingVoidOfDebitValuePointTransaction = true;
  }

  public loadDebitValuePointTransaction(request: DebitPointTransactionRequest) {
    this.loadingIndicatorDebitValuePointTransaction = true;
    this.homepageService.GetDebitValuePointTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberDebitValuePointTransaction = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemDebitValuePointTransaction = temp.totalItem;
      this.loadingIndicatorDebitValuePointTransaction = false;
      this.isLoadingDebitValuePointTransaction = false;
      this.totalDebitPointTrans = temp.totalDebitValue;
      if (temp.listPointTransaction != null) {
        this.debitValuePointTransactionList = temp.listPointTransaction.map((result: any) => {
          const pointTransactionModel = PointTransaction.buildPointTransaction(result);
          return pointTransactionModel;
        });
      }
      this.loadTotalBalancePointTransaction();
    });
  }

  public loadVoidOfCreditValuePointTransaction(request: VoidOfCreditPointTransactionRequest) {
    this.loadingIndicatorVoidOfCreditPointTransaction = true;
    this.homepageService.GetVoidOfCreditValuePointTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberVoidOfCreditPointTransaction = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemVoidOfCreditPointTransaction = temp.totalItem;
      this.totalVoidOfCreditPointTrans = temp.totalVoidOfCreditValue;
      this.loadingIndicatorVoidOfCreditPointTransaction = false;
      this.isLoadingVoidOfCreditPointTransaction = false;
      if (temp.listPointTransaction != null) {
        this.voidOfCreditPointTransactionList = temp.listPointTransaction.map((result: any) => {
          const pointTransactionModel = PointTransaction.buildPointTransaction(result);
          return pointTransactionModel;
        });
      }
      this.loadTotalBalancePointTransaction();
    });
  }

  public loadVoidOfDebitValuePointTransaction(request: VoidOfDebitPointTransactionRequest) {
    this.loadingIndicatorVoidOfDebitValuePointTransaction = true;
    this.homepageService.GetVoidOfDebitValuePointTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberVoidOfDebitValuePointTransaction = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemVoidOfDebitValuePointTransaction = temp.totalItem;
      this.loadingIndicatorVoidOfDebitValuePointTransaction = false;
      this.isLoadingVoidOfDebitValuePointTransaction = false;
      this.totalVoidOfDebitPointTrans = temp.totalVoidOfDebitValue;
      if (temp.listPointTransaction != null) {
        this.voidOfDebitValuePointTransactionList = temp.listPointTransaction.map((result: any) => {
          const pointTransactionModel = PointTransaction.buildPointTransaction(result);
          return pointTransactionModel;
        });
      }
      this.loadTotalBalancePointTransaction();
    });
  }

  public loadCreditValuePointTransaction(request: CreditPointTransactionRequest) {
    this.loadingIndicatorCreditPointTransaction = true;
    this.homepageService.GetCreditValuePointTransaction(request).subscribe(res => {
      const temp = res;
      this.pageNumberCreditPointTransaction = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemCreditPointTransaction = temp.totalItem;
      this.totalCreditPointTrans = temp.totalCreditValue;
      this.loadingIndicatorCreditPointTransaction = false;
      this.isLoadingCreditPointTransaction = false;
      if (temp.listPointTransaction != null) {
        this.creditPointTransactionList = temp.listPointTransaction.map((result: any) => {
          const pointTransactionModel = PointTransaction.buildPointTransaction(result);
          return pointTransactionModel;
        });
      }
      this.loadTotalBalancePointTransaction();
    });
  }

  setDebitValuePointTransactionPage(pageInfo: { offset: number; }) {
    const request: DebitPointTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadDebitValuePointTransaction(request);
  }

  setCreditValuePointTransactionPage(pageInfo: { offset: number; }) {
    const request: CreditPointTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadCreditValuePointTransaction(request);
  }

  setVoidOfCreditValuePointTransactionPage(pageInfo: { offset: number; }) {
    const request: VoidOfCreditPointTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadVoidOfCreditValuePointTransaction(request);
  }

  setVoidOfDebitValuePointTransactionPage(pageInfo: { offset: number; }) {
    const request: VoidOfDebitPointTransactionRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadVoidOfDebitValuePointTransaction(request);
  }

  public loadTotalBalancePointTransaction() {
    this.totalBalancePointTrans =
      this.totalCreditPointTrans + this.totalVoidOfDebitPointTrans - this.totalDebitPointTrans - this.totalVoidOfCreditPointTrans;
  }

  setDataSummary() {
    this.setIsLoadingData();
    this.setDebitValuePointTransactionPage({
      offset: 0
    });
    this.setCreditValuePointTransactionPage({
      offset: 0
    });
    this.setVoidOfCreditValuePointTransactionPage({
      offset: 0
    });
    this.setVoidOfDebitValuePointTransactionPage({
      offset: 0
    });
    this.loadTotalBalancePointTransaction();
  }
}

