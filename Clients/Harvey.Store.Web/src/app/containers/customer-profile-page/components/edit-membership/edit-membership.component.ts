import { Component, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';
import { Customer } from '../../../../shared/models/customer.model';
import { User } from '../../../../shared/models/user.model';
import { Outlet } from '../../../../shared/models/outlet.model';
import { SharedService } from '../../../../shared/services/shared.service';
import { MembershipTransactionService } from '../../../../shared/services/membership-transaction.service';
import { AuthService } from '../../../../auth/auth.service';
import {
  AddMembershipCommand,
  AnnounceNewMembership,
  AnnounceEditComment,
  MembershipActionType,
  OptionExpiryDate
} from '../../../../shared/models/membershipTransaction.model';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../../shared/services/user.service';
import { RoleName } from '../../../../shared/constants/role.constant';
import { TopUpWalletRequest } from 'src/app/shared/models/walletTransaction.model';
import { WalletTransactionService } from 'src/app/shared/services/wallet-transactions.service';
import { TranslateService } from '@ngx-translate/core';
import { NgbDateCustomParserFormatter } from '../../../../shared/helper/ngb-datepicker-formatter';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { CommonConstants } from 'src/app/shared/constants/common.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';


const membershipTypeFE = {
  basic: {
    typeName: 'Basic',
    displayName: 'Basic',
    typeCode: '1'
  },
  premiumPlus: {
    typeName: 'Premium+',
    displayName: 'Premium+',
    typeCode: '2'
  }
};

const defaultOptionExpired: OptionExpiryDate = {
  periodTime: 1,
  topupValue: 10
};
const defaultAutoTopupValue = 10;
const upgrade = 2;
const extend = 4;
const renew = 3;
@Component({
  selector: 'app-edit-membership',
  templateUrl: './edit-membership.component.html',
  styleUrls: ['./edit-membership.component.scss'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ],
  encapsulation: ViewEncapsulation.None
})
export class EditMembershipComponent implements OnInit {

  @Input() currentCustomer = new Customer();
  @Input() currentUser = new User();
  @Input() currentOutlet = new Outlet();
  @Input() userRoles = [];
  @Input() walletBalance = 0;

  @ViewChild('datepicker') datePicker;

  public listOptionsExpiredDate: Array<OptionExpiryDate> = [];
  public autoTopupValue;
  public chooseOptionExpiredDate: OptionExpiryDate;
  public currentMembership: any;
  public chooseMembership: any;
  public expiredDatePlaceHolder = '';
  public chooseExpiredDate: any = null;
  public currentExpiredDate;
  public isCheck = false;
  public statusCurrentMembership;
  public isAutoTopupWallet;

  public chooseMembershipTypeId = membershipTypeFE.basic.typeCode;
  public comment = '';
  public appSettingsByStoreType: any;

  public membershipButtonColorProperties = {};

  public ipAddress = '';

  public isWait = false;
  public fullPhoneNumber = '';

  public userToken = '';

  public isShowEditForm = false;
  public isShowChooseEditAction = true;
  public isEditComment = false;
  public isEditMembership = false;
  public appSettings: any;
  public walletTitle: string;

  constructor(public activeModal: NgbActiveModal,
    private sharedService: SharedService,
    private membershipTransactionService: MembershipTransactionService,
    private authService: AuthService,
    private toast: ToastrService,
    private walletTransactionService: WalletTransactionService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService,
    private appSettingService: AppSettingService) {
    this.initPage();
  }

  initPage() {
    this.userToken = this.authService.getTokenFormStorage();
    this.listOptionsExpiredDate.push(defaultOptionExpired);
    this.chooseOptionExpiredDate = this.listOptionsExpiredDate[0];
    this.autoTopupValue = defaultAutoTopupValue;
  }

  ngOnInit() {
    if (this.currentUser && this.currentOutlet && this.userToken) {
      this.getCurrentMembership();
      this.setValueFromAppSettingsOfStore();
      this.statusCurrentMembership = this.getStatusCurrentMembership();
    } else {
      this.authService.logout();
    }
    this.getIpAddress();
    this.getWalletTitle();
  }

  getCurrentMembership() {
    this.currentMembership = this.setShowMembership(this.currentCustomer.membership);
    this.fullPhoneNumber = `+${this.currentCustomer.phoneCountryCode} ${this.currentCustomer.phone}`;
    this.comment = this.currentCustomer.commentMembership;
    if (this.currentCustomer.expiredDate) {
      this.currentExpiredDate = moment.utc(this.currentCustomer.expiredDate).local().format('YYYY/MM/DD');
      this.expiredDatePlaceHolder = moment.utc(this.currentCustomer.expiredDate).local().format('DD/MM/YYYY');
    }
  }

  getAutoTopupValue() {
    return Number(this.appSettingsByStoreType.find(element => element.name === 'AutoTopupWalletValue').value);
  }

  getListOptionExpiredDate() {
    const listOptionsExpiredDate: Array<OptionExpiryDate> = [];
    this.appSettingsByStoreType.forEach(element => {
      if (element.groupName === 'ExpiredDatePeriod') {
        const tempOption: OptionExpiryDate = {
          periodTime: Number(element.value),
          topupValue: this.autoTopupValue * Number(element.value)
        };
        listOptionsExpiredDate.push(tempOption);
      }
    });
    return listOptionsExpiredDate;
  }

  getIsAutoTopupWallet() {
    return this.appSettingsByStoreType.find(a => a.name === 'IsAutoTopupWallet').value === 'true' ? true : false;
  }

  setShowMembership(membershipName: string) {
    switch (membershipName) {
      case membershipTypeFE.basic.typeName:
        this.membershipButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipBasicButtonColor);
        return membershipTypeFE.basic;

      case membershipTypeFE.premiumPlus.typeName:
        this.membershipButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipPremiumButtonColor);
        return membershipTypeFE.premiumPlus;

      default:
        return membershipTypeFE.basic;
    }
  }

  getStatusCurrentMembership() {
    const expired = moment.utc(this.currentCustomer.expiredDate).local().format('YYYY/MM/DD');
    const now = moment.utc(new Date()).local().format('YYYY/MM/DD');
    if (this.currentMembership.typeName === membershipTypeFE.basic.typeName) {
      return upgrade;
    }
    if (expired < now && this.currentCustomer.hasPreminumMembershipTransaction
      && this.currentMembership.typeName === membershipTypeFE.premiumPlus.typeName) {
      return renew;
    }
    return extend;
  }

  onCheckBoxOptionClick() {
    let newExpiredDate: Date;
    const statusCurrentMembership = this.getStatusCurrentMembership();
    this.chooseExpiredDate = '';
    this.chooseMembership = null;
    this.isCheck = !this.isCheck;
    if (this.isCheck) {
      if (statusCurrentMembership === upgrade) {
        const nowTime = new Date(Date.now());
        newExpiredDate = nowTime;
        newExpiredDate.setFullYear(nowTime.getFullYear() + this.chooseOptionExpiredDate.periodTime);
        this.expiredDatePlaceHolder = moment.utc(newExpiredDate).local().format('DD/MM/YYYY');
        this.chooseExpiredDate = newExpiredDate;
        this.currentMembership = membershipTypeFE.premiumPlus;
        this.chooseMembership = membershipTypeFE.premiumPlus;
        this.membershipButtonColorProperties = this.getMembershipButtonColor(CommonConstants.storeMembershipPremiumButtonColor);
      }
      if (statusCurrentMembership === renew) {
        const nowTime = new Date(Date.now());
        newExpiredDate = nowTime;
        newExpiredDate.setFullYear(nowTime.getFullYear() + this.chooseOptionExpiredDate.periodTime);
        this.expiredDatePlaceHolder = moment.utc(newExpiredDate).local().format('DD/MM/YYYY');
        this.chooseExpiredDate = newExpiredDate;
        this.chooseMembership = this.currentMembership;
      }
      if (statusCurrentMembership === extend) {
        const currentExpiredDate = new Date(this.currentExpiredDate);
        newExpiredDate = currentExpiredDate;
        newExpiredDate.setFullYear(currentExpiredDate.getFullYear() + this.chooseOptionExpiredDate.periodTime);
        this.expiredDatePlaceHolder = moment.utc(newExpiredDate).local().format('DD/MM/YYYY');
        this.chooseExpiredDate = newExpiredDate;
        this.chooseMembership = this.currentMembership;
      }
    } else {
      this.expiredDatePlaceHolder = this.currentCustomer.expiredDate ?
        moment.utc(this.currentCustomer.expiredDate).local().format('DD/MM/YYYY') : null;
      this.currentMembership = this.setShowMembership(this.currentCustomer.membership);
      this.chooseExpiredDate = null;
      this.chooseMembership = null;
    }
  }

  onClickDatepicker() {
    this.chooseMembership = this.currentMembership;
    this.expiredDatePlaceHolder = this.currentCustomer.expiredDate ?
      moment.utc(this.currentCustomer.expiredDate).local().format('DD/MM/YYYY') : null;
    this.currentMembership = this.setShowMembership(this.currentCustomer.membership);
    this.chooseExpiredDate = null;
    this.isCheck = false;
    this.datePicker.toggle();
  }

  setNewExpiredMembership(chooseExpired: any) {
    let newExpiredDate: Date;
    let chooseExpiredDate;
    if (!chooseExpired) {
      return null;
    } else {
      if (this.isCheck) {
        chooseExpiredDate = moment.utc(chooseExpired).local().format('YYYY/MM/DD');
        newExpiredDate = new Date(chooseExpiredDate);
      } else {
        chooseExpiredDate = chooseExpired;
        const tempChooseExpiredDate = new Date(chooseExpiredDate.year,
          chooseExpiredDate.month - 1, chooseExpiredDate.day);
        newExpiredDate = new Date(tempChooseExpiredDate);
      }
    }
    return newExpiredDate;
  }

  editMembership(chooseExpired: any) {
    const newExpiredDate = this.setNewExpiredMembership(chooseExpired);
    const nowTime = new Date(moment.utc(Date.now()).local().format('YYYY/MM/DD'));
    if (!newExpiredDate) {
      this.translate.get('ERROR.empty-choose-expired-date').subscribe(message => {
        this.toast.error(message);
        this.isWait = false;
      });
    }
    if (nowTime >= newExpiredDate) {
      this.translate.get('ERROR.choosing-date-higher').subscribe(message => {
        this.toast.error(message);
        this.isWait = false;
      });
    } else {
      let membershipActionType = 0;
      if (!this.isCheck) {
        membershipActionType = MembershipActionType.ChangeExpiredDate;
      } else {
        membershipActionType = this.statusCurrentMembership ? this.statusCurrentMembership : 0;
      }
      const transaction: AddMembershipCommand = {
        customerId: this.currentCustomer.id,
        outletId: this.currentOutlet.id,
        userId: this.currentUser.id,
        comment: this.comment,
        expiredDate: newExpiredDate,
        membershipTypeId: this.chooseMembership.typeCode,
        ipAddress: this.ipAddress,
        membershipActionType: membershipActionType
      };
      this.membershipTransactionService.addMembershipTransaction(transaction).subscribe(res => {
        if (res) {
          this.translate.get('MESSAGE.edit-membership-success').subscribe(message => {
            this.toast.success(message);
            if (this.isCheck) {
              this.autoTopupWallet();
            }
            const announceNewMembership: AnnounceNewMembership = {
              membershipName: this.chooseMembership.typeName,
              expired: newExpiredDate,
              totalTransaction: this.currentCustomer.totalStranstion + 1,
              comment: this.comment
            };
            this.sharedService.membershipEditSubcription.next(announceNewMembership);
            this.isWait = false;
          });
        } else {
          this.translate.get('ERROR.edit-membership-failed').subscribe(message => {
            this.toast.error(message);
            const editFailed = '-1';
            const announceFailedNewMembership: AnnounceNewMembership = {
              membershipName: editFailed,
              expired: null,
              totalTransaction: this.currentCustomer.totalStranstion,
              comment: this.comment
            };
            this.sharedService.membershipEditSubcription.next(announceFailedNewMembership);
            this.isWait = false;
          });
        }
      }, error => {
        this.translate.get('ERROR.something-wrong').subscribe(message => {
          this.toast.error(message);
          this.isWait = false;
        });
      });
    }
  }

  autoTopupWallet() {
    if (this.chooseMembership.typeName === membershipTypeFE.premiumPlus.typeName &&
      this.isAutoTopupWallet && this.autoTopupValue) {
      const request: TopUpWalletRequest = {
        userId: this.currentUser.id,
        customerId: this.currentCustomer.id,
        outletId: this.currentOutlet.id,
        value: this.chooseOptionExpiredDate.topupValue,
        ipAddress: this.ipAddress
      };
      this.walletTransactionService.topUpWallet(this.userToken, request).subscribe(() => {
        this.sharedService.topUpWalletBalanceSubcription.next(this.walletBalance + this.chooseOptionExpiredDate.topupValue);
        this.translate.get('MESSAGE.auto-topup-success').subscribe(message => {
          this.toast.success(`${this.chooseOptionExpiredDate.topupValue}$ ${message}`);
        });
      }, error => {
        this.translate.get('ERROR.auto-topup-failed').subscribe(message => {
          this.toast.error(`${this.chooseOptionExpiredDate.topupValue}$ ${message}`);
        });
      });
    }
  }

  editMembershipByStaff() {
    if (!this.chooseExpiredDate) {
      this.translate.get('ERROR.empty-choose-expired-date').subscribe(message => {
        this.toast.error(message);
        this.isWait = false;
      });
    } else {
      if (this.chooseMembership.typeName === membershipTypeFE.basic.typeName
        && this.currentMembership.typeName === membershipTypeFE.premiumPlus.typeName) {
        this.translate.get('ERROR.do-not-permission-downgrade').subscribe(message => {
          this.toast.error(message);
          this.isWait = false;
        });
      }
      if (this.chooseMembership.typeName === membershipTypeFE.basic.typeName
        && this.currentMembership.typeName === membershipTypeFE.basic.typeName) {
        this.translate.get('ERROR.current-membership-is-basic').subscribe(message => {
          this.toast.error(message);
          this.isWait = false;
        });
      } else {
        this.editMembership(this.chooseExpiredDate);
      }
    }
  }

  editMembershipByAdminStaff() {
    if (!this.chooseExpiredDate) {
      this.translate.get('ERROR.empty-choose-expired-date').subscribe(message => {
        this.toast.error(message);
        this.isWait = false;
      });
    } else {
      if (this.chooseMembership.typeName === membershipTypeFE.basic.typeName
        && this.currentMembership.typeName === membershipTypeFE.basic.typeName) {
        this.translate.get('ERROR.current-membership-is-basic').subscribe(message => {
          this.toast.error(message);
          this.isWait = false;
        });
      } else {
        this.editMembership(this.chooseExpiredDate);
      }
    }
  }

  onClickSaveButton() {
    this.isWait = true;
    if (this.isEditMembership) {
      this.onClickButtonSaveEditMembership();
    }
    if (this.isEditComment) {
      this.onClickButtonSaveEditComment();
    }
  }

  onClickButtonSaveEditMembership() {
    const isAdminStaff = this.checkRoleAdmin(this.userRoles);
    if (isAdminStaff) {
      this.editMembershipByAdminStaff();
    } else {
      this.editMembershipByStaff();
    }
  }

  onClickButtonSaveEditComment() {
    const currentExpiredDate = new Date(this.currentExpiredDate);
    const transaction: AddMembershipCommand = {
      customerId: this.currentCustomer.id,
      outletId: this.currentOutlet.id,
      userId: this.currentUser.id,
      comment: this.comment,
      expiredDate: currentExpiredDate,
      membershipTypeId: this.currentMembership.typeCode,
      ipAddress: this.ipAddress,
      membershipActionType: MembershipActionType.Comment
    };
    this.membershipTransactionService.addMembershipTransaction(transaction).subscribe(res => {
      if (res) {
        this.translate.get('MESSAGE.edit-comment-success').subscribe(message => {
          this.toast.success(message);
          const announceEditComment: AnnounceEditComment = {
            comment: this.comment,
            totalTransaction: this.currentCustomer.totalStranstion + 1
          };
          this.sharedService.editCommentSubcription.next(announceEditComment);
          this.isWait = false;
        });
      } else {
        this.translate.get('ERROR.edit-comment-failed').subscribe(message => {
          this.toast.error(message);
          const announceEditComment: AnnounceEditComment = {
            comment: this.comment,
            totalTransaction: this.currentCustomer.totalStranstion
          };
          this.sharedService.editCommentSubcription.next(announceEditComment);
          this.isWait = false;
        });
      }
    }, error => {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
        this.isWait = false;
      });
    });
  }

  public getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      this.ipAddress = ip;
    });
  }

  onClickEditMembershipButton() {
    this.isShowChooseEditAction = false;
    this.isEditComment = false;
    this.isEditMembership = true;
  }

  onClickEditCommentButton() {
    this.isShowChooseEditAction = false;
    this.isEditComment = true;
    this.isEditMembership = false;
  }

  onClose(result?: boolean): void {
    this.activeModal.close(result);
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  checkRoleAdmin(role: Array<string>) {
    if (role.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
      return false;
    } else {
      return true;
    }
  }

  private setValueFromAppSettingsOfStore() {
    this.appSettingService.getAppSettingsByType(CommonConstants.storeTypeAppsetting).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingsByStoreType = res.appSettingModels;
        this.isAutoTopupWallet = this.getIsAutoTopupWallet();
        this.autoTopupValue = this.getAutoTopupValue() ? this.getAutoTopupValue() : defaultAutoTopupValue;
        const tempListOptionExpiredDate = this.getListOptionExpiredDate();
        if (tempListOptionExpiredDate && tempListOptionExpiredDate.length) {
          this.listOptionsExpiredDate = tempListOptionExpiredDate;
          this.chooseOptionExpiredDate = this.listOptionsExpiredDate[0];
        }
      } else {
        this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, () => {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  public getWalletTitle() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle));
    if (appSetting.value) {
      this.translate.get('EDIT_MEMBERSHIP.in-tog-wallet').subscribe(message => {
        this.walletTitle = 'in ' + appSetting.value + ' ' + message;
      });
    } else {
      this.walletTitle = '';
    }
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
