import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RedeemPointComponent } from '../../../containers/customer-profile-page/components/redeem-point/redeem-point.component';
import { Customer } from '../../models/customer.model';
import { TopUpWalletComponent } from '../../../containers/customer-profile-page/components/top-up-wallet/top-up-wallet.component';
import { SpendWalletComponent } from '../../../containers/customer-profile-page/components/spend-wallet/spend-wallet.component';
import { InfoDialogComponent } from '../info-dialog/info-dialog.component';
import {
  AddLoyaltyPointComponent
} from '../../../containers/customer-profile-page/components/add-loyalty-point/add-loyalty-point.component';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ExpiryPointDialogComponent } from '../expiry-point-dialog/expiry-point-dialog.component';
import * as moment from 'moment';
import { GetExpiryPointsRequest, GetExpiryPointsResponse } from '../../models/pointTransaction.model';
import { PointTransactionService } from '../../services/point-transactions.service';
import { CommonConstants } from '../../constants/common.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { AppSettingService } from '../../services/app-setting.service';

const cardType = {
  point: {
    typeName: 'Point',
    title: 'Reward Point',
    icon: '/assets/img/tog_store_points.png',
    blankBalance: 0,
    addButtonLable: 'Add Points',
    spendButtonLable: 'Redeem',
    titleBalance: 'Point earned',
    currencyUnit: ''
  },
  wallet: {
    typeName: 'Wallet',
    title: 'Wallet',
    icon: '/assets/img/tog_store_wallet.png',
    blankBalance: 0,
    addButtonLable: 'Top up',
    spendButtonLable: 'Spend',
    titleBalance: 'Credit balance',
    currencyUnit: '$'
  }
};
const appSettingTypeStoreApp = 2;
const base64Format = 'data:image/png;base64,';
@Component({
  selector: 'app-balance-card',
  templateUrl: './balance-card.component.html',
  styleUrls: ['./balance-card.component.scss']
})
export class BalanceCardComponent implements OnInit {

  @Input() type = cardType.point.typeName;
  @Input() balance;
  @Input() currentCustomer: Customer;
  @Input() isWaitting = true;
  @Input() isBirthmonth: boolean;
  @Input() isAllowRedeem: boolean;
  @Input() appSettings: any;
  @Input() isLoadExpiringPoint = true;
  @Input() getExpiryPointsResponse: GetExpiryPointsResponse;

  @Output() newBalance = new EventEmitter<number>();

  card: any;
  addButtonStyles: any;
  spendButtonStyles: any;
  public icon = '';
  public membershipTypePremium = 'Premium+';
  public title: string;
  public options: Array<any> = [];
  public rate: number;

  constructor(private modalService: NgbModal,
    private translate: TranslateService,
    private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private pointTransactionService: PointTransactionService,
    private appSettingService: AppSettingService) {
  }

  ngOnInit() {
    this.setButtonStyles('StoreAppAddButtonColor');
    this.setButtonStyles('StoreAppSpendButtonColor');
    this.title = this.getSummaryLabelTitle();
    this.getCardType(this.type);
    this.setRedeemRateValueByAppSettingsOfStore();
  }

  getCardType(type: string) {
    switch (type) {
      case cardType.point.typeName: {
        this.card = cardType.point;
        this.title = `${this.title} ${cardType.point.title}`;
        this.icon = this.appSettings.appSettingModels ?
          `${base64Format}${this.appSettings.appSettingModels.find(a => a.name === 'IconPoint' &&
            a.appSettingTypeId === appSettingTypeStoreApp).value}`
          : this.card.icon;
        break;
      }
      case cardType.wallet.typeName: {
        this.card = cardType.wallet;
        this.title = `${this.title} ${cardType.wallet.title}`;
        this.icon = this.appSettings.appSettingModels ?
          `${base64Format}${this.appSettings.appSettingModels.find(a => a.name === 'IconWallet' &&
            a.appSettingTypeId === appSettingTypeStoreApp).value}`
          : this.card.icon;
        break;
      }
    }
  }

  emitNewBalance(balance: number) {
    this.newBalance.emit(balance);
  }

