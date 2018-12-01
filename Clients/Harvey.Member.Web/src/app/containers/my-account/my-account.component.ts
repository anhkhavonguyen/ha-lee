import { Component, OnInit, ViewChild, HostListener, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { MembershipType, UserProfileModel } from '../../shared/models/user-profile.model';
import {
  GetBalancePointAndWalletResponse,
  MembershipResponse,
  MyAccountService,
  MembershipTransaction,
  PointTransaction,
  WalletTransaction,
  CustomerTransactionRequest,
  GetExpiryPointsResponse,
  GetExpiryPointsRequest
} from './my-account.service';
import { PhoneUtil } from '../../shared/utils/phone-utils';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
import { AppSettingService } from '../../shared/services/app-setting.service';
import { TranslateService } from '@ngx-translate/core';
import { CommonConstants } from 'src/app/shared/constants/common.constant';

const screeSizeLargeDevices = 1024;
const screenSizeSmallDevices = 568;
const appSettingMemberApp = 3;
const appSettingStoreApp = 2;
@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MyAccountComponent implements OnInit {

  userProfile: UserProfileModel = new UserProfileModel();
  userBalance: GetBalancePointAndWalletResponse = new GetBalancePointAndWalletResponse();
  userMembership: string;
  expiredDate: string;
  membershipType: MembershipType = new MembershipType();
  userPhone = '';
  defaultPicture = '/assets/img/icon/tog_profile_icon.png';
  displayPicture = '';
  isLoading = true;

  @ViewChild('tableTransactionPoint') tableTransactionPoint: any;
  @ViewChild('tableTransactionWallet') tableTransactionWallet: any;
  @ViewChild('tableTransactionMembership') tableTransactionMembership: any;
  rows: any[] = [];
  expanded: any = {};
  public isLargeDevices = false;
  public isSmallDevices = false;

  public totalMembershipTransaction = 0;
  public totalItemPointTransaction = 0;
  public totalWalletTransaction = 0;

  public pageNumberMembership = 0;
  public pageNumberPoint = 0;
  public pageNumberWallet = 0;

  public pageSize = 2;

  public membershipTransactionList: Array<MembershipTransaction> = [];
  public pointTransactionList: Array<PointTransaction> = [];
  public walletTransactionList: Array<WalletTransaction> = [];

  public loadingIndicatorMembershipTransaction = true;
  public loadingIndicatorPointTransaction = true;
  public loadingIndicatorWalletTransaction = true;

  public currentVisible = 3;
  public currentPagePoint = 1;
  public currentPageWallet = 1;
  public isBirthmonth = false;
  public birthday = '';

  public appSettings: any;
  public appSettingsByStoreApp: any;
  public iconPoint = '';
  public defaultIconPoint = '/assets/img/icon/tog_store_points.png';
  public iconWallet = '';
  public defaultIconWallet = '/assets/img/icon/tog_store_wallet.png';
  public checkExistMember = false;

  public isViewPointHistory = false;
  public isViewWalletHistory = false;
  public isViewMembershipHistory = false;

  public getExpiryPointsResponse: GetExpiryPointsResponse;
  public isLoadingExpiryPoint = true;
  public hasExpiryPoint = false;
  public periodTimeDisplayExpiringPoint = 3;

  public membershipBasicButtonColorProperties = {};
  public membershipPremiumButtonColorProperties = {};

  constructor(private router: Router,
    private oauthService: OAuthService,
    private authStorage: OAuthStorage,
    private accountService: MyAccountService,
    private toast: ToastrService,
    private appSettingService: AppSettingService,
    private translate: TranslateService) {
    if (!this.oauthService.hasValidAccessToken()) {
      this.authStorage.removeItem('access_token');
      this.authStorage.removeItem('expires_at');
      this.toast.warning('Your session has timed out. Please sign in again');
      this.router.navigate(['/auth/login']);
    }
  }

  ngOnInit() {
    this.appSettingService.getAppSettingByType(appSettingMemberApp).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettings = res;
        this.periodTimeDisplayExpiringPoint = this.getPeriodTimeDisplayExpiringPoint();
        this.iconPoint = this.appSettings.appSettingModels ?
          `data:image/png;base64,${this.appSettings.appSettingModels.find(a => a.name === 'IconPoint').value}` : this.defaultIconPoint;
        this.iconWallet = this.appSettings.appSettingModels ?
          `data:image/png;base64,${this.appSettings.appSettingModels.find(a => a.name === 'IconWallet').value}` : this.defaultIconWallet;
      } else {
        this.translate.get('ERROR.try-again').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
      });
    });

    this.appSettingService.getAppSettingByType(appSettingStoreApp).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingsByStoreApp = res;

        this.membershipPremiumButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipPremiumButtonColor);
        this.membershipBasicButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipBasicButtonColor);
      } else {
        this.translate.get('ERROR.try-again').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
      });
    });

    this.getUserInfo();
    this.getScreedDisplay();
  }

  getUserInfo() {
    this.isLoading = true;
    this.accountService.getUserProfile().subscribe((response: any) => {
      if (!response) {
        this.checkExistMember = false;
        this.isLoading = false;
        return;
      } else {
        this.checkExistMember = true;
        this.userProfile = response;
        this.userPhone = PhoneUtil.FormatPhoneNumber(this.userProfile.phoneCountryCode, this.userProfile.phoneNumber);
        this.displayPicture = this.userProfile.avatar ? `data:image/jpg;base64,${this.userProfile.avatar}` : this.defaultPicture;
        this.isBirthmonth = this.checkBirthdayMonth(this.userProfile.dateOfBirth);
        this.birthday = this.userProfile.dateOfBirth ? moment.utc(this.userProfile.dateOfBirth).local().format('LL') : null;
        this.getCurrentMembership();
        this.getExpiryPoint();
        this.accountService.getBalancePointAndWallet().subscribe((res: any) => {
          this.userBalance = res;
          this.isLoading = false;
        });
      }
    });
  }


  getCurrentMembership() {
    this.accountService.getCurrentMembership().subscribe((response: MembershipResponse) => {
      if (!response) {
        return;
      }
      this.userMembership = response.membership;
      if (response.expiredDate) {
        this.expiredDate = moment.utc(response.expiredDate).local().format('LL');
      } else {
        this.expiredDate = null;
      }
    });
  }

  checkIsExpired(expiredDate: string, membership: string) {
    const expired = moment.utc(expiredDate).local().format('YYYY/MM/DD');
    const dateNow = moment.utc(new Date()).local().format('YYYY/MM/DD');
    if (membership === this.membershipType.premium && expired < dateNow) {
      return true;
    } else {
      return false;
    }
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

  getTransactionMembershipHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userProfile.id,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };
    this.accountService.getMembershipTransactions(requestHistoryTransaction).subscribe(res => {
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

  getTransactionPointHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userProfile.id,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };

    this.accountService.getPointTransactions(requestHistoryTransaction).subscribe(res => {
      const temp = res;
      this.pageNumberPoint = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemPointTransaction = temp.totalItem;
      this.loadingIndicatorPointTransaction = false;

      this.pointTransactionList = temp.listPointTransaction.map(result => {
        const pointTransactionModel = PointTransaction.buildPointTransaction(result);
        return pointTransactionModel;
      });
    });
  }

  getTransactionWalletHistory(pageInfo: { offset: number; }) {
    const requestHistoryTransaction: CustomerTransactionRequest = {
      customerId: this.userProfile.id,
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };

    this.accountService.getWalletTransactions(requestHistoryTransaction).subscribe(res => {
      const temp = res;
      this.pageNumberPoint = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalWalletTransaction = temp.totalItem;
      this.loadingIndicatorWalletTransaction = false;

      this.walletTransactionList = temp.listWalletTransaction.map(result => {
        const walletTransactionModel = WalletTransaction.buildWalletTransaction(result);
        return walletTransactionModel;
      });
    });
  }

  isExtraLargeDevices() {
    if (!this.isLargeDevices && !this.isSmallDevices) {
      return true;
    } else {
      return false;
    }
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

  toggleExpandPointHistory() {
    this.isViewPointHistory = !this.isViewPointHistory;
    if (this.isViewPointHistory) {
      this.getTransactionPointHistory({ offset: 0 });
    }
  }

  toggleExpandWalletHistory() {
    this.isViewWalletHistory = !this.isViewWalletHistory;
    if (this.isViewWalletHistory) {
      this.getTransactionWalletHistory({ offset: 0 });
    }
  }

  toggleExpandMembershipHistory() {
    this.isViewMembershipHistory = !this.isViewMembershipHistory;
    if (this.isViewMembershipHistory) {
      this.getTransactionMembershipHistory({ offset: 0 });
    }
  }

  onDetailToggle(event) {
  }

  isNewMember() {
    return this.expiredDate === '-' ? true : false;
  }

  editProfile() {
    this.router.navigate(['/app/edit-profile']);
  }

  changePhoneNumber() {
    this.router.navigate(['/app/change-phone']);
  }

  logOut() {
    this.isLoading = true;
    this.authStorage.removeItem('access_token');
    this.authStorage.removeItem('expires_at');
    this.router.navigate(['/auth/login']);
  }

  checkBirthdayMonth(birthday: any) {
    if (birthday === null) {
      return false;
    } else if (moment.utc(birthday).local().month() !== moment(new Date()).month()) {
      return false;
    }
    return true;
  }

  getExpiryPoint() {
    this.isLoadingExpiryPoint = true;
    this.hasExpiryPoint = false;
    if (this.userProfile) {
      const now = moment.utc(Date.now()).local().format('YYYY/MM/DD');
      const tempNow = new Date(now);
      const toDate = moment.utc(tempNow.setMonth(tempNow.getMonth() + this.periodTimeDisplayExpiringPoint)).local().format('YYYY/MM/DD');
      const request: GetExpiryPointsRequest = {
        customerId: this.userProfile.id,
        fromDate: now,
        toDate: toDate
      };
      this.accountService.getExpiryPoints(request).subscribe(res => {
        this.getExpiryPointsResponse = res;
        if (this.getExpiryPointsResponse.expiryPoints) {
          this.getExpiryPointsResponse.expiryPoints.reverse();
        }
        if (this.getExpiryPointsResponse.totalExpirypoint > 0) {
          this.hasExpiryPoint = true;
        }
        this.isLoadingExpiryPoint = false;
      });
    }
  }

  getPeriodTimeDisplayExpiringPoint() {
    const periodTime = this.appSettings.appSettingModels.find(a => a.name === 'PeriodTimeToNotifyExpiringPoint');
    return periodTime && periodTime.value ? Number(periodTime.value) : 3;
  }

  private getMembershipButtonColor(appSettingName: string) {
    if (this.appSettingsByStoreApp == null) {
      return '';
    }

    const appSettingColor = this.appSettingsByStoreApp.appSettingModels.find(x => (x.name === appSettingName));
    return (appSettingColor && appSettingColor.value) ? JSON.parse(appSettingColor.value) : '';
  }
}
