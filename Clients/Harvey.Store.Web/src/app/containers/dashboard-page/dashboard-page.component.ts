import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared/services/shared.service';
import { User } from '../../shared/models/user.model';
import { Outlet } from '../../shared/models/outlet.model';
import { AuthService } from '../../auth/auth.service';
import { PageName } from '../../shared/constants/routing.constant';
import { UserService } from '../../shared/services/user.service';
import { RoleName } from '../../shared/constants/role.constant';
import { CustomerService } from '../../shared/services/customer.service';
import { FilterCustomerRequest } from '../../shared/models/customer.model';
import {
  CreditPointTransactionRequest,
  DebitPointTransactionRequest,
  VoidOfCreditPointTransactionRequest,
  VoidOfDebitPointTransactionRequest
} from '../../shared/models/pointTransaction.model';
import { PointTransactionService } from '../../shared/services/point-transactions.service';
import { WalletTransactionService } from '../../shared/services/wallet-transactions.service';
import {
  CreditWalletTransactionRequest,
  DebitWalletTransactionRequest,
  VoidOfCreditWalletTransactionRequest,
  VoidOfDebitWalletTransactionRequest
} from '../../shared/models/walletTransaction.model';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { VoidMemberTransactionRequest } from 'src/app/shared/models/membershipTransaction.model';
import { MembershipTransactionService } from 'src/app/shared/services/membership-transaction.service';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../shared/constants/common.constant';

@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})
export class DashboardPageComponent implements OnInit {

  public currentUser: User = new User();
  public currentOutlet: Outlet = new Outlet();
  private userToken = '';
  public nameRoleAdminStaff = RoleName.ROLE_ADMIN_STAFF;

  private userRoles = [];
  private loadingUserRoles = true;
  private appSettings: any;

  public pageSize = 10;
  public dateFilter = moment(new Date()).format('MM/DD/YYYY');
  public filteredFromDate = {
    year: new Date().getFullYear(),
    month: new Date().getMonth() + 1,
    day: new Date().getDate()
  };
  public filteredToDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
  public from_date_filter: any;
  public end_date_filter: any;
  public show_from_date_filter = '';
  public show_end_date_filter = '';

  public totalNewBasicCustomer = 0;
  public loadingTotalNewBasicCustomer = true;

  public totalUpgradedCustomer = 0;
  public loadingTotalUpgradedCustomer = true;

  public totalRenewedCustomer = 0;
  public loadingTotalRenewedCustomer = true;

  public totalExtendedCustomer = 0;
  public loadingTotalExtendedCustomer = true;

  public totalExpiredCustomer = 0;
  public loadingTotalExpiredCustomer = true;

  public totalDowngradedCustomer = 0;
  public loadingTotalDowngradedCustomer = true;

  public totalBalancePointTrans = 0;
  public loadingTotalBalancePointTrans = true;
  public totalAddPoints = 0;
  public loadingTotalAddPoints = true;
  public totalRedeemPoints = 0;
  public loadingTotalRedeemPoints = true;
  public totalVoidOfAddPoints = 0;
  public loadingTotalVoidOfAddPoint = true;
  public totalVoidOfRedeemPoints = 0;
  public loadingTotalVoidOfRedeemPoints = true;

  public totalWalletTranasctions = 0;
  public loadingTotalWalletTranasctions = true;
  public totalTopup = 0;
  public loadingTotalTopup = true;
  public totalSpend = 0;
  public loadingTotalSpend = true;
  public totalVoidTopup = 0;
  public loadingTotalVoidTopup = true;
  public totalVoidSpend = 0;
  public loadingTotalVoidSpend = true;

  public totalVoidMembership = 0;
  public loadingTotalVoidMembership = true;

  public isLoading = true;

  public outletAvatar: string;

  public panelColorProperties = {};

