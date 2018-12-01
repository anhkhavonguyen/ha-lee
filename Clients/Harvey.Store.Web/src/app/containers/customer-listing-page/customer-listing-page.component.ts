import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { SharedService } from '../../shared/services/shared.service';
import { Outlet } from '../../shared/models/outlet.model';
import { PageName } from '../../shared/constants/routing.constant';
import { User } from '../../shared/models/user.model';
import { Customer, CustomersRequest } from '../../shared/models/customer.model';
import { CustomerService } from '../../shared/services/customer.service';
import { fromEvent } from 'rxjs';
import { UserService } from '../../shared/services/user.service';
import { RoleName } from '../../shared/constants/role.constant';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { CommonConstants } from '../../shared/constants/common.constant';
import { AppSettingService } from '../../shared/services/app-setting.service';
import { OAuthStorage } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-customer-listing-page',
  templateUrl: './customer-listing-page.component.html',
  styleUrls: ['./customer-listing-page.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerListingPageComponent implements OnInit {

  public currentOutlet = new Outlet();
  public currentUser = new User();
  public userToken = '';

  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public customerList: Array<Customer> = [];
  public loadingIndicator = true;
  public searchText = '';
  public userRoles = [];
  public outletAvatar: string;
  private appSettings: any;

  public nameAdminStaffRole = RoleName.ROLE_ADMIN_STAFF;

  public panelColorProperties = {};

  @ViewChild('searchInput') searchInput: ElementRef;

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private customerService: CustomerService,
    private userService: UserService,
    private toast: ToastrService,
    private oAuthStorage: OAuthStorage,
    private translate: TranslateService) {
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.initPage();
    }
  }

  initPage() {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
    this.userToken = this.authService.getTokenFormStorage();
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
  }

  ngOnInit() {
    this.checkAccessPage();

    this.onSearch();
    this.addKeyUpEventToSearchText();
    this.outletAvatar = this.getOutletAvatarImage();
  }

  checkAccessPage() {
    if (this.currentOutlet && this.currentUser) {
      this.checkRoleToAccess();
    }
    if (!this.currentOutlet) {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
    if (!this.currentUser) {
      this.authService.logout();
    }
  }

  checkRoleToAccess() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      if (this.userRoles.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
        this.sharedService.routingToPage(PageName.HOME_PAGE);
      }
    }, () => {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  checkPermissionCurrentUser(roleName: string) {
    if (this.userRoles.indexOf(roleName) !== -1) {
      return true;
    }
    return false;
  }

  onClickHomePageButton() {
    this.sharedService.routingToPage(PageName.HOME_PAGE);
  }

  onClickServingButton() {
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickTransactionsButton() {
    this.sharedService.routingToPage(PageName.TRANSACTIONS_HISTORY_PAGE);
  }

  onClickChangePasswordButton() {
    this.sharedService.routingToPage(PageName.CHANGE_PASSWORD_PAGE);
  }

  onClickDashboardButton() {
    this.sharedService.routingToPage(PageName.DASHBOARD_PAGE);
  }

  onClickLogoutButton() {
    this.authService.logout();
  }

  public loadCustomers(request: CustomersRequest) {
    this.customerService.getCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;
      this.loadingIndicator = false;
      this.customerList = temp.customerListResponse.map(result => {
        const customerModel = Customer.buildCustomer(result);
        return customerModel;
      });
    }, () => {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  setPage(pageInfo) {
    const request: CustomersRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      searchText: this.searchText,
    };
    this.loadCustomers(request);
  }

  onSearch() {
    this.setPage({ offset: 0 });
  }

  addKeyUpEventToSearchText() {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .subscribe(() => {
        this.onSearch();
      });
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
