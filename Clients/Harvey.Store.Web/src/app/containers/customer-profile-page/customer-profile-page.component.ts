import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../shared/services/shared.service';
import { Outlet } from '../../shared/models/outlet.model';
import { User } from '../../shared/models/user.model';
import { CustomerService } from '../../shared/services/customer.service';
import {
  Customer,
  ResendResetPasswordRequest,
  ResendSignUpRequest,
  GetBasicCustomerInfoRequest,
  Status
} from '../../shared/models/customer.model';
import { PageName } from '../../shared/constants/routing.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { AuthService } from '../../auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonConstants, AppSettingLabelByContentTypeConstants } from '../../shared/constants/common.constant';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { EditMembershipComponent } from './components/edit-membership/edit-membership.component';
import { UserService } from '../../shared/services/user.service';
import { environment } from '../../../environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { InfoDialogComponent } from '../../shared/components/info-dialog/info-dialog.component';
import { AppSettingService } from '../../shared/services/app-setting.service';
import { ChangePhoneDialogComponent } from '../../shared/components/change-phone-dialog/change-phone-dialog.component';
import { ValidatePhoneModel } from '../../shared/models/validate-phone.model';
import { GetExpiryPointsResponse, GetExpiryPointsRequest } from '../../shared/models/pointTransaction.model';
import { PointTransactionService } from '../../shared/services/point-transactions.service';
import { QrCodeComponent } from './components/qr-code/qr-code.component';
import { GetAppsettingResponse, AppSettingModel } from '../../shared/models/app-setting.model';


const typeBalance = {
  point: 'Point',
  wallet: 'Wallet',
};

const membershipTypeFE = {
  basic: {
    typeName: 'Basic',
    displayName: 'Basic',
    typeClass: '',
    typeCode: '1'
  },
  premiumPlus: {
    typeName: 'Premium+',
    displayName: 'Premium+',
    typeClass: 'premium-plus-member',
    typeCode: '2'
  }
};

@Component({
  selector: 'app-customer-profile-page',
  templateUrl: './customer-profile-page.component.html',
  styleUrls: ['./customer-profile-page.component.scss']
})
export class CustomerProfilePageComponent implements OnInit {

  point = typeBalance.point;
  wallet = typeBalance.wallet;

  public currentOutlet = new Outlet();
  public currentUser = new User();
  public currentCustomer = new Customer();

  public birthday = '';
  public expiredDate = '';
  public fullnNameCustomer = '';
  public lastUsed = '';
  public membershipType = membershipTypeFE.basic;
  public rewardPointBalance = 0;
  public walletBalance = 0;

  public isLoading = true;
  public countRefresh = 0;
  public countryCode = '';
  public phoneNumber = '';

  public validRedeemPointSubcription: Subscription;
  public redeemBalanceSubscription: Subscription;
  public addBalanceSubscription: Subscription;
  public spendWalletBalanceSubscription: Subscription;
  public isValidRedeem = true;

  public topUpWalletSubscription: Subscription;

  public isRefresh = false;
  public refreshCutomerInfoSubcription: Subscription;

  public messageLoading = '';
  public isWaittingPoint = true;
  public isWaittingWallet = true;

  public userId = '';
  public userToken = '';

  public imageCustomer = '';
  public defaultCustomerPicture = '/assets/img/tog_profile_icon.png';

  private userRoles = [];

  public membershipEditSubcription: Subscription;
  public editCommentSubcription: Subscription;

  public isConfirmedSignUp = false;
  public isResetPassword = false;
  public isLoadingTransparent = false;

  public isRetry = false;
  public hasPreminumMembershipTransaction = false;
  public isBirthmonth = false;

  public membershipButtonColorProperties = {};

  public appSettings: GetAppsettingResponse;

  public ChangePhoneFeature = false;
  public outletAvatar: any;

  public isLoadExpiringPoint = true;
  public periodTimeDisplayExpiringPoint;
  public getExpiryPointsResponse: GetExpiryPointsResponse;