  onAddingPointClick() {
    if (this.appSettings) {
      const dialogRef = this.modalService.open(AddLoyaltyPointComponent, { size: 'lg', centered: true, backdrop: 'static' });
      const instance = dialogRef.componentInstance;
      instance.rewardPoint = this.balance;
      instance.currentCustomer = this.currentCustomer;
      instance.isBirthmonth = this.isBirthmonth;
      return dialogRef.result.then((result) => {
        if (result) {
          this.balance = result;
          this.emitNewBalance(this.balance);
        } else {
          this.emitNewBalance(null);
        }

      }, (reason) => {
      });
    } else {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning(message);
      });
    }

  }

  onRedeemPointClick() {
    if (this.appSettings && this.options.length !== 0) {
      const dialogRef = this.modalService.open(RedeemPointComponent, { size: 'lg', centered: true, backdrop: 'static' });
      const instance = dialogRef.componentInstance;
      instance.rewardPoint = this.balance;
      instance.currentCustomer = this.currentCustomer;
      instance.options = this.options;
      return dialogRef.result.then((result) => {
        if (result && result !== -1) {
          this.balance = this.balance - result;
          this.emitNewBalance(this.balance);
        }
        if (!result) {
          this.emitNewBalance(null);
        }
      }, () => {
      });
    } else {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning(message);
      });
    }
  }

  onAddClick() {
    this.type === cardType.point.typeName ? this.onAddingPointClick() : this.onTopupClick();
  }

  onSubClick() {
    this.type === cardType.point.typeName ? this.onRedeemPointClick() : this.onSpendingClick();
  }

  onTopupClick() {
    const dialogRef = this.modalService.open(TopUpWalletComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.walletBalance = this.balance;
    instance.currentCustomer = this.currentCustomer;

    return dialogRef.result.then((result) => {
      this.balance = this.balance + result;
      this.emitNewBalance(this.balance);
    }, () => {
    });
  }

  onSpendingClick() {
    const dialogRef = this.modalService.open(SpendWalletComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.walletBalance = this.balance;
    instance.currentCustomer = this.currentCustomer;

    return dialogRef.result.then((result) => {
      this.balance = this.balance - result;
      this.emitNewBalance(this.balance);
    }, () => {
    });
  }


  isInvalidMembershipTypeForRedeem() {
    return this.isAllowRedeem;
  }

  isSpendAvailable() {
    if ((this.type === 'Point' && !this.isAllowRedeem) || this.balance === 0) {
      return true;
    } else {
      return false;
    }
  }

  openModalHistory() {
    const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true });
    const instance = dialogRef.componentInstance;
    instance.header = this.type === cardType.point.typeName ? 'Points transaction history' : 'Wallet transaction history';
    instance.isViewHistory = true;
    instance.typeHistory = this.type;
    instance.userId = this.currentCustomer.id;
  }

  openExpiringPoint() {
    if (this.type === cardType.point.typeName && this.getExpiryPointsResponse) {
      const dialogRef = this.modalService.open(ExpiryPointDialogComponent, { size: 'lg', centered: true });
      const instance = dialogRef.componentInstance;
      instance.getExpiryPointsResponse = this.getExpiryPointsResponse;
    } else {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.warning(message);
      });
    }
  }

  getSummaryLabelTitle() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle));
    if (appSetting.value) {
      return appSetting.value;
    }
    return '';
  }

  setButtonStyles(colorType) {
    const appSettingButtonColorValue = JSON.parse(this.appSettings.appSettingModels.find( a => a.name === colorType).value);
    if (colorType === 'StoreAppAddButtonHoverColor' || colorType === 'StoreAppAddButtonColor' ) {
      this.addButtonStyles = {
        'background-color': appSettingButtonColorValue['background-color'],
        'border-color':  appSettingButtonColorValue['border-color'],
        'color': appSettingButtonColorValue['color'],
      };
    } else {
      this.spendButtonStyles = {
        'background-color': appSettingButtonColorValue['background-color'],
        'border-color':  appSettingButtonColorValue['border-color'],
        'color': appSettingButtonColorValue['color'],
      };
    }
  }

  setRedeemRateValueByAppSettingsOfStore() {
    this.appSettingService.getAppSettingsByType(CommonConstants.storeTypeAppsetting).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        const appSettingModelsByStoreType = res.appSettingModels;
        this.setRedeemRateValue(appSettingModelsByStoreType);
        this.setDisableOption(appSettingModelsByStoreType);
      }
    });
  }

  setRedeemRateValue(appSettingsOfStore) {
    const redeemRateValue = appSettingsOfStore.find(item => item.groupName === 'RateValue' && item.name === 'RedeemRateValue');
    if (redeemRateValue) {
      this.rate = redeemRateValue.value;
    }
  }

  setDisableOption(appSettingsOfStore) {
    appSettingsOfStore.forEach(element => {
      if (element.groupName === 'OptionRedeem') {
        const item = {
          lable: element.name,
          value: this.rate * element.value,
          disabled: false
        };
        this.options.push(item);
      }
    });

    this.options.sort((item1, item2) => parseInt(item1.value) - parseInt(item2.value));

    for (let i = 0; i < this.options.length; i++) {
      if (this.options[i].lable.valueOf() > this.balance.valueOf()) {
        this.options[i].disabled = true;
      }
    }
  }
}

