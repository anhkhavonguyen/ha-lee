import { Component, OnInit, Input, ViewChild, HostListener, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerTransactionRequest } from '../../models/user.model';
import { PointTransactionService } from '../../services/point-transactions.service';
import { WalletTransactionService } from '../../services/wallet-transactions.service';
import { CustomerPointTransaction } from '../../models/pointTransaction.model';
import { CustomerWalletTransaction } from '../../models/walletTransaction.model';
import { CustomerMembershipTransaction } from '../../models/membershipTransaction.model';
import { MembershipTransactionService } from '../../services/membership-transaction.service';

const screeSizeLargeDevices = 1024;
const screenSizeSmallDevices = 812;

@Component({
  selector: 'app-info-dialog',
  templateUrl: './info-dialog.component.html',
  styleUrls: ['./info-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoDialogComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private pointService: PointTransactionService,
    private walletService: WalletTransactionService,
    private membershipService: MembershipTransactionService) { }

  @Input() isViewHistory = false;
  @Input() typeHistory;
  @Input() userId;

  @ViewChild('tableTransactionPoint') tableTransactionPoint: any;
  @ViewChild('tableTransactionWallet') tableTransactionWallet: any;
  @ViewChild('tableTransactionMembership') tableTransactionMembership: any;
  rows: any[] = [];
  expanded: any = {};
  public isLargeDevices = false;
  public isSmallDevices = false;

  public typePointHistory = 'Point';
  public typeWalletHistory = 'Wallet';
  public typeMembershipHistory = 'Membership';

  public pageSize = 5;
  public currentVisible = 3;
  public pageNumberPoint = 0;
  public pageNumberWallet = 0;
  public pageNumberMembership = 0;

  public pointTransactionList: Array<CustomerPointTransaction> = [];
  public walletTransactionList: Array<CustomerWalletTransaction> = [];
  public membershipTransactionList: Array<CustomerMembershipTransaction> = [];

  public loadingIndicatorPointTransaction = true;
  public loadingIndicatorWalletTransaction = true;
  public loadingIndicatorMembershipTransaction = true;

  public totalMembershipTransaction = 0;
  public totalItemPointTransaction = 0;
  public totalWalletTransaction = 0;

  public header = '';
  public content = '';

  ngOnInit() {
    this.getScreedDisplay();
    switch (this.typeHistory) {
      case this.typePointHistory:
        this.getTransactionPointHistory({ offset: 0 });
        break;
      case this.typeWalletHistory:
        this.getTransactionWalletHistory({ offset: 0 });
        break;
      case this.typeMembershipHistory:
        this.getTransactionMembershipHistory({ offset: 0});
        break;
    }
  }

  getScreedDisplay() {
    if (window.screen.width <= screeSizeLargeDevices && window.screen.width > screenSizeSmallDevices) {
      this.isLargeDevices = true;
      this.isSmallDevices = false;
    }
    if (window.screen.width <= screenSizeSmallDevices) {
      this.isLargeDevices = false;
      this.isSmallDevices = true;
    }
    if (window.screen.width > screeSizeLargeDevices) {
      this.isLargeDevices = false;
      this.isSmallDevices = false;
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    if (event.target.innerWidth <= screeSizeLargeDevices && event.target.innerWidth >= screenSizeSmallDevices) {
      this.isLargeDevices = true;
      this.isSmallDevices = false;
    }
    if (event.target.innerWidth < screenSizeSmallDevices) {
      this.isLargeDevices = false;
      this.isSmallDevices = true;
    }
    if (event.target.innerWidth > screeSizeLargeDevices) {
      this.isLargeDevices = false;
      this.isSmallDevices = false;
    }
  }

  getTransactionMembershipHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userId,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };
    this.membershipService.getCustomerMembershipTransactions(requestHistoryTransaction).subscribe(res => {
      const temp = res;
      this.pageNumberMembership = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalMembershipTransaction = temp.totalItem;
      this.loadingIndicatorMembershipTransaction = false;

      this.membershipTransactionList = temp.listMembershipTransaction.map(result => {
        const membershipTransactionModel = CustomerMembershipTransaction.buildMembershipTransaction(result);
        return membershipTransactionModel;
      });
    });
  }

  getTransactionPointHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userId,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };
    this.pointService.getCustomerPointTransactions(requestHistoryTransaction).subscribe(res => {
      const temp = res;
      this.pageNumberPoint = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemPointTransaction = temp.totalItem;
      this.loadingIndicatorPointTransaction = false;

      this.pointTransactionList = temp.listPointTransaction.map(result => {
        const pointTransactionModel = CustomerPointTransaction.buildPointTransaction(result);
        return pointTransactionModel;
      });
    });
  }

  getTransactionWalletHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userId,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };
    this.walletService.getCustomerWalletTransactions(requestHistoryTransaction).subscribe(res => {
      const temp = res;
      this.pageNumberPoint = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalWalletTransaction = temp.totalItem;
      this.loadingIndicatorWalletTransaction = false;

      this.walletTransactionList = temp.listWalletTransaction.map(result => {
        const walletTransactionModel = CustomerWalletTransaction.buildWalletTransaction(result);
        return walletTransactionModel;
      });
    });
  }

  toggleExpandRowPoint(row) {
    this.tableTransactionPoint.rowDetail.toggleExpandRow(row);
  }

  toggleExpandRowWallet(row) {
    this.tableTransactionWallet.rowDetail.toggleExpandRow(row);
  }

  toggleExpandRowMembership(row) {
    this.tableTransactionMembership.rowDetail.toggleExpandRow(row);
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }
}