  public panelColorProperties = {};

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private customerService: CustomerService,
    private oAuthStorage: OAuthStorage,
    private modalService: NgbModal,
    private activatedRoute: ActivatedRoute,
    private toast: ToastrService,
    private userService: UserService,
    private translate: TranslateService,
    private appSettingService: AppSettingService,
    private pointTransactionService: PointTransactionService) {
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.initPage();
    }
  }

  initPage() {
    this.appSettings = this.sharedService.getAppSettings();
    if (!this.appSettings) {
      this.isLoading = true;
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning(message);
      });
    } else {
      this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
      this.periodTimeDisplayExpiringPoint = this.getPeriodTimeDisplayExpiringPoint();
      this.userToken = this.authService.getTokenFormStorage();
      this.currentUser = this.sharedService.getAnnounceCurrentUser();
      this.currentOutlet = this.sharedService.getAnnounceOutlet();
      this.ChangePhoneFeature = this.appSettings.appSettingModels.find(a => a.name === 'ChangePhoneNumber').value ?
        (this.appSettings.appSettingModels.find(a => a.name === 'ChangePhoneNumber').value === 'true' ? true : false) : false;
      this.activatedRoute.queryParams.subscribe(param => {
        this.userId = param.customerId;
        this.getCustomerInfo();
        this.getExpiryPoint();
      });

      this.getUserRoles();

      this.redeemBalanceSubscription = this.sharedService.redeemBalanceSubscription.subscribe(data => {
        if (data >= 0) {
          this.currentCustomer.totalStranstion += 1;
          this.getExpiryPoint();
          this.getPointBalance();
        }
      });

      this.addBalanceSubscription = this.sharedService.addBalanceSubscription.subscribe(data => {
        if (data) {
          this.currentCustomer.totalStranstion += 1;
          this.getExpiryPoint();
          this.getPointBalance();
        }
      });

      this.topUpWalletSubscription = this.sharedService.topUpWalletBalanceSubcription.subscribe(data => {
        this.currentCustomer.totalStranstion += 1;
        this.getWalletBalance();
      });

      this.spendWalletBalanceSubscription = this.sharedService.spendWalletBalanceSubcription.subscribe(data => {
        this.currentCustomer.totalStranstion += 1;
        this.getWalletBalance();
      });

      this.refreshCutomerInfoSubcription = this.sharedService.refreshCustomerInfoSubscription.subscribe(data => {
        if (data) {
          this.getCustomerInfo();
        }
      });
    }

  }

  ngOnInit() {
    this.checkAccessPage();
    this.outletAvatar = this.getOutletAvatarImage();
  }

  viewMembershipHistory() {
    const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true });
    const instance = dialogRef.componentInstance;
    instance.header = 'Membership transaction history';
    instance.isViewHistory = true;
    instance.typeHistory = 'Membership';
    instance.userId = this.currentCustomer.id;
  }


  checkAccessPage() {
    if (!this.currentUser) {
      this.authService.logout();
    }
    if (!this.currentCustomer) {
      this.sharedService.routingToPage(PageName.SERVING_PAGE);
    }
    if (!this.currentOutlet) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
  }

  showModalMessageExpired() {
    if (this.isExpired()) {
      this.translate.get('MESSAGE.message-expired').subscribe(message => {
        const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true });
        const instance = dialogRef.componentInstance;
        instance.header = 'Message';
        instance.content = message;
      });
    }
  }

  getCustomerInfo() {
    this.customerService.getByCustomerId(this.userToken, this.userId).subscribe(customerInfo => {
      const userId = customerInfo.id;
      if (userId && customerInfo.status === Status.active) {
        this.currentCustomer = customerInfo;
        this.showModalMessageExpired();
        this.getCustomerInfoToShow();
        this.getExpiryPoint();
        this.checkConfirmedSignUp();
      } else {
        this.isLoading = true;
        this.isRetry = true;
        this.translate.get('ERROR.customer-not-found').subscribe(message => {
          this.messageLoading = message;
        });
      }
    }, () => {
      this.isLoading = true;
      this.isRetry = true;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.messageLoading = message;
      });
    });
  }

  getCustomerInfoToShow() {
    this.imageCustomer = this.currentCustomer.profileImage ?
      `data:image/jpg;base64,${this.currentCustomer.profileImage}` : this.defaultCustomerPicture;
    this.fullnNameCustomer = this.getFullNameCustomer();
    this.expiredDate = this.currentCustomer.expiredDate ? moment.utc(this.currentCustomer.expiredDate).local().format('DD/MM/YYYY') : null;
    this.hasPreminumMembershipTransaction = this.currentCustomer.hasPreminumMembershipTransaction;
    this.isBirthmonth = this.checkBirthdayMonth(this.currentCustomer.dateOfBirth);
    this.currentCustomer.dateOfBirth = this.currentCustomer.dateOfBirth
      ? moment.utc(this.currentCustomer.dateOfBirth).local().format('MMMM') : null;
    this.currentCustomer.lastUsed =
      this.currentCustomer.lastUsed ? moment.utc(this.currentCustomer.lastUsed).local().format('DD/MM/YYYY') : null;
    this.getMembershipTypeToShow(this.currentCustomer.membership);
    this.getBalance();
  }

  getFullNameCustomer() {
    const firstName = this.currentCustomer.firstName ? this.currentCustomer.firstName : '';
    const lastName = this.currentCustomer.lastName ? this.currentCustomer.lastName : '';
    const fullName = firstName || lastName ? firstName + ' ' + lastName : '';
    return fullName;
  }

  getBalance() {
    this.isWaittingPoint = this.isWaittingWallet = true;
    this.customerService.getBalanceRewardPoint(this.userToken, this.currentCustomer.id).subscribe(pointBalance => {
      this.rewardPointBalance = pointBalance;
      this.isWaittingPoint = false;
      this.customerService.getBalanceWallet(this.userToken, this.currentCustomer.id).subscribe(walletBalance => {
        this.walletBalance = walletBalance;
        this.isWaittingWallet = this.isLoading = false;
      }, error => {
        this.isWaittingWallet = false;
        this.translate.get('ERROR.cant-get-wallet-balance').subscribe(message => {
          this.toast.error(message);
        });
      });
    }, error => {
      this.isWaittingPoint = this.isWaittingWallet = false;
      this.translate.get('ERROR.cant-get-point-balance').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getEmitNewPointBalance(pointBalance: number) {
    this.rewardPointBalance = pointBalance;
  }

  getEmitNewWalletBalance(walletBalance: number) {
    this.walletBalance = walletBalance;
  }

  getPointBalance() {
    this.isWaittingPoint = true;
    this.customerService.getBalanceRewardPoint(this.userToken, this.currentCustomer.id).subscribe(pointBalance => {
      this.rewardPointBalance = pointBalance;
      this.isWaittingPoint = false;
    }, error => {
      this.isWaittingPoint = false;
      this.translate.get('ERROR.cant-get-wallet-balance').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getWalletBalance() {
    this.isWaittingWallet = true;
    this.customerService.getBalanceWallet(this.userToken, this.currentCustomer.id).subscribe(walletBalance => {
      this.isWaittingWallet = false;
      this.walletBalance = walletBalance;
    }, error => {
      this.isWaittingWallet = false;
      this.translate.get('ERROR.cant-get-wallet-balance').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getMembershipTypeToShow(membershipName: string) {
    switch (membershipName) {
      case membershipTypeFE.basic.typeName:
        this.membershipType = membershipTypeFE.basic;
        this.membershipButtonColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storeMembershipBasicButtonColor);
        break;
      case membershipTypeFE.premiumPlus.typeName:
        this.membershipType = membershipTypeFE.premiumPlus;
        this.membershipButtonColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storeMembershipPremiumButtonColor);
        break;
      default: this.membershipType = membershipTypeFE.basic;
        break;
    }
  }

  onClickDoneButton() {
    this.oAuthStorage.removeItem(CommonConstants.currentCustomer);
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickEditButton() {
    if (!this.appSettings) {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning(message);
      });
    } else {
      const modalRef = this.modalService.open(EditMembershipComponent, { size: 'lg', centered: true, backdrop: 'static' });
      const instance = modalRef.componentInstance;
      instance.currentCustomer = this.currentCustomer;
      instance.currentUser = this.currentUser;
      instance.currentOutlet = this.currentOutlet;
      instance.userRoles = this.userRoles;
      instance.walletBalance = this.walletBalance;

      this.membershipEditSubcription = this.sharedService.membershipEditSubcription.subscribe(data => {
        const failed = '-1';
        const newMembership = data.membershipName;
        const newDate = data.expired;
        const newExpired = newDate ? moment.utc(newDate).local().format('DD/MM/YYYY') : null;
        if (data.membershipName !== failed) {
          this.currentCustomer.membership = newMembership;
          this.getMembershipTypeToShow(newMembership);
          this.currentCustomer.expiredDate = newDate;
          this.expiredDate = newExpired;
          this.hasPreminumMembershipTransaction = true;
          this.currentCustomer.totalStranstion = data.totalTransaction;
          this.currentCustomer.commentMembership = data.comment;
          if (this.currentCustomer.membership === membershipTypeFE.premiumPlus.typeName) {
            this.currentCustomer.hasPreminumMembershipTransaction = true;
          }
        }
        modalRef.close();
      });

      this.editCommentSubcription = this.sharedService.editCommentSubcription.subscribe(res => {
        this.currentCustomer.totalStranstion = res.totalTransaction;
        this.currentCustomer.commentMembership = res.comment;
        modalRef.close();
      });
    }
  }

  onClickRetryButton() {
    this.isRetry = false;
    setTimeout(() => {
      this.countRefresh = 0;
      this.messageLoading = '';
      this.isLoading = true;
    }, 200);
    setTimeout(() => {
      this.getCustomerInfo();
    }, 1600);
  }

  private getUserRoles() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
    }, error => {
      this.isRetry = true;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
        this.authService.logout();
      });
    });
  }

  checkConfirmedSignUp() {
    const request: GetBasicCustomerInfoRequest = {
      countryCode: this.currentCustomer.phoneCountryCode,
      phoneNumber: this.currentCustomer.phone
    };
    this.customerService.getBasicCustomerInfo(request).subscribe(res => {
      if (res && res.emailConfirmed && res.phoneNumberConfirmed) {
        this.isConfirmedSignUp = true;
        this.isResetPassword = false;
      } else {
        this.isConfirmedSignUp = false;
        this.isResetPassword = true;
      }
    });
  }

  onClickResendSignUpLink() {
    const request: ResendSignUpRequest = {
      countryCode: this.currentCustomer.phoneCountryCode,
      phoneNumber: this.currentCustomer.phone,
      outletName: this.currentOutlet.name,
      originalUrl: environment.memberPageUrl,
      userId: this.currentCustomer.id
    };
    this.isLoadingTransparent = true;
    this.customerService.resendSignUpLink(request).subscribe(res => {
      this.isLoadingTransparent = false;
      this.isConfirmedSignUp = true;
      this.translate.get('MESSAGE.resend-sign-up-link-success').subscribe(message => {
        this.toast.success(message);
      });
    }, () => {
      this.isLoadingTransparent = false;
      this.translate.get('ERROR.resend-sign-up-link-failed').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  isExpired() {
    const expired = moment.utc(this.currentCustomer.expiredDate).local().format('YYYY/MM/DD');
    const now = moment.utc(new Date()).local().format('YYYY/MM/DD');
    if (expired < now && this.currentCustomer.hasPreminumMembershipTransaction
      && this.currentCustomer.membership === membershipTypeFE.premiumPlus.typeName) {
      return true;
    }
    return false;
  }

  isAllowRedeem() {
    const expired = this.isExpired();
    if (expired || this.currentCustomer.membership === membershipTypeFE.basic.typeName) {
      return false;
    } else {
      return true;
    }
  }

  isShowExpired() {
    if (!this.currentCustomer.hasPreminumMembershipTransaction ||
      this.currentCustomer.membership === membershipTypeFE.basic.typeName) { return false; }
    return true;
  }

  onClickResendResetPassword() {
    const request: ResendResetPasswordRequest = {
      userName: this.currentCustomer.phoneCountryCode + this.currentCustomer.phone,
      originalUrl: environment.memberPageUrl,
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue),
      outletName: this.currentOutlet.name
    };
    this.isLoadingTransparent = true;
    this.customerService.resendResetPasswordLink(request).subscribe(res => {
      this.isLoadingTransparent = false;
      this.isResetPassword = true;
      this.translate.get('MESSAGE.resend-reset-password-link-success').subscribe(message => {
        this.toast.success(message);
      });
    }, () => {
      this.isLoadingTransparent = false;
      this.translate.get('ERROR.resend-reset-password-link-failed').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  checkBirthdayMonth(birthday: any) {
    if (birthday === null) {
      return false;
    } else if (moment.utc(birthday).local().month() !== moment(new Date()).month()) {
      return false;
    }
    return true;
  }

  onClickChangePhoneButton() {
    const idTypeValidatePhoneAppSetting = 4;
    let validatePhones: Array<ValidatePhoneModel> = [];

    this.appSettingService.getAppSettingsByType(idTypeValidatePhoneAppSetting).subscribe(result => {
      validatePhones = this.sharedService.getValidatePhone(result);
      if (validatePhones.length !== 0) {
        const modalRef = this.modalService.open(ChangePhoneDialogComponent, { size: 'lg', centered: true, backdrop: 'static' });
        const instance = modalRef.componentInstance;
        instance.validatePhones = validatePhones;
        instance.currentCustomer = this.currentCustomer;
        instance.currentUser = this.currentUser;
        instance.clickSaveEvent.subscribe(request => {
          if (request) {
            this.customerService.changePhoneNumber(request).subscribe(res => {
              if (res) {
                const duplicatePhoneNumber = 'duplicate_phone_number';
                if (res === duplicatePhoneNumber) {
                  this.translate.get('ERROR.duplicate_phone_number').subscribe(message => {
                    this.toast.error(message);
                  });
                } else {
                  this.translate.get('MESSAGE.change-phone-success').subscribe(message => {
                    this.toast.success(message);
                    this.customerService.getByCustomerId(this.userToken, this.userId).subscribe(customerInfo => {
                      const userId = customerInfo.id;
                      if (userId && customerInfo.status === Status.active) {
                        this.currentCustomer = customerInfo;
                        this.getCustomerInfoToShow();
                        this.checkConfirmedSignUp();
                      } else {
                        this.isLoading = true;
                        this.isRetry = true;
                        this.translate.get('ERROR.customer-not-found').subscribe(error => {
                          this.messageLoading = message;
                        });
                      }
                    }, () => {
                      this.isLoading = true;
                      this.isRetry = true;
                      this.translate.get('ERROR.something-wrong').subscribe(error => {
                        this.messageLoading = message;
                      });
                    });
                  });
                }
              } else {
                this.translate.get('ERROR.something-wrong').subscribe(message => {
                  this.toast.success(message);
                });
              }
            });
          }
        }, error => {
          this.translate.get('ERROR.something-wrong').subscribe(message => {
            this.toast.success(message);
          });
        });
      }
    }, error => {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getExpiryPoint() {
    this.isLoadExpiringPoint = true;
    if (this.currentCustomer) {
      const now = moment.utc(Date.now()).local().format('YYYY/MM/DD');
      const tempNow = new Date(now);
      tempNow.setMonth(tempNow.getMonth() + this.periodTimeDisplayExpiringPoint);
      const toDate = moment.utc(tempNow).local().format('YYYY/MM/DD');
      const request: GetExpiryPointsRequest = {
        customerId: this.currentCustomer.id,
        fromDate: now,
        toDate: toDate
      };
      this.pointTransactionService.getExpiryPoints(request).subscribe(res => {
        this.getExpiryPointsResponse = res;
        if (this.getExpiryPointsResponse.expiryPoints) {
          this.getExpiryPointsResponse.expiryPoints.reverse();
        }
        this.isLoadExpiringPoint = false;
      });
    }
  }

  getPeriodTimeDisplayExpiringPoint() {
    const periodTime = this.appSettings.appSettingModels.find(a => a.name === 'PeriodTimeToNotifyExpiringPoint');
    return periodTime && periodTime.value ? Number(periodTime.value) : 3;
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

  onClickViewQRCode() {
    const modal = this.modalService.open(QrCodeComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = modal.componentInstance;
    instance.imageCustomer = this.imageCustomer;
    instance.customerCode = this.currentCustomer.customerCode;
  }

  private getMembershipButtonColor(appSettingName: string) {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    if (this.appSettings == null) {
      return '';
    }

    const appSettingColor = this.appSettings.appSettingModels.find(x => (x.name === appSettingName));
    return (appSettingColor && appSettingColor.value) ? JSON.parse(appSettingColor.value) : '';
  }
}

