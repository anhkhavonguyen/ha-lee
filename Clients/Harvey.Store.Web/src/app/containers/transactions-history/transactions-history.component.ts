import { Component, OnInit, ViewEncapsulation, ViewChild, HostListener } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { SharedService } from '../../shared/services/shared.service';
import { MembershipTransaction } from '../../shared/models/membershipTransaction.model';
import { PointTransaction } from '../../shared/models/pointTransaction.model';
import { WalletTransaction } from '../../shared/models/walletTransaction.model';
import { MembershipTransactionService } from '../../shared/services/membership-transaction.service';
import { PointTransactionService } from '../../shared/services/point-transactions.service';
import { WalletTransactionService } from '../../shared/services/wallet-transactions.service';
import { GetTransactionsByOutletRequest, Outlet } from '../../shared/models/outlet.model';
import { PageName } from '../../shared/constants/routing.constant';
import { UserService } from '../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InfoDialogComponent } from 'src/app/shared/components/info-dialog/info-dialog.component';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../shared/constants/common.constant';

const screeSizeLargeDevices = 1024;
const screenSizeSmallDevices = 568;

@Component({
  selector: 'app-transactions-history',
  templateUrl: './transactions-history.component.html',
  styleUrls: ['./transactions-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TransactionsHistoryComponent implements OnInit {

  public totalMembershipTransaction = 0;
  public totalItemPointTransaction = 0;
  public totalWalletTransaction = 0;

  public currentOutlet = new Outlet();
  public customerAge = 0;
  public pageNumberMembership = 0;
  public pageNumberPoint = 0;
  public pageNumberWallet = 0;
  public pageSize = 10;

  public membershipTransactionList: Array<MembershipTransaction> = [];
  public pointTransactionList: Array<PointTransaction> = [];
  public walletTransactionList: Array<WalletTransaction> = [];
  public loadingIndicatorMembershipTransaction = true;
  public loadingIndicatorPointTransaction = true;
  public loadingIndicatorWalletTransaction = true;
  public userToken = '';
  public userRoles = [];

  public appSettings: any;
  public outletAvatar: string;

  @ViewChild('tableTransactionPoint') tableTransactionPoint: any;
  @ViewChild('tableTransactionWallet') tableTransactionWallet: any;
  @ViewChild('tableTransactionMembership') tableTransactionMembership: any;
  rows: any[] = [];
  expanded: any = {};
  public isLargeDevices = false;
  public isSmallDevices = false;

  public panelColorProperties = {};

  constructor(private authService: AuthService,
    private membershipTransactionService: MembershipTransactionService,
    private pointTransactionService: PointTransactionService,
    private walletTransactionService: WalletTransactionService, private sharedService: SharedService,
    private userService: UserService, private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private modalService: NgbModal) {
    this.userToken = this.authService.getTokenFormStorage();
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.userService.getUserRole(this.userToken).subscribe(res => {
        this.userRoles = res.roles;
      });
    }
  }

  ngOnInit() {
    if (!this.sharedService.getAnnounceCurrentUser()) {
      this.authService.logout();
    }
    if (!this.sharedService.getAnnounceOutlet()) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.userToken = this.authService.getTokenFormStorage();
    this.setPageMembershipTransaction({ offset: 0 });
    this.setPagePointTransaction({ offset: 0 });
    this.setPageWalletTransaction({ offset: 0 });
    this.getScreedDisplay();
    this.outletAvatar = this.getOutletAvatarImage();
    this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
  }

  getScreedDisplay() {
    if (window.screen.width <= screeSizeLargeDevices && window.screen.width >= screenSizeSmallDevices) {
      this.isLargeDevices = true;
      this.isSmallDevices = false;
    }
    if (window.screen.width < screenSizeSmallDevices) {
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

  isExtraLargeDevices() {
    if (!this.isLargeDevices && !this.isSmallDevices) {
      return true;
    } else {
      return false;
    }
  }

  onClickHomePageButton() {
    this.sharedService.routingToPage(PageName.HOME_PAGE);
  }

  onClickServingButton() {
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickListCustomerButton() {
    this.sharedService.routingToPage(PageName.CUSTOMER_LISTING_PAGE);
  }

  onClickLogoutButton() {
    this.authService.logout();
  }

  onClickChangePasswordButton() {
    this.sharedService.routingToPage(PageName.CHANGE_PASSWORD_PAGE);
  }

  onClickDashboardButton() {
    this.sharedService.routingToPage(PageName.DASHBOARD_PAGE);
  }

  public loadMembershipTransaction(request: GetTransactionsByOutletRequest) {
    this.membershipTransactionService.getMembershipTransactions(request).subscribe(res => {
      const temp = res;
      this.pageNumberMembership = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalMembershipTransaction = temp.totalItem;
      this.loadingIndicatorMembershipTransaction = false;

      this.membershipTransactionList = temp.listMembershipTransaction.map(result => {
        const membershipTransactionModel = MembershipTransaction.buildMembershipTransaction(result);
        return membershipTransactionModel;
      });
    });
  }

  public loadPointTransaction(request: GetTransactionsByOutletRequest) {
    this.pointTransactionService.getPointTransactions(this.userToken, request).subscribe(res => {
      const temp = res;
      this.pageNumberPoint = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemPointTransaction = temp.totalItem;
      this.loadingIndicatorPointTransaction = false;

      this.pointTransactionList = temp.pointTransactionModels.map(response => {
        const pointTransactionModel = PointTransaction.buildPointTransaction(response);
        return pointTransactionModel;
      });
    });
  }

  public loadWalletTransaction(request: GetTransactionsByOutletRequest) {
    this.walletTransactionService.getWalletTransactions(this.userToken, request).subscribe(res => {
      const temp = res;
      this.pageNumberWallet = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalWalletTransaction = temp.totalItem;
      this.loadingIndicatorWalletTransaction = false;

      this.walletTransactionList = temp.listWalletTransaction.map(response => {
        const walletTransactionModel = WalletTransaction.buildWalletTransaction(response);
        return walletTransactionModel;
      });
    });
  }


  setPageMembershipTransaction(pageInfo) {
    const request: GetTransactionsByOutletRequest = {
      outletId: this.currentOutlet.id,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadMembershipTransaction(request);
  }

  setPagePointTransaction(pageInfo) {
    const request: GetTransactionsByOutletRequest = {
      outletId: this.currentOutlet.id,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadPointTransaction(request);
  }

  setPageWalletTransaction(pageInfo) {
    const request: GetTransactionsByOutletRequest = {
      outletId: this.currentOutlet.id,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadWalletTransaction(request);
  }

  checkPermissionCurrentUser(roleName: string) {
    if (this.userRoles.indexOf(roleName) !== -1) {
      return true;
    }
    return false;
  }

  onClickShowComment(comment: any) {
    const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.header = 'Comment';
    instance.content = comment;
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

  onDetailToggle(event) {
  }

  private getOutletAvatarImage() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppHeaderLogoImage));
    if (appSetting.value) {
      return `data:image/jpg;base64,${appSetting.value}`;
    }
    return '';
  }
}
