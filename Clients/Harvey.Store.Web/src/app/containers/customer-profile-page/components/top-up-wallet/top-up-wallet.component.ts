import { Component, Input, OnInit } from '@angular/core';
import { Customer } from '../../../../shared/models/customer.model';
import { Outlet } from '../../../../shared/models/outlet.model';
import { User } from '../../../../shared/models/user.model';
import { AuthService } from '../../../../auth/auth.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SharedService } from '../../../../shared/services/shared.service';
import { TopUpWalletRequest } from '../../../../shared/models/walletTransaction.model';
import { WalletTransactionService } from '../../../../shared/services/wallet-transactions.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../../../shared/constants/common.constant';

@Component({
  selector: 'app-top-up-wallet',
  templateUrl: './top-up-wallet.component.html',
  styleUrls: ['./top-up-wallet.component.scss']
})
export class TopUpWalletComponent implements OnInit {

  @Input() walletBalance: number;
  @Input() currentCustomer: Customer;

  public currentOutlet = new Outlet();
  public currentUser = new User();
  public walletValue = '';
  public isConfirm = false;
  public isNext = false;
  public isSelectedOption = false;
  public selectedOptionValue: any;
  public isOther = false;
  public isShowWalletInput = false;
  public isEdit = false;
  ipAddress = '';

  options: Array<any> = [];


  public newBalance = 0;
  public appSettings: any;
  public walletTitle: string;

  constructor(private authService: AuthService,
    public activeModal: NgbActiveModal,
    private walletTransactionService: WalletTransactionService,
    private sharedService: SharedService,
    private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService) {
  }

  ngOnInit() {
    this.setDisableOption();
    this.getIpAddress();
    this.getWalletTitle();

  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  setDisableOption() {
    this.options = [{ lable: '$20.00', value: 20, 'disabled': false },
    { lable: 'Other', value: -1, 'disabled': false }];
  }

  onTopUpOptionClicked(value: any): void {
    this.isSelectedOption = true;
    this.selectedOptionValue = value;
    if (value.value === -1) {
      this.isOther = true;
      this.isConfirm = false;
      this.isNext = true;
      this.isShowWalletInput = true;
      this.walletValue = '0';
    } else {
      this.walletValue = value.value;
      this.newBalance = Number(this.walletBalance) + Number(this.walletValue);
      this.isOther = false;
      this.isConfirm = true;
      this.isNext = false;
      this.isShowWalletInput = false;
    }
  }

  public addNumber(value: any) {
    if (this.walletValue === '0') {
      this.walletValue = '';
    }
    this.walletValue = this.walletValue + '' + value;
  }

  public subNumber() {
    this.walletValue = this.walletValue.substr(0, (this.walletValue.length - 1));
    if (this.walletValue === '') {
      this.walletValue = '0';
    }
  }

  public addDotValue() {
    const value = '.';
    this.walletValue = this.walletValue + '' + value;
  }

  onClickNextButton() {
    const isVaild = new RegExp('^\\d*\\.?\\d*$').test(this.walletValue.toString());
    if (isVaild) {
      this.isConfirm = true;
      this.isNext = false;
      this.newBalance = Number(this.walletBalance) + Number(this.walletValue);
      if (this.isShowWalletInput) {
        this.isEdit = true;
      } else {
        this.isEdit = false;
      }
    } else {
      this.translate.get('ERROR.top-up-value-invalid').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  editAmount() {
    this.isEdit = false;
    this.isNext = true;
    this.isConfirm = false;
  }

  onClickConfirmButton() {
    const userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();

    const request: TopUpWalletRequest = {
      userId: this.currentUser.id,
      customerId: this.currentCustomer.id,
      outletId: this.currentOutlet.id,
      value: Number(this.walletValue),
      ipAddress: this.ipAddress
    };

    this.activeModal.close(request.value);
    if (Number(this.walletValue) <= 0 || !this.walletValue) {
      this.translate.get('ERROR.top-up-greater-zero').subscribe(message => {
        this.toast.error(message);
      });
    } else {
      this.walletTransactionService.topUpWallet(userToken, request).subscribe(res => {
        if (res < 0) {
          this.translate.get('ERROR.top-up-failed').subscribe(message => {
            this.toast.error(message);
          });
        } else {
          this.sharedService.topUpWalletBalanceSubcription.next(this.newBalance);
          this.translate.get('MESSAGE.top-up-success').subscribe(message => {
            this.toast.success(message);
          });
        }
      }, error => {
        this.translate.get('ERROR.top-up-failed').subscribe(message => {
          this.toast.error(message);
        });
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
