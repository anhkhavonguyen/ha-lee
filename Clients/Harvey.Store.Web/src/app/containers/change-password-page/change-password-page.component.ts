import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { SharedService } from '../../shared/services/shared.service';
import { Outlet } from '../../shared/models/outlet.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../shared/services/user.service';
import { PageName } from '../../shared/constants/routing.constant';
import { ToastrService } from 'ngx-toastr';
import { ChangePasswordRequest, User } from '../../shared/models/user.model';
import { RoleName } from '../../shared/constants/role.constant';
import { TranslateService } from '@ngx-translate/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../shared/constants/common.constant';

@Component({
  selector: 'app-change-password-page',
  templateUrl: './change-password-page.component.html',
  styleUrls: ['./change-password-page.component.scss']
})
export class ChangePasswordPageComponent implements OnInit {

  changePasswordForm: FormGroup = new FormGroup({});
  public currentOutlet = new Outlet();
  public currentUser = new User();
  public request = new ChangePasswordRequest();

  public nameRoleAdminStaff = RoleName.ROLE_ADMIN_STAFF;
  public userRoles = [];
  public userToken = '';

  public repeatPassword = '';
  public currentPassword = '';
  public newPassword = '';

  public isLoading = false;

  public appSettings: any;
  public outletAvatar: string;

  public panelColorProperties = {};

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private formBuilder: FormBuilder,
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
    this.currentOutlet = this.sharedService.getAnnounceOutlet();
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    this.userToken = this.authService.getTokenFormStorage();
    this.panelColorProperties = this.sharedService.getJsonValueAppsetting(CommonConstants.storePanelColor);
  }

  ngOnInit() {
    this.checkAccessPage();

    this.changePasswordForm = this.formBuilder.group({
      currentPasswordControl: ['', [Validators.required]],
      newPasswordControl: ['', [Validators.required]],
      repeatNewPasswordControl: ['', [Validators.required]]
    });

    this.outletAvatar = this.getOutletAvatarImage();
  }

  onClickHomeButton() {
    this.sharedService.routingToPage(PageName.HOME_PAGE);
  }

  onClickListCustomerButton() {
    this.sharedService.routingToPage(PageName.CUSTOMER_LISTING_PAGE);
  }

  onClickServingButton() {
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickTransactionsButton() {
    this.sharedService.routingToPage(PageName.TRANSACTIONS_HISTORY_PAGE);
  }

  onClickDashboardButton() {
    this.sharedService.routingToPage(PageName.DASHBOARD_PAGE);
  }

  onClickLogoutButton() {
    this.authService.logout();
  }

  onClickConfirmButton() {
    this.isLoading = true;
    if (!this.repeatPassword || !this.currentPassword || !this.newPassword) {
      this.translate.get('ERROR.fill-all').subscribe(message => {
        this.toast.error(message);
      });
      this.isLoading = false;
    } else {
      if (this.currentPassword === this.newPassword && this.newPassword !== this.repeatPassword) {
        this.translate.get('ERROR.must-diffent-current-password-&-confirm-new-password').subscribe(message => {
          this.toast.error(message);
        });
        this.isLoading = false;
      } else {
        if (this.currentPassword === this.newPassword) {
          this.translate.get('ERROR.must-different-current-password').subscribe(message => {
            this.toast.error(message);
          });
          this.isLoading = false;
        }
        if (this.newPassword !== this.repeatPassword) {
          this.translate.get('ERROR.must-confirm-new-password').subscribe(message => {
            this.toast.error(message);
          });
          this.isLoading = false;
        }
      }
      if (this.currentPassword !== this.newPassword && this.newPassword === this.repeatPassword) {
        this.request = { currentPassword: this.currentPassword, newPassword: this.newPassword };
        this.userService.changePassword(this.userToken, this.request).subscribe((res) => {
          this.isLoading = false;
          this.translate.get('MESSAGE.change-password-success').subscribe(message => {
            this.toast.success(message);
          });
          this.authService.logout();
        }, error => {
          this.isLoading = false;
          this.translate.get('ERROR.change-password-failed').subscribe(message => {
            this.toast.error(message);
          });
        });
      }
    }
  }

  checkAccessPage() {
    if (this.currentOutlet && this.currentUser) {
      this.checkRoleToAccess();
    } else {
      this.sharedService.routingToPage(PageName.HOME_PAGE);
    }
  }

  checkRoleToAccess() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      if (this.userRoles.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
        this.sharedService.routingToPage(PageName.HOME_PAGE);
      }
    });
  }

  checkPermissionCurrentUser(roleName: string) {
    if (this.userRoles.indexOf(roleName) !== -1) {
      return true;
    }
    return false;
  }

  private getOutletAvatarImage() {
    this.appSettings = this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;

    const appSetting = this.appSettings.appSettingModels.find(x => (x.name === CommonConstants.storeAppHeaderLogoImage));
    if (appSetting.value) {
      return `data:image/jpg;base64,${appSetting.value}`;
    }
    return '';
  }
}
