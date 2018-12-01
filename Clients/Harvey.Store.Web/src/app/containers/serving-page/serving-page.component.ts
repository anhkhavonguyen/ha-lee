import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { SharedService } from '../../shared/services/shared.service';
import { Outlet } from '../../shared/models/outlet.model';
import { User } from '../../shared/models/user.model';
import { CustomerService } from '../../shared/services/customer.service';
import { CheckValidCustomerServing } from '../../shared/models/customer.model';
import { PageName, RoutingParamPhone, RoutingParamUser } from '../../shared/constants/routing.constant';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../shared/services/user.service';
import { RoleName } from '../../shared/constants/role.constant';
import { AppSettingService } from '../../shared/services/app-setting.service';
import { ValidatePhoneModel } from '../../shared/models/validate-phone.model';
import { TranslateService } from '@ngx-translate/core';
import { CommonConstants } from '../../shared/constants/common.constant';
import { OAuthStorage } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-serving-page',
  templateUrl: './serving-page.component.html',
  styleUrls: ['./serving-page.component.scss']
})
export class ServingPageComponent implements OnInit {

  @ViewChild('numPad') numPad;
  public phoneNumber = '';
  public countryCode = '65';
  public currentOutlet = new Outlet();
  public currentUser = new User();
  public userToken = '';
  public validatePhones: Array<ValidatePhoneModel> = [];
  private appSettings: any;
  public isLoading = true;
  public outletAvatar: string;
  private idTypeRoleAppsetting = 4;

  private userRoles = [];
  public roleAdminStaff = RoleName.ROLE_ADMIN_STAFF;

  public panelColorProperties = {};

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private customerService: CustomerService,
    private toast: ToastrService,
    private userService: UserService,
    private appSettingService: AppSettingService,
    private translate: TranslateService,
    private oAuthStorage: OAuthStorage) {
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.initPage();
    }
  }

  initPage() {
    this.userToken = this.authService.getTokenFormStorage();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
    this.getUserRoles();
  }

  ngOnInit() {
    this.checkAccessPage();
    this.outletAvatar = this.getOutletAvatarImage();
  }

  @HostListener('window:keydown)', ['$event'])
  keysEvent(event) {
    if (event.key === 'Enter') {
      this.onClickLoginButton();
    }
  }

  pasteEvent($event): void {
    this.phoneNumber = $event.clipboardData.getData('text/plain');
  }

  checkAccessPage() {
    if (!this.currentUser) {
      this.authService.logout();
    }
    if (!this.currentOutlet) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
  }

  private getUserRoles() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      if (!this.checkUserRoleToAccessPage()) {
        this.translate.get('ERROR.do-not-permission').subscribe(message => {
          this.toast.error(message);
        });
        this.authService.logout();
      } else {
        this.getListValidatePhone();
      }
    }, error => {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
      this.authService.logout();
    });
  }

  checkUserRoleToAccessPage() {
    if (this.userRoles.indexOf(RoleName.ROLE_STAFF) === -1 && this.userRoles.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
      return false;
    } else {
      return true;
    }
  }

  checkPermissionForButton(role: string) {
    if (this.userRoles.indexOf(role) === -1) {
      return false;
    } else {
      return true;
    }
  }

  getListValidatePhone() {
    this.appSettingService.getAppSettingsByType(this.idTypeRoleAppsetting).subscribe(result => {
      this.validatePhones = this.sharedService.getValidatePhone(result);
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  getPhoneNumber(_phoneNumber: string) {
    this.phoneNumber = _phoneNumber;
  }

  onClickHomeButton() {
    this.sharedService.routingToPage(PageName.HOME_PAGE);
  }

  onClickListCustomerButton() {
    this.sharedService.routingToPage(PageName.CUSTOMER_LISTING_PAGE);
  }

  onClickChangePasswordButton() {
    this.sharedService.routingToPage(PageName.CHANGE_PASSWORD_PAGE);
  }

  onClickDashboardButton() {
    this.sharedService.routingToPage(PageName.DASHBOARD_PAGE);
  }

  onChooseCountryCode(_countryCode: string) {
    this.countryCode = _countryCode;
  }

  onClickLogoutButton() {
    this.authService.logout();
  }

  onClickTransactionsButton() {
    this.sharedService.routingToPage(PageName.TRANSACTIONS_HISTORY_PAGE);
  }

  onClickLoginButton() {
    const isVaildPhone = this.validatePhoneNumber(this.phoneNumber, this.countryCode);
    if (this.phoneNumber && this.countryCode && isVaildPhone) {
      this.isLoading = true;
      this.getNewAppSettings();
    } else {
      this.translate.get('ERROR.invalid-phone').subscribe(message => {
        this.toast.error(message);
        this.clearInputPhoneNumber();
      });
    }
  }

  getNewAppSettings() {
    this.oAuthStorage.removeItem(CommonConstants.appSettings);
    this.appSettingService.getAppSettings(this.userToken).subscribe(res => {
      if (res) {
        this.oAuthStorage.setItem(CommonConstants.appSettings, JSON.stringify(res));
        this.checkCustomerInfo();
      } else {
        this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
          this.toast.error(message);
          this.authService.logout();
        });
      }
    }, error => {
      this.translate.get('ERROR.cant-get-app-setting').subscribe(message => {
        this.toast.error(message);
        this.authService.logout();
      });
    });
  }

  clearInputPhoneNumber() {
    this.numPad.clearPhoneNumber();
    this.phoneNumber = '';
    this.countryCode = '65';
  }

  checkCustomerInfo() {
    const request: CheckValidCustomerServing = {
      countryCode: this.countryCode,
      phone: this.phoneNumber,
      staffId: this.currentUser.id,
      isServing: true,
      outletId: this.currentOutlet.id
    };
    this.customerService.checkValidCustomerServing(request).subscribe(res => {
      this.isLoading = false;
      if (res) {
        const routingParam: RoutingParamUser = { customerId: res.id };
        this.sharedService.routingToPageWithParam(PageName.CUSTOMER_PROFILE_PAGE, routingParam);
      } else {
        const routingParam: RoutingParamPhone = { countryCode: this.countryCode, phoneNumber: this.phoneNumber };
        this.sharedService.routingToPageWithParam(PageName.TERMS_AND_PRIVACY_PAGE, routingParam);
      }
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  validatePhoneNumber(phone: string, countryCode: string) {
    let isVaild = false;
    this.validatePhones.forEach(item => {
      if (item.countryCode === countryCode) {
        isVaild = new RegExp(item.regex).test(phone);
      }
    });
    return isVaild;
  }

  getOutletAvatarImage() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppHeaderLogoImage));
    if (appSetting.value) {
      return `data:image/jpg;base64,${appSetting.value}`;
    }
    return '';
  }
}