  constructor(private sharedService: SharedService,
    private authService: AuthService,
    private userService: UserService,
    private customerService: CustomerService,
    private pointTransactionService: PointTransactionService,
    private walletTransactionService: WalletTransactionService,
    private membershipTransactionService: MembershipTransactionService,
    private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService) {
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.initPage();
    }
  }

  initPage() {
    this.userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
    this.getUserRoles();
    this.createDateFilter(this.filteredFromDate, this.filteredToDate);
    this.getSummary();
  }

  ngOnInit() {
    this.checkAccessPage();
    this.outletAvatar = this.getOutletAvatarImage();
  }

  getSummary() {
    this.isLoading = true;
    this.getCustomerSummary();
    this.getPointSummary();
    this.getWalletSummary();
    this.loadTotalVoidMembership();
  }

  getCustomerSummary() {
    this.loadTotalNewBasicCustomers();
    this.loadTotalUpgradedCustomers();
    this.loadTotalRenewedCustomers();
    this.loadTotalExtendedCustomers();
    this.loadTotalExpiredCustomers();
    this.loadTotalDowngradedCustomers();
  }

  getPointSummary() {
    this.loadTotalAddPoints();
    this.loadTotalRedeemPoints();
    this.loadTotalVoidAddPoints();
    this.loadTotalVoidRedeemPoints();
  }

  loadTotalVoidAddPoints(pageNumber?: number) {
    this.loadingTotalVoidOfAddPoint = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: VoidOfCreditPointTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.pointTransactionService.getVoidOfCreditValuePointTransaction(request).subscribe(res => {
      this.totalVoidOfAddPoints = res.totalVoidOfCreditValue;
      this.loadingTotalVoidOfAddPoint = false;
      this.loadTotalBalancePointTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-void-add').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalVoidRedeemPoints(pageNumber?: number) {
    this.loadingTotalVoidOfRedeemPoints = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: VoidOfDebitPointTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.pointTransactionService.getVoidOfDebitValuePointTransaction(request).subscribe(res => {
      this.totalVoidOfRedeemPoints = res.totalVoidOfDebitValue;
      this.loadingTotalVoidOfRedeemPoints = false;
      this.loadTotalBalancePointTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-void-redeem').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getWalletSummary() {
    this.loadTotalTopup();
    this.loadTotalSpend();
    this.loadTotalVoidTopup();
    this.loadTotalVoidSpend();
  }

  loadTotalVoidTopup(pageNumber?: number) {
    this.loadingTotalVoidTopup = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: VoidOfCreditWalletTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.walletTransactionService.getVoidOfCreditValueWalletTransaction(request).subscribe(res => {
      this.totalVoidTopup = res.totalVoidOfCreditValue;
      this.loadingTotalVoidTopup = false;
      this.loadTotalBalanceWalletTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-void-topup').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalVoidSpend(pageNumber?: number) {
    this.loadingTotalVoidSpend = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: VoidOfDebitWalletTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.walletTransactionService.getVoidOfDebitValueWalletTransaction(request).subscribe(res => {
      this.totalVoidSpend = res.totalVoidOfDebitValue;
      this.loadingTotalVoidSpend = false;
      this.loadTotalBalanceWalletTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-void-spend').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalUpgradedCustomers(pageNumber?: number) {
    this.loadingTotalUpgradedCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getUpgradedCustomers(request).subscribe(res => {
      this.totalUpgradedCustomer = res.totalItem;
      this.loadingTotalUpgradedCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-new-premium-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalRenewedCustomers(pageNumber?: number) {
    this.loadingTotalRenewedCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getRenewedCustomers(request).subscribe(res => {
      this.totalRenewedCustomer = res.totalItem;
      this.loadingTotalRenewedCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-new-premium-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalExtendedCustomers(pageNumber?: number) {
    this.loadingTotalExtendedCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getExtendedCustomers(request).subscribe(res => {
      this.totalExtendedCustomer = res.totalItem;
      this.loadingTotalExtendedCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-new-premium-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalNewBasicCustomers(pageNumber?: number) {
    this.loadingTotalNewBasicCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getnewCustomers(request).subscribe(res => {
      this.totalNewBasicCustomer = res.totalItem;
      this.loadingTotalNewBasicCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-new-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalExpiredCustomers(pageNumber?: number) {
    this.loadingTotalExpiredCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getExpiredCustomers(request).subscribe(res => {
      this.totalExpiredCustomer = res.totalItem;
      this.loadingTotalExpiredCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-expired-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalDowngradedCustomers(pageNumber?: number) {
    this.loadingTotalDowngradedCustomer = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: FilterCustomerRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.customerService.getDowngradedCustomers(request).subscribe(res => {
      this.totalDowngradedCustomer = res.totalItem;
      this.loadingTotalDowngradedCustomer = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-downgraded-members').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalAddPoints(pageNumber?: number) {
    this.loadingTotalAddPoints = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: CreditPointTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.pointTransactionService.getCreditValuePointTransaction(request).subscribe(res => {
      this.totalAddPoints = res.totalCreditValue;
      this.loadingTotalAddPoints = false;
      this.loadTotalBalancePointTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-add').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalRedeemPoints(pageNumber?: number) {
    this.loadingTotalRedeemPoints = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: DebitPointTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.pointTransactionService.getDebitValuePointTransaction(request).subscribe(res => {
      this.totalRedeemPoints = res.totalDebitValue;
      this.loadingTotalRedeemPoints = false;
      this.loadTotalBalancePointTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-redeem').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalTopup(pageNumber?: number) {
    pageNumber = pageNumber ? pageNumber : 0;
    this.loadingTotalTopup = true;
    const request: CreditWalletTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.walletTransactionService.getCreditValueWalletTransaction(request).subscribe(res => {
      this.totalTopup = res.totalCreditValue;
      this.loadingTotalTopup = false;
      this.loadTotalBalanceWalletTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-topup').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalSpend(pageNumber?: number) {
    pageNumber = pageNumber ? pageNumber : 0;
    this.loadingTotalSpend = true;
    const request: DebitWalletTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.walletTransactionService.getDebitValueWalletTransaction(request).subscribe(res => {
      this.totalSpend = res.totalDebitValue;
      this.loadingTotalSpend = false;
      this.loadTotalBalanceWalletTransaction();
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-spend').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  loadTotalVoidMembership(pageNumber?: number) {
    this.loadingTotalVoidMembership = true;
    pageNumber = pageNumber ? pageNumber : 0;
    const request: VoidMemberTransactionRequest = {
      pageNumber: pageNumber,
      pageSize: this.pageSize,
      fromDateFilter: this.from_date_filter,
      toDateFilter: this.end_date_filter,
      outletId: this.currentOutlet.id
    };
    this.membershipTransactionService.getVoidMembershipTransaction(request).subscribe(res => {
      this.totalVoidMembership = res.totalItem;
      this.loadingTotalVoidMembership = false;
      this.checkIsLoading();
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.cant-get-total-void-membership').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  checkIsLoading() {
    this.isLoading = (this.loadingTotalAddPoints || this.loadingTotalRedeemPoints
      || this.loadingTotalNewBasicCustomer || this.loadingTotalExpiredCustomer
      || this.loadingTotalUpgradedCustomer || this.loadingTotalExtendedCustomer
      || this.loadingTotalRenewedCustomer
      || this.loadingTotalDowngradedCustomer || this.loadingTotalTopup
      || this.loadingTotalSpend || this.loadingUserRoles || this.loadingTotalVoidOfAddPoint || this.loadingTotalVoidOfRedeemPoints
      || this.loadingTotalVoidTopup || this.loadingTotalVoidSpend || this.loadingTotalVoidMembership);
  }

  public loadTotalBalancePointTransaction() {
    this.totalBalancePointTrans = this.totalAddPoints + this.totalVoidOfRedeemPoints - this.totalRedeemPoints - this.totalVoidOfAddPoints;
  }

  public loadTotalBalanceWalletTransaction() {
    const floatNumber =
      this.calculateFloatNumber([this.totalTopup, this.totalVoidSpend, this.totalSpend, this.totalVoidTopup]);
    this.totalWalletTranasctions =
      (this.totalTopup * floatNumber
        + this.totalVoidSpend * floatNumber
        - this.totalSpend * floatNumber
        - this.totalVoidTopup * floatNumber) / floatNumber;
  }

  checkAccessPage() {
    if (!this.currentUser) {
      this.authService.logout();
    }

    if (!this.currentOutlet) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
  }

  getUserRoles() {
    this.loadingUserRoles = true;
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      this.loadingUserRoles = false;
      if (!this.checkPermissionToAccessPage()) {
        this.translate.get('ERROR.do-not-permission').subscribe(message => {
          this.toast.error(message);
        });
        this.authService.logout();
      }
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  checkPermissionToAccessPage() {
    if (this.userRoles.indexOf(RoleName.ROLE_STAFF) === -1 && this.userRoles.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
      return false;
    } else {
      return true;
    }
  }

  checkPermissionCurrentUser(roleName: string) {
    if (this.userRoles.indexOf(roleName) !== -1) {
      return true;
    }
    return false;
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

  onClickTransactionsHistoryButton() {
    this.sharedService.routingToPage(PageName.TRANSACTIONS_HISTORY_PAGE);
  }

  onClickChangePasswordButton() {
    this.sharedService.routingToPage(PageName.CHANGE_PASSWORD_PAGE);
  }

  onClickLogoutButton() {
    this.authService.logout();
  }

  createDateFilter(filterFromDate: any, filterToDate: any) {
    if (!filterFromDate) {
      filterFromDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    }
    if (!filterToDate) {
      filterToDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    }
    this.from_date_filter = new Date(filterFromDate.year, filterFromDate.month - 1, filterFromDate.day, 0, 0, 0).toISOString();
    this.show_from_date_filter = moment.utc(this.from_date_filter).local().format('YYYY-MM-DD');
    this.end_date_filter = new Date(filterToDate.year, filterToDate.month - 1, filterToDate.day, 23, 59, 59).toISOString();
    this.show_end_date_filter = moment.utc(this.end_date_filter).local().format('YYYY-MM-DD');
  }

  onClickRefreshButton() {
    this.createDateFilter(this.filteredFromDate, this.filteredToDate);
    if (this.from_date_filter > this.end_date_filter) {
      this.translate.get('ERROR.from-date-cannot-greater').subscribe(message => {
        this.toast.warning(message);
      });
    } else {
      this.getSummary();
    }
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
