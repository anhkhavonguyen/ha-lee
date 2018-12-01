import { Component, OnInit, OnDestroy } from '@angular/core';
import { CustomerDetailService } from 'src/app/containers/customer/customer-detail/customer-detail.service';
import { Customer, MembershipActionType } from 'src/app/containers/customer/customer.model';
import { CustomerService } from 'src/app/containers/customer/customer.service';
import {
  CustomerInfoRequest,
  CustomerTransactionRequest,
  MembershipTransaction, PointTransaction,
  WalletTransaction,
  VoidPointRequest,
  VoidWalletRequest,
  MembershipType,
  VoidMembershipRequest,
  Status,
  TypeConfirm,
  ChangeStatusCustomerRequest,
  AddBlankMembershipTransactionRequest,
  SubtractWalletRequest
} from 'src/app/containers/customer/customer-detail/customer-detail.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewEncapsulation } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { User } from '../../../shared/models/user.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { InfoDialogComponent } from 'src/app/shared/components/info-dialog/info-dialog.component';
import * as moment from 'moment';
import { now } from 'moment';
import { EditCustomerInfoComponent } from '../edit-customer-info/edit-customer-info.component';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { ValidatePhoneModel } from 'src/app/containers/customer/edit-customer-info/validate-phone.model';
import { Activity, GetHistoryChangeNumberCustomerRequest, GetHistoryCustomerActivitiesRequest } from '../../activity/activity.model';
import { ActivityService } from '../../activity/activity.service';
import { ReactiveCustomerComponent } from 'src/app/containers/customer/reactive-customer/reactive-customer.component';
import { Subscription } from 'rxjs';
import { AppSettingLabelConstants, CommonConstants } from 'src/app/shared/constants/common.constant';

