import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PointTransactionService } from '../../../../shared/services/point-transactions.service';
import { AddPointRequest } from '../../../../shared/models/pointTransaction.model';
import { AuthService } from '../../../../auth/auth.service';
import { Outlet } from '../../../../shared/models/outlet.model';
import { SharedService } from '../../../../shared/services/shared.service';
import { Customer } from '../../../../shared/models/customer.model';
import { User } from '../../../../shared/models/user.model';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { CommonConstants } from '../../../../shared/constants/common.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-add-loyalty-point',
  templateUrl: './add-loyalty-point.component.html',
  styleUrls: ['./add-loyalty-point.component.scss']
})
export class AddLoyaltyPointComponent implements OnInit {

  @Input() rewardPoint: number;
  @Input() currentCustomer: Customer;
  @Input() isBirthmonth: boolean;

  public userToken = '';
  public isConfirm = false;
  public currentOutlet = new Outlet();
  public currentUser = new User();
  public ipAddress = '';

  public pointValue = '0';
  public pointValueAdd = 0;
  public pointValueAddBonus = 0;
  public newBalance = 0;
  public totalPointAdd = 0;
  public customerName = '';
  public pointExchangeRate;
  public birthdayBonusExchangeRate;
  public isBirthdayBonusAvailable;
  public appSettings: any;
  public rewardsPointsTitle: string;

  constructor(private authService: AuthService,
    public activeModal: NgbActiveModal,
    private pointTransactionService: PointTransactionService,
    private sharedService: SharedService,
    private appSettingService: AppSettingService,
    private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService) {
    this.rewardPoint = 0;
    this.userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    this.getIpAddress();
  }

