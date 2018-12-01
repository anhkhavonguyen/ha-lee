import { Component, OnInit } from '@angular/core';
import { Outlet } from '../../shared/models/outlet.model';
import { AuthService } from '../../auth/auth.service';
import { SharedService } from '../../shared/services/shared.service';
import { User } from '../../shared/models/user.model';
import { UserService } from '../../shared/services/user.service';
import { PageName } from '../../shared/constants/routing.constant';
import { AppSettingService } from '../../shared/services/app-setting.service';
import { AppSettingModel } from '../../shared/models/app-setting.model';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../shared/constants/common.constant';
import { ToastrService } from 'ngx-toastr';
import { RoleName } from '../../shared/constants/role.constant';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  public appSettings: Array<AppSettingModel> = [];
  public listOutlet = new Array<Outlet>();
  public currentUser = new User();
  public userToken = '';

  public isLoading = true;

  private userRoles = [];
  public appSettingsDataSubcription: Subscription;
  public outletImage: string;
  public titleApp: string;

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private userService: UserService,
    private appSettingService: AppSettingService,
    private oAuthStorage: OAuthStorage,
    private toast: ToastrService,
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
    this.getUserRoles();
  }

  ngOnInit() {
    this.checkAccessPage();
    if (this.appSettingService.appSettingData) {
      this.outletImage = this.getOutletDefaultImage(this.appSettingService.appSettingData);
      this.titleApp = this.getTitleApp(this.appSettingService.appSettingData);
    }
    this.appSettingsDataSubcription = this.appSettingService.appSettingsDataByStoreTypeSubject.subscribe(data => {
      this.outletImage = this.getOutletDefaultImage(data);
      this.titleApp = this.getTitleApp(data);
    });
  }

  checkAccessPage() {
    if (!this.currentUser) {
      this.authService.logout();
    }
  }

  private getUserRoles() {
    this.userService.getUserRole(this.userToken).subscribe(res => {
      this.userRoles = res.roles;
      if (!this.checkUserRoleToAccessPage()) {
        this.translate.get('ERROR.do-not-permission').subscribe(message => {
          this.toast.error(message);
          this.authService.logout();
        });
      } else {
        this.getListOutlets();
      }
    }, () => {
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
        this.authService.logout();
      });
    });
  }

  checkUserRoleToAccessPage() {
    if (this.userRoles.indexOf(RoleName.ROLE_STAFF) === -1 && this.userRoles.indexOf(RoleName.ROLE_ADMIN_STAFF) === -1) {
      return false;
    } else {
      return true;
    }
  }

  getListOutlets() {
    this.isLoading = true;
    this.userService.getOutletByStaff(this.userToken, this.currentUser.id)
      .subscribe(response => {
        this.listOutlet = response.outletModels;
        this.getAppSettings();
      }, () => {
        this.translate.get('ERROR.cant-get-list-outlets').subscribe(message => {
          this.isLoading = false;
          this.toast.error(message);
        });
      });
  }

  getAppSettings() {
    this.appSettingService.getAppSettings(this.userToken).subscribe(data => {
      if (data) {
        this.appSettings = data;
        this.oAuthStorage.setItem(CommonConstants.appSettings, JSON.stringify(this.appSettings));
        this.isLoading = false;
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

  onClickInfoCard(outlet: Outlet) {
    this.sharedService.setAnnounceOutlet(outlet);
    this.sharedService.routingToPage(PageName.SERVING_PAGE);
  }

  onClickBackToLogin() {
    this.authService.logout();
  }

  getOutletDefaultImage(appSettings: any): string {
    if (appSettings) {
      const appSetting = appSettings.find(x => (x.name === CommonConstants.storeAppOutletLogoImage));
      if (appSetting.value) {
        return `data:image/jpg;base64,${appSetting.value}`;
      }
      return '';
    }
  }

  getTitleApp(appSettings: any): string {
    if (appSettings) {
      const appSetting = appSettings.find(x => (x.name === CommonConstants.storeHomeAppTitle));
      if (appSetting.value) {
        return appSetting.value;
      }
      return '';
    }
  }
}
