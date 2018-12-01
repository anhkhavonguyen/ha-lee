import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PointTransactionService } from '../../../../shared/services/point-transactions.service';
import { AuthService } from '../../../../auth/auth.service';
import { Outlet } from '../../../../shared/models/outlet.model';
import { SharedService } from '../../../../shared/services/shared.service';
import { CheckPINRequest, Customer, ResendPINRequest } from '../../../../shared/models/customer.model';
import { User } from '../../../../shared/models/user.model';
import { RedeemPointRequest } from '../../../../shared/models/pointTransaction.model';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../../../shared/services/customer.service';
import { TranslateService } from '@ngx-translate/core';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { CommonConstants, AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-redeem-point',
  templateUrl: './redeem-point.component.html',
  styleUrls: ['./redeem-point.component.scss']
})
export class RedeemPointComponent implements OnInit {

  @Input() rewardPoint: number;
  @Input() currentCustomer: Customer;
  @Input() options: Array<any> = [];

  public isNext = false;
  public convertedPointValue: number;
  public currentOutlet = new Outlet();


  public pointRedeemDiscountValue: number;
  PINvalue = '';
  public isSelectedOption = false;
  rate: number;
  selectedOptionValue: any;
  public currentUser = new User();
  hasPINValue = false;
  isConfirm = false;
  charPINvalue = '';
  isValidPinLength = true;
  ipAddress = '';

  public isWait = false;
  public isWaitResend = false;

  public isResend = false;

  public newBalance = 0;

  public appSettings: any;
  public rewardsPointsTitle: string;

  constructor(private authService: AuthService,
    public activeModal: NgbActiveModal,
    private pointTransactionService: PointTransactionService,
    private sharedService: SharedService,
    private toast: ToastrService,
    private customerService: CustomerService,
    private translate: TranslateService,
    private oAuthStorage: OAuthStorage,
    private appSettingService: AppSettingService) {
  }

  ngOnInit() {
    this.sortOption();
    this.getIpAddress();
    this.getRewardsPointsTitle();
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  sortOption() {
    this.options.sort((item1, item2) => parseInt(item1.value) - parseInt(item2.value));

    for (let i = 0; i < this.options.length; i++) {
      if (this.options[i].lable.valueOf() > this.rewardPoint.valueOf()) {
        this.options[i].disabled = true;
      } else {
        this.options[i].disabled = false;
      }
    }
  }
  onRedeemOptionClicked(value: any): void {
    this.isNext = true;
    this.isSelectedOption = true;
    this.selectedOptionValue = value;
    this.newBalance = Number(this.rewardPoint) - Number(this.selectedOptionValue.lable);
  }

  onClickNextInputPINButton() {
    if (this.charPINvalue.length !== 4) {
      this.translate.get('ERROR.invalid-pin').subscribe(message => {
        this.toast.error(message);
      });
      this.isValidPinLength = false;
      this.clearValue();
    } else {
      const request: CheckPINRequest = {
        userId: this.currentCustomer.id,
        PIN: this.PINvalue
      };
      this.isWait = true;
      this.customerService.checkPIN(request).subscribe(res => {
        if (res.isValidPIN) {
          this.isWait = false;
          this.isConfirm = true;
        } else {
          this.translate.get('ERROR.invalid-pin').subscribe(message => {
            this.toast.error(message);
          });
          this.isConfirm = false;
          this.isWait = false;
          this.clearValue();
        }
      });
    }
  }

  onClickConfirmButton() {
    const userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();

    const request: RedeemPointRequest = {
      customerId: this.currentCustomer.id,
      outletId: this.currentOutlet.id,
      value: this.selectedOptionValue.lable,
      userId: this.currentUser.id,
      ipAddress: this.ipAddress
    };
    const newBalance = this.rewardPoint - Number(this.selectedOptionValue.lable);
    if (newBalance < 0) {
      const flagInvalidRedeem = -1;
      this.activeModal.close(flagInvalidRedeem);
      this.translate.get('ERROR.balance-points-invalid').subscribe(message => {
        this.toast.error(message);
      });
    } else {
      this.activeModal.close(request.value);
      this.pointTransactionService.redeemPointTransaction(userToken, request).subscribe(res => {
        if (res !== -1) {
          this.translate.get('MESSAGE.redeem-success').subscribe(message => {
            this.toast.success(message);
          });
          this.sharedService.redeemBalanceSubscription.next(newBalance);
        } else {
          this.translate.get('ERROR.balance-points-invalid').subscribe(message => {
            this.toast.error(message);
          });
        }
      }, error => {
        this.translate.get('ERROR.something-wrong').subscribe(message => {
          this.toast.error(message);
        });
      });
    }
  }

  resendPIN() {
    const outlet = this.sharedService.getAnnounceOutlet();
    const request: ResendPINRequest = {
      countryCode: this.currentCustomer.phoneCountryCode,
      numberPhone: this.currentCustomer.phone,
      outletName: encodeURIComponent(outlet.name),
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue)
    };
    this.isWaitResend = true;
    this.customerService.resendPIN(request).subscribe(res => {
      this.translate.get('MESSAGE.pin-was-send').subscribe(message => {
        this.toast.success(message);
      });
      this.isWaitResend = false;
      this.isResend = true;
    });
  }

  public addNumber(value: any) {
    this.hasPINValue = true;
    if (this.PINvalue == null) {
      this.PINvalue = '';
    }
    if (this.charPINvalue == null) {
      this.charPINvalue = '';
    }
    this.PINvalue = this.PINvalue + value;
    this.charPINvalue = this.charPINvalue + '*';

  }

  public clearValue() {
    this.PINvalue = '';
    this.charPINvalue = '';
    this.hasPINValue = false;
  }

  public subNumber() {
    this.PINvalue = this.PINvalue.substr(0, (this.PINvalue.length - 1));
    this.charPINvalue = this.charPINvalue.substr(0, (this.charPINvalue.length - 1));
  }

  editAmount() {
    this.isNext = false;
    this.isSelectedOption = false;
    this.PINvalue = null;
    this.charPINvalue = null;
    this.isConfirm = false;
  }

  public getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      this.ipAddress = ip;
    });
  }

  public isVaildInputPIN() {
    if (!this.charPINvalue || this.isWait) {
      return true;
    } else {
      return false;
    }
  }

  public getRewardsPointsTitle() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
    JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle));
    if (appSetting.value) {
      this.translate.get('REDEEM_POINT.rewards-points').subscribe(message => {
        this.rewardsPointsTitle =  appSetting.value + ' ' + message;
      });
    } else {
      this.rewardsPointsTitle = '';
    }
  }
}