const idTypeRoleAppsetting = 4;
const ValidatePhoneId = 4;

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerDetailComponent implements OnInit, OnDestroy {
  constructor(
    private customerDetailService: CustomerDetailService,
    private customerService: CustomerService,
    private userService: UserService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private appSettingService: AppSettingsService,
    private toast: ToastrService,
    private translate: TranslateService,
    private activityService: ActivityService,
    private router: Router,
    private appSettingsService: AppSettingsService) {
    this.route
      .params
      .subscribe(params => {
        this.customerId = params['customerId'];
        this.customerCode = params['customerCode'];
      });

    this.appSummaryLabelTitle = this.getSummaryLabelTitle(this.appSettingsService.appSettingsDataByAdminType);
    this.appSettingsDataSubcription = this.appSettingsService.appSettingsDataByAdminTypeSubject.subscribe(data => {
      this.appSummaryLabelTitle = this.getSummaryLabelTitle(data);
    });
  }

  public totalMembershipTransaction = 0;
  public totalItemPointTransaction = 0;
  public totalWalletTransaction = 0;
  public totalHistoryCustomerActivities = 0;

  public customer = new Customer();
  public customerId = '';
  public customerAge = 0;
  public pageNumberMembership = 0;
  public pageNumberPoint = 0;
  public pageNumberWallet = 0;
  public pageNumberHistoryChangeMobile = 0;
  public pageSize = 10;
  public membershipTransactionList: Array<MembershipTransaction> = [];
  public pointTransactionList: Array<PointTransaction> = [];
  public walletTransactionList: Array<WalletTransaction> = [];
  public historyCustomerActivities: Array<Activity> = [];

  public loadingIndicatorMembershipTransaction = true;
  public loadingIndicatorPointTransaction = true;
  public loadingIndicatorWalletTransaction = true;
  public loadingIndicatorHistoryCustomerActivities = true;
  public ipAddress = '';
  public profileImage = '/assets/img/default-avatar.jpg';
  user: User = new User();
  public isExpiredCustomer = false;
  public isPremiumCustomer = false;
  public isBirthmonth = false;
  public membershipType = '';
  public validatePhones: Array<ValidatePhoneModel> = [];
  public customerCode = '';
  isActive = true;
  statusChange = '';
  migrationOutletId = '10ecc00f-6e24-49bc-b735-c18127433f8b';
  migrationStaffId = '76af5bea-6af1-4a18-b38e-1396875518e5';
  defaultSubtractWalletValue = 10;

  public appSettingsDataSubcription = new Subscription();
  public appSummaryLabelTitle = '';

  public appSettingModelsByStoreType: any[] = [];
  public appSettingByStoreTypeSubscription = new Subscription();
  public membershipBasicButtonColorProperties = {};
  public membershipPremiumButtonColorProperties = {};

  ngOnInit() {
    window.scrollTo(0, 0);
    this.loadCustomerInfo(this.customerId);
    this.setPageMembershipTransaction({ offset: 0 });
    this.setPagePointTransaction({ offset: 0 });
    this.setPageWalletTransaction({ offset: 0 });
    this.setPageHistoryCustomerActivities({ offset: 0 });
    this.getIpAddress();
    this.userService.getUserProfile().subscribe(res => {
      this.user = res;
      this.user.fullName = res.firstName + ' ' + res.lastName;
    });
    this.getListValidatePhone();

    this.appSettingModelsByStoreType = this.appSettingService.appSettingModelsByStoreType;
    this.setMembershipButtonColor();
    this.appSettingByStoreTypeSubscription = this.appSettingService.appSettingsDataByStoreTypeSubject.subscribe(res => {
      this.appSettingModelsByStoreType = res;
      this.setMembershipButtonColor();
    });
  }

  ngOnDestroy(): void {
    this.appSettingByStoreTypeSubscription.unsubscribe();
  }

  public loadCustomerInfo(customerId: string) {
    this.customerDetailService.getCustomerInfo(new CustomerInfoRequest({
      customerId: customerId
    })).subscribe(response => {
      this.isPremiumCustomer = this.checkPremiumCustomer(response.membership);
      this.isExpiredCustomer = this.checkExpiredCustomer(response.expiredDate);
      this.membershipType = response.membership;
      this.isBirthmonth = this.checkBirthdayMonth(response.dateOfBirth);
      this.statusChange = +response.status === Status.Activate ? Status[Status.Deactivate] : Status[Status.Activate];
      this.isActive = +response.status === Status.Activate ? true : false;
      this.customer = Customer.buildCustomer(response);
      this.profileImage = this.customer.profileImage !== '' ? `data:image/jpg;base64,${this.customer.profileImage}` : this.profileImage;
    });
  }

  public loadMembershipTransaction(request: CustomerTransactionRequest) {
    this.customerDetailService.getMembershipTransactions(request).subscribe(res => {
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

  public loadPointTransaction(request: CustomerTransactionRequest) {
    this.customerDetailService.getPointTransactions(request).subscribe(res => {
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

  public loadWalletTransaction(request: CustomerTransactionRequest) {
    this.customerDetailService.getWalletTransactions(request).subscribe(res => {
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


  public loadHistoryCustomerActivities(request: GetHistoryCustomerActivitiesRequest) {
    this.activityService.getHistoryCustomerActivities(request).subscribe(res => {
      const temp = res;
      this.pageNumberHistoryChangeMobile = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalHistoryCustomerActivities = temp.totalItem;
      this.loadingIndicatorHistoryCustomerActivities = false;

      this.historyCustomerActivities = temp.actionModels.map(result => {
        const data = Activity.buildActivity(result);
        return data;
      });
    });
  }


  setPageMembershipTransaction(pageInfo: { offset: number; }) {
    const request: CustomerTransactionRequest = {
      customerId: this.customerId,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadMembershipTransaction(request);
  }

  setPagePointTransaction(pageInfo: { offset: number; }) {
    const request: CustomerTransactionRequest = {
      customerId: this.customerId,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadPointTransaction(request);
  }

  setPageWalletTransaction(pageInfo: { offset: number; }) {
    const request: CustomerTransactionRequest = {
      customerId: this.customerId,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadWalletTransaction(request);
  }


  setPageHistoryCustomerActivities(pageInfo: { offset: number; }) {
    const request: GetHistoryCustomerActivitiesRequest = {
      customerCode: this.customerCode,
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.loadHistoryCustomerActivities(request);
  }

  onClickVoidPoint(item: PointTransaction) {
    const request: VoidPointRequest = {
      userId: this.user.id,
      ipAddress: this.ipAddress,
      pointTransactionId: item.id,
      voidByName: this.user ? this.user.fullName : ''
    };

    const dialogRef = this.modalService.open(ConfirmDialogComponent, { centered: true });
    const instance = dialogRef.componentInstance;
    instance.typeConfirm = TypeConfirm.VoidPoint;
    instance.content = 'Are you sure you want to void?';
    instance.request = request;
    instance.title = 'Void Point Tranasction';

    return dialogRef.result.then((result) => {
      if (result !== -1) {
        item.voided = true;
        this.setPagePointTransaction({ offset: 0 });
      }
    }, (reason) => {
    });
  }

  onClickVoidWallet(walletTransaction: WalletTransaction) {
    const request: VoidWalletRequest = {
      userId: this.user.id,
      ipAddress: this.ipAddress,
      walletTransactionId: walletTransaction.id,
      voidByName: this.user ? this.user.fullName : ''
    };

    const dialogRef = this.modalService.open(ConfirmDialogComponent, { centered: true });
    const instance = dialogRef.componentInstance;
    instance.typeConfirm = TypeConfirm.VoidWallet;
    instance.request = request;
    instance.content = 'Are you sure you want to void?';
    instance.title = 'Void Wallet Transaction';

    return dialogRef.result.then((result) => {
      if (result !== -1) {
        walletTransaction.voided = true;
        this.setPageWalletTransaction({ offset: 0 });
      }
    }, (reason) => {
    });
  }

  onClickVoidMembership(membershipTransaction: MembershipTransaction) {
    const request: VoidMembershipRequest = {
      userId: this.user.id,
      ipAddress: this.ipAddress,
      membershipTransactionId: membershipTransaction.id,
      voidByName: this.user ? this.user.fullName : '',
      membershipActionType: MembershipActionType.Void
    };

    const dialogRef = this.modalService.open(ConfirmDialogComponent, { centered: true });
    const instance = dialogRef.componentInstance;
    instance.typeConfirm = TypeConfirm.VoidMembership;
    instance.request = request;
    instance.content = 'Are you sure you want to void?';
    instance.title = 'Void Membership Transaction';

    return dialogRef.result.then((result: any) => {
      if (result) {
        this.translate.get('APP.CUSTOMER_DETAIL_COMPONENT.VOID_MEMBERSHIP_TRANSACTION_SUCCESS').subscribe(message => {
          this.toast.success(message);
        });
        membershipTransaction.voided = true;
        this.setPageMembershipTransaction({ offset: 0 });
        this.loadCustomerInfo(this.customerId);
      }
    }, (reason) => {
    });
  }

  getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then((ip: string) => {
      this.ipAddress = ip;
    });
  }

  onClickShowComment(comment: any) {
    const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.header = 'Comment';
    instance.content = comment;
  }

  checkExpiredCustomer(expiredDate: any) {
    if (expiredDate === null) {
      return false;
    } else {
      const expired = moment.utc(expiredDate).local().format('YYYY/MM/DD');
      const nowTime = moment.utc(new Date()).local().format('YYYY/MM/DD');
      if (expired < nowTime) {
        return true;
      }
    }
    return false;
  }

  checkPremiumCustomer(_membershipType: string) {
    return _membershipType === MembershipType.premium ? true : false;
  }

  checkBirthdayMonth(birthday: any) {
    if (birthday === null) {
      return false;
    } else if (moment.utc(birthday).local().month() !== moment(new Date()).month()) {
      return false;
    }
    return true;
  }

  getListValidatePhone() {
    this.appSettingService.getAppSettingsByType(idTypeRoleAppsetting).subscribe(result => {
      if (result && result.appSettingModels && result.appSettingModels.length) {
        this.validatePhones = this.getValidatePhone(result);
      } else {
        this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe((message: any) => {
        this.toast.error(message);
      });
    });
  }

  public getValidatePhone(phoneValidates: any) {
    const listValidate: Array<ValidatePhoneModel> = [];
    phoneValidates.appSettingModels
      .forEach((item: any) => {
        if (item.appSettingTypeId === ValidatePhoneId) {
          const validate: ValidatePhoneModel = {
            countryCode: item.name,
            name: item.value.substr(0, 2),
            regex: item.value.substr(3)
          };
          listValidate.push(validate);
        }
      });
    return listValidate;
  }

  editCustomerPhone() {
    const duplicatePhoneNumber = 'duplicate_phone_number';
    const dialogRef = this.modalService.open(EditCustomerInfoComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.validatePhones = this.validatePhones;
    instance.customer = this.customer;
    instance.clickSaveEvent.subscribe((request: any) => {
      if (request) {
        this.customerService.UpdateCustomerProfile(request).subscribe(result => {
          if (result) {
            if (result === duplicatePhoneNumber) {
              this.translate.get('APP.ERROR.DUPLICATE_PHONE_NUMBER').subscribe((res: any) => {
                this.toast.error(res);
              });
            } else {
              this.translate.get('APP.CUSTOMER_DETAIL_COMPONENT.UPDATE_CUSTOMER_PROFILE_SUCCESS').subscribe(res => {
                this.toast.success(res);
                this.setPageHistoryCustomerActivities({ offset: 0 });
                this.loadCustomerInfo(this.customerId);
              });
            }
          } else {
            this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe((res: any) => {
              this.toast.error(res);
            });
          }
        }, err => {
          this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe((result: any) => {
            this.toast.error(result);
          });
        });
      }
    });
  }

  onClickStatusBtn() {
    const request: ChangeStatusCustomerRequest = {
      userId: this.user.id,
      phoneCountryCode: this.customer.phoneCountryCode,
      phoneNumber: this.customer.phoneNumber,
      customerId: this.customer.id,
      isActive: +this.customer.status === Status.Activate ? Status.Deactivate : Status.Activate,
      createdByName: this.user ? `${this.user.firstName} ${this.user.lastName}` : ''
    };
    const dialogRef = this.modalService.open(ConfirmDialogComponent, { centered: true });
    const instance = dialogRef.componentInstance;
    instance.typeConfirm = TypeConfirm.ChangeStatusCustomer;
    instance.content = `Are you sure you want to ${this.statusChange} this account?`;
    instance.request = request;
    instance.title = 'Change Status';

    return dialogRef.result.then((result: any) => {
      if (result) {
        this.loadCustomerInfo(this.customerId);
        this.setPageHistoryCustomerActivities({ offset: 0 });
      } else {
        this.reactiveWithNewCustomer();
      }
    }, (reason) => {
    });
  }

  reactiveWithNewCustomer() {
    const dialogRef = this.modalService.open(ReactiveCustomerComponent, { centered: true });
    const instance = dialogRef.componentInstance;
    instance.validatePhones = this.validatePhones;
    instance.currentUser = this.user;
    instance.customerId = this.customerId;
    instance.clickSaveEvent.subscribe((req: any) => {
      if (req) {
        this.customerService.ReactiveCustomer(req).subscribe((res: any) => {
          instance.isLoading = false;
          if (res) {
            instance.activeModal.dismiss();
            this.toast.success('Change status successfully!');
            this.loadCustomerInfo(this.customerId);
            this.setPageHistoryCustomerActivities({ offset: 0 });
          } else {
            this.translate.get('APP.ERROR.CAN_NOT_REACTIVE_WITH_PHONE_NUMBER').subscribe(message => {
              this.toast.error(message);
            });
          }
        }, error => {
          instance.isLoading = false;
          instance.activeModal.dismiss();
          if (error.status > 204) {
            this.translate.get('APP.ERROR.CAN_NOT_REACTIVE_WITH_PHONE_NUMBER').subscribe(message => {
              this.toast.error(message);
            });
          }
        });
      }
    });
  }

  private getSummaryLabelTitle(appSettingsData: any) {
    if (appSettingsData && Array.isArray(appSettingsData)) {
      const appSummaryLabelTitle = appSettingsData.find((x: any) => x.name === AppSettingLabelConstants.AdminAppSummaryLabelTitleText);
      if (appSummaryLabelTitle && appSummaryLabelTitle.value) {
        return appSummaryLabelTitle.value;
      }
    }

    return '';
  }

  private getMembershipButtonColor(appSettingName: string) {

    const appSettingModels = this.appSettingModelsByStoreType;
    if (!Array.isArray(appSettingModels)) {
      return '';
    }

    const appSettingColor = appSettingModels.find(x => (x.name === appSettingName));
    return (appSettingColor && appSettingColor.value) ? JSON.parse(appSettingColor.value) : '';
  }

  private setMembershipButtonColor() {
    this.membershipPremiumButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipPremiumButtonColor);
    this.membershipBasicButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipBasicButtonColor);
  }
}