  ngOnInit() {
    this.setRateValueByAppSettingsOfStore();
    this.getRewardsPointsTitle();
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClickNextButton() {
    if (!this.pointExchangeRate || !this.birthdayBonusExchangeRate || this.isBirthdayBonusAvailable == null) {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning();
        this.activeModal.close();
      });
    } else {
      this.customerName = this.currentCustomer.firstName + ' ' + this.currentCustomer.lastName;
      if (this.isBirthdayBonusAvailable && this.isBirthmonth) {
        const convertedRateBonus = this.isBirthmonth ? this.pointExchangeRate * this.birthdayBonusExchangeRate : 0;
        this.pointValueAdd = Number(this.pointValue) * this.pointExchangeRate;
        this.pointValueAddBonus = convertedRateBonus !== 0 ? Number(this.pointValue) * convertedRateBonus - this.pointValueAdd : 0;
        this.totalPointAdd = this.pointValueAdd + this.pointValueAddBonus;
        this.newBalance = this.totalPointAdd + this.rewardPoint;
        this.isConfirm = true;
      } else {
        this.pointValueAdd = Number(this.pointValue) * this.pointExchangeRate;
        this.totalPointAdd = this.pointValueAdd + this.pointValueAddBonus;
        this.newBalance = this.totalPointAdd + this.rewardPoint;
        this.isConfirm = true;
      }
    }

  }

  onClickConfirmButton() {
    const request: AddPointRequest = {
      customerId: this.currentCustomer.id,
      outletId: this.currentOutlet.id,
      comment: '',
      value: this.pointValueAdd,
      userId: this.currentUser.id,
      ipAddress: this.ipAddress
    };
    const requestBonus: AddPointRequest = {
      customerId: this.currentCustomer.id,
      outletId: this.currentOutlet.id,
      comment: '',
      value: this.pointValueAddBonus,
      userId: this.currentUser.id,
      ipAddress: this.ipAddress
    };
    this.activeModal.close(this.newBalance);
    if (this.totalPointAdd <= 0) {
      this.translate.get('ERROR.adding-points-greater-zero').subscribe(message => {
        this.toast.error(message);
      });
    } else {
      const addPointTransaction$ = this.pointTransactionService.addPointTransaction(this.userToken, request);
      const addPointBonusTransaction$ = this.pointTransactionService.addPointTransaction(this.userToken, requestBonus);
      if (this.isBirthmonth && this.isBirthdayBonusAvailable) {
        addPointTransaction$.subscribe(res => {
          if (res < 0) {
            this.translate.get('ERROR.add-point-failed').subscribe(message => {
              this.toast.error(message);
            });
          } else {
            this.translate.get('MESSAGE.add-points-success').subscribe(message => {
              this.toast.success(message);
              this.sharedService.addBalanceSubscription.next(this.newBalance);
              addPointBonusTransaction$.subscribe(resBonus => {
                if (resBonus < 0) {
                  this.translate.get('ERROR.add-point-birthday-bonus-failed').subscribe(messageBonus => {
                    this.toast.error(message);
                  });
                } else {
                  this.sharedService.addBalanceSubscription.next(this.newBalance);
                  this.translate.get('MESSAGE.add-point-birthday-bonus-success').subscribe(messageBonus => {
                    this.toast.success(messageBonus);
                  });
                }
              }, error => {
                this.translate.get('ERROR.add-point-birthday-bonus-failed').subscribe(messageBonus => {
                  this.toast.error(message);
                });
              });
            });
          }
        }, error => {
          this.translate.get('ERROR.add-point-failed').subscribe(message => {
            this.toast.error(message);
          });
        });
      } else {
        this.pointTransactionService.addPointTransaction(this.userToken, request).subscribe((res: any) => {
          if (res < 0) {
            this.translate.get('ERROR.add-point-failed').subscribe(message => {
              this.toast.error(message);
            });
          } else {
            this.translate.get('MESSAGE.add-points-success').subscribe(message => {
              this.sharedService.addBalanceSubscription.next(this.newBalance);
              this.toast.success(message);
            });
          }
        }, error => {
          this.translate.get('ERROR.add-point-failed').subscribe(message => {
            this.toast.error(message);
          });
        });
      }
    }
  }

  checkCustomerBirthday(customer: Customer) {
    const monthCurrent = (new Date()).getUTCMonth();
    const monthBirthday = customer && customer.dateOfBirth && new Date(customer.dateOfBirth).getUTCMonth() ?
      new Date(customer.dateOfBirth).getUTCMonth() : -1;
    return monthBirthday === monthCurrent;
  }

  editAmount() {
    this.isConfirm = false;
  }

  public addNumber(value: any) {
    if (this.pointValue === '0') {
      this.pointValue = '';
    }
    this.pointValue = this.pointValue + '' + value;
  }

  public clearValue() {
    this.pointValue = '0';
  }

  public subNumber() {
    this.pointValue = this.pointValue.substr(0, (this.pointValue.length - 1));
    if (this.pointValue === '') {
      this.pointValue = '0';
    }
  }

  public isShowPointBirthdayBonus() {
    return this.isBirthdayBonusAvailable && this.isBirthmonth ? true : false;
  }

  public getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      this.ipAddress = ip;
    });
  }

  private setRateValueByAppSettingsOfStore() {
    this.appSettingService.getAppSettingsByType(CommonConstants.storeTypeAppsetting).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        const appSettingModelsByStoreType = res.appSettingModels;

        this.pointExchangeRate = appSettingModelsByStoreType.find(a => a.name === 'PointExchangeRate').value ?
          Number(appSettingModelsByStoreType.find(a => a.name === 'PointExchangeRate').value) : null;

        this.birthdayBonusExchangeRate = appSettingModelsByStoreType.find(a => a.name === 'BirthdayBonusExchangeRate').value ?
          Number(appSettingModelsByStoreType.find(a => a.name === 'BirthdayBonusExchangeRate').value) : null;

        this.isBirthdayBonusAvailable = appSettingModelsByStoreType.find(a => a.name === 'IsAutoBirthdayBonus').value ?
          (appSettingModelsByStoreType.find(a => a.name === 'IsAutoBirthdayBonus').value
            === 'true' ? true : false) : null;
      } else {
        this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
          this.toast.warning();
          this.activeModal.close();
        });
      }
    }, () => {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.warning();
        this.activeModal.close();
      });
    });
  }

  private getRewardsPointsTitle() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
    JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle));
    if (appSetting.value) {
      this.translate.get('ADD_LOYALTY_POINT.tog-points').subscribe(message => {
        this.rewardsPointsTitle =  appSetting.value + ' ' + message;
      });
    } else {
      this.rewardsPointsTitle = '';
    }
  }
}
