import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../shared/services/shared.service';
import { PageName, RoutingParamUser } from '../../shared/constants/routing.constant';
import { NewCustomerRequest } from '../../shared/models/customer.model';
import { UserService } from '../../shared/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Outlet } from '../../shared/models/outlet.model';
import { User } from '../../shared/models/user.model';
import { CustomerService } from '../../shared/services/customer.service';
import { AuthService } from '../../auth/auth.service';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { MembershipTransactionService } from '../../shared/services/membership-transaction.service';
import { AddMembershipCommand, MembershipActionType } from '../../shared/models/membershipTransaction.model';
import { RoleName } from '../../shared/constants/role.constant';
import { TranslateService } from '@ngx-translate/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-terms-and-privacy-page',
  templateUrl: './terms-and-privacy-page.component.html',
  styleUrls: ['./terms-and-privacy-page.component.scss']
})
export class TermsAndPrivacyPageComponent implements OnInit {

  public isJoin = false;
  public currentOutlet = new Outlet();
  public currentUser = new User();
  public countryCode = '';
  public phoneNumber = '';
  public userToken = '';
  public isLoading = false;
  public ipAddress = '';

  public userRoles = [];
  public appSettings: any;
  public appSetting: any;
  public termOfUseTitle: '';
  public termOfUseContent: '';
  public policyTitle: '';
  public policyContent: '';
  public agreeContent: any;

  constructor(private sharedService: SharedService,
    private userService: UserService,
    private activatedRoute: ActivatedRoute
    , private customerService: CustomerService,
    private authService: AuthService,
    private toast: ToastrService,
    private membershipTransactionService: MembershipTransactionService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService) {
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
    this.activatedRoute.queryParams.subscribe(param => {
      this.countryCode = param.countryCode;
      this.phoneNumber = param.phoneNumber;
    });
    this.getUserRoles();
  }

  ngOnInit() {
    this.checkAccessPage();
    this.getIpAddress();
    this.getTermPolicy();
    this.getAgreeContent();
  }

  checkAccessPage() {
    if (!this.currentOutlet) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
    if (!this.phoneNumber || !this.countryCode) {
      this.sharedService.routingToPage(PageName.SERVING_PAGE);
    }
    if (!this.currentUser) {
      this.authService.logout();
    }
  }

  onClickJoinButton() {
    this.initNewCustomer();
  }

  initNewCustomer() {
    this.isLoading = true;

    const newCustomerRequest: NewCustomerRequest = {
      phoneCountryCode: this.countryCode,
      phoneNumber: this.phoneNumber,
      originalUrl: environment.memberPageUrl,
      outletId: this.currentOutlet.id,
      outletName: this.currentOutlet.name,
      createdBy: this.currentUser.id,
      staffId: this.currentUser.id
    };

    this.customerService.initNewCustomer(this.userToken, newCustomerRequest).subscribe(result => {
      if (result) {
        const customerId = result;
        this.isLoading = false;
        this.translate.get('MESSAGE.create-new-customer-success').subscribe(message => {
          this.toast.success(message);
        });
        this.initBlankMembershipTransaction(customerId);
        const routingParam: RoutingParamUser = { customerId: customerId };
        this.sharedService.routingToPageWithParam(PageName.UPDATE_CUSTOMER_PROFILE_PAGE, routingParam);
      } else {
        this.isLoading = false;
        this.translate.get('ERROR.cant-create-new-customer').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      if (error.status > 204) {
        this.isLoading = false;
        this.translate.get('ERROR.cant-create-new-customer').subscribe(message => {
          this.toast.error(message);
        });
      }
    });

  }

  initBlankMembershipTransaction(customerId: string) {
    const basicType = '1';
    const request: AddMembershipCommand = {
      customerId: customerId,
      outletId: this.currentOutlet.id,
      userId: this.currentUser.id,
      comment: null,
      expiredDate: null,
      membershipTypeId: basicType,
      ipAddress: this.ipAddress,
      membershipActionType: MembershipActionType.New
    };
    this.membershipTransactionService.addMembershipTransaction(request).subscribe(res => res);
  }

  onClickBackButton() {
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickCheckboxJoin() {
    this.isJoin = !this.isJoin;
  }

  public getIpAddress() {
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      this.ipAddress = ip;
    });
  }

  private getUserRoles() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      if (!this.checkUserRoleToAccessPage()) {
        this.translate.get('ERROR.do-not-permission').subscribe(message => {
          this.toast.error(message);
        });
        this.authService.logout();
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

  getTermPolicy() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
    JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const termOfUse = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeContentTermOfUse));
    if (termOfUse) {
      this.termOfUseTitle = termOfUse.value.split('|||')[0];
      this.termOfUseContent = termOfUse.value.split('|||')[1];
    }

    const policy = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeContentPrivacyPolicy));
    if (policy) {
      this.policyTitle = policy.value.split('|||')[0];
      this.policyContent = policy.value.split('|||')[1];
    }
  }

  getAgreeContent() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
    JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const summaryTitle = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppSummaryLabelTitle)).value;
    const appTitle = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeHomeAppTitle)).value;
    if (summaryTitle && appTitle) {
      this.translate.get('TERMS_AND_PRIVACY.confirm-message').subscribe(message => {
        this.agreeContent = message;
        this.agreeContent = this.agreeContent.replace('{0}', summaryTitle);
        this.agreeContent = this.agreeContent.replace('{1}', appTitle);
      });
    } else {
      this.translate.get('TERMS_AND_PRIVACY.confirm-message').subscribe(message => {
        this.agreeContent = message;
        this.agreeContent = this.agreeContent.replace('{0}', 'TOG');
        this.agreeContent = this.agreeContent.replace('{1}', 'Toy Or Game');
      });
    }

  }
}
