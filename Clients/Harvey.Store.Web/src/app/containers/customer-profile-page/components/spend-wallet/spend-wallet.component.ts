import { Component, Input, OnInit } from '@angular/core';
import { CheckPINRequest, Customer, ResendPINRequest } from '../../../../shared/models/customer.model';
import { Outlet } from '../../../../shared/models/outlet.model';
import { User } from '../../../../shared/models/user.model';
import { AuthService } from '../../../../auth/auth.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { WalletTransactionService } from '../../../../shared/services/wallet-transactions.service';
import { SharedService } from '../../../../shared/services/shared.service';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../../../shared/services/customer.service';
import { RoleName } from '../../../../shared/constants/role.constant';
import { SpendWalletRequest } from '../../../../shared/models/walletTransaction.model';
import { TranslateService } from '@ngx-translate/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants, AppSettingLabelByContentTypeConstants } from '../../../../shared/constants/common.constant';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';

@Component({
  selector: 'app-spend-wallet',
  templateUrl: './spend-wallet.component.html',
  styleUrls: ['./spend-wallet.component.scss']
})
export class SpendWalletComponent implements OnInit {

  @Input() walletBalance: number;
  @Input() currentCustomer: Customer;

  public currentOutlet = new Outlet();
  public currentUser = new User();

  public walletValue = '0';
  public isConfirm = false;
  public isNext = true;
  public isEdit = false;
  public isInputPIN = false;
  public charPINvalue = '';
  private pinValue = '';
  ipAddress = '';

  public isWait = false;
  public isWaitResend = false;

  public isResend = false;

  public newBalance = 0;
  public appSettings: any;
  public walletTitle: string;

  constructor(private authService: AuthService,
    public activeModal: NgbActiveModal,
    private walletTransactionService: WalletTransactionService,
    private sharedService: SharedService,
    private toast: ToastrService,
    private customerService: CustomerService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService,
    private appSettingService: AppSettingService) {
  }

  ngOnInit() {
    this.getIpAddress();
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  checkRole(role: Array<string>) {
    role.forEach(element => {
      if (element === RoleName.ROLE_STAFF) {
        return false;
      }
      if (element === RoleName.ROLE_ADMIN_STAFF) {
        return true;
      }
    });
  }

  public addNumber(value: any) {
    if (this.isInputPIN) {
      this.pinValue = this.pinValue + value;
      this.charPINvalue = this.charPINvalue + '*';
    } else {
      if (this.walletValue === '0') {
        this.walletValue = '';
      }
      this.walletValue = this.walletValue + '' + value;
    }
  }

  public subNumber() {
    if (this.isInputPIN) {
      this.pinValue = this.pinValue.substr(0, (this.pinValue.length - 1));
      this.charPINvalue = this.charPINvalue.substr(0, (this.charPINvalue.length - 1));
    } else {
      this.walletValue = this.walletValue.substr(0, (this.walletValue.length - 1));
      if (this.walletValue === '') {
        this.walletValue = '0';
      }
    }
  }

  public addDotValue() {
    const value = '.';
    if (this.isInputPIN) {
      this.pinValue = this.pinValue + value;
      this.charPINvalue = this.charPINvalue + '*';
    } else {
      this.walletValue = this.walletValue + '' + value;
    }
  }

  onClickNextButton() {
    const isVaild = new RegExp('^\\d*\\.?\\d*$').test(this.walletValue.toString());
    if (isVaild && Number(this.walletValue) <= Number(this.walletBalance)) {
      this.isConfirm = false;
      this.isNext = false;
      this.isEdit = false;
      this.isInputPIN = true;
    } else {
      this.translate.get('ERROR.balance-wallet-invalid').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  onClickNextInputPINButton() {
    this.isWait = true;
    const request: CheckPINRequest = {
      userId: this.currentCustomer.id,
      PIN: this.pinValue
    };
    this.customerService.checkPIN(request).subscribe(res => {
      if (res.isValidPIN) {
        this.isWait = false;
        this.charPINvalue = '';
        this.pinValue = '';
        this.isConfirm = true;
        this.isNext = false;
        this.isEdit = true;
        this.isInputPIN = false;
        this.newBalance = Number(this.walletBalance) - Number(this.walletValue);
      } else {
        this.isWait = false;
        this.translate.get('ERROR.invalid-pin').subscribe(message => {
          this.toast.error(message);
        });
        this.isConfirm = false;
        this.isNext = false;
        this.isEdit = false;
        this.isInputPIN = true;
        this.clearValue();
      }
    });
  }

  editAmount() {
    this.isConfirm = false;
    this.isNext = true;
    this.isEdit = false;
    this.isInputPIN = false;
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

  onClickConfirmButton() {
    const userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();


    if (Number(this.walletValue) >= 0) {

      if (this.walletBalance >= Number(this.walletValue)) {
        const request: SpendWalletRequest = {
          customerId: this.currentCustomer.id,
          outletId: this.currentOutlet.id,
          value: Number(this.walletValue),
          userId: this.currentUser.id,
          ipAddress: this.ipAddress,
          staffId: this.currentUser.id,
          createdByName: this.currentUser ? `${this.currentUser.firstName} ${this.currentUser.lastName}` : ''
        };
        this.activeModal.close(request.value);
        this.walletTransactionService.spendWallet(userToken, request).subscribe(res => {
          if (res < 0) {
            this.translate.get('ERROR.spend-failed').subscribe(message => {
              this.toast.error(message);
            });
          } else {
            this.sharedService.spendWalletBalanceSubcription.next(this.newBalance);
            this.translate.get('MESSAGE.spend-success').subscribe(message => {
              this.toast.success(message);
            });
          }
        }, error => {
          this.translate.get('ERROR.spend-failed').subscribe(message => {
            this.toast.error(message);
          });
        });
      } else {
        this.translate.get('ERROR.balance-wallet-invalid').subscribe(message => {
          this.toast.error(message);
        });
      }

    } else {
      this.translate.get('ERROR.spend-greater-zero').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  public getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      this.ipAddress = ip;
    });
  }

  public isValidWalletIput() {
    if (!this.walletValue || Number(this.walletValue) <= 0) {
      return true;
    } else {
      return false;
    }
  }

  public isVaildInputPIN() {
    if (!this.charPINvalue || this.isWait) {
      return true;
    } else {
      return false;
    }
  }

  public clearValue() {
    this.pinValue = '';
    this.charPINvalue = '';
  }

  public getWalletTitle() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
    JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle));
    if (appSetting.value) {
      this.translate.get('TOP_UP_WALLET.wallet-name').subscribe(message => {
        this.walletTitle =  appSetting.value + ' ' + message;
      });
    } else {
      this.walletTitle = '';
    }
  }
}
