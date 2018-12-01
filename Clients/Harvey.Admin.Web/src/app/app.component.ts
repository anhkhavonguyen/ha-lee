import { Component, Inject } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { AppTranslateService } from 'src/app/shared/services/translate.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { OnInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { OAuthService, JwksValidationHandler, OAuthEvent } from 'angular-oauth2-oidc';
import { authConfig } from 'src/app/auth/auth.config';
import { UserService } from 'src/app/shared/services/user.service';
import { Title, DOCUMENT } from '@angular/platform-browser';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { AppSettingModel } from 'src/app/shared/models/app-settings.model';
import { element } from 'protractor';

const AdminTypeAppsetting = 1;
const AdminAppTitleText = 'AdminAppTitle';
const AdminAppFaviconText = 'AdminAppFavicon';

const StoreTypeAppsetting = 2;
const ContentTypeAppsetting = 6;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(
    private authService: AuthService,
    private appTranslateService: AppTranslateService,
    private oauthService: OAuthService,
    private userService: UserService,
    private titleService: Title,
    private appSettingService: AppSettingsService,
    @Inject(DOCUMENT) private document: HTMLDocument) {
    this.getInfoFromAppSetting();
    this.appTranslateService.setup();
    this.loginSubcription = this.authService.loginSubcription.subscribe(info => {
      this.isAuthenticated = info;
      this.getUserName();
    });
  }

  title = 'app';
  public isAuthenticated = false;
  public loginSubcription = new Subscription();
  public userName = '';
  public defaultFivicon = 'harvey_icon.ico';

  ngOnDestroy(): void {
    this.loginSubcription.unsubscribe();
  }

  ngOnInit() {
    this.isAuthenticated = this.authService.checkAuthenticated();
    this.oauthConfigure();
    const expiredToken = this.authService.getExpiredToken() !== '' ? +this.authService.getExpiredToken() : 0;
    const now = new Date();
    const expiredTokenAt = +this.authService.getExpiredTokenAt();
    const timeInterval = expiredToken - (expiredTokenAt - now.getTime());
    if (timeInterval > 0) {
      this.authService.setIntervalCheckValidToken(timeInterval);
    }
    this.getUserName();
  }

  private oauthConfigure() {
    this.oauthService.configure(authConfig);
    this.oauthService.setStorage(localStorage);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
  }

  private getUserName() {
    if (this.isAuthenticated) {
      this.userService.getUserProfile().subscribe(res => {
        this.userName = `${res.firstName} ${res.lastName}`;
      });
    }
  }

  getInfoFromAppSetting() {
    this.appSettingService.getAppSettingsByType(AdminTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.setAppTitle(res.appSettingModels);
        this.setAppFavicon(res.appSettingModels);
        this.appSettingService.triggerAppSettingsData(res.appSettingModels);
      }
    });

    this.appSettingService.getAppSettingsByType(ContentTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingService.triggerAppSettingsDataByContentType(res.appSettingModels);
      }
    });

    this.appSettingService.getAppSettingsByType(StoreTypeAppsetting).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingService.triggerAppSettingsDataByStoreType(res.appSettingModels);
      }
    });
  }

  setAppTitle(appSettingModel: any) {
    if (appSettingModel) {
      const appTitle = appSettingModel.find((x: any) => x.name === AdminAppTitleText);
      if (appTitle && appTitle.value) {
        this.titleService.setTitle(appTitle.value);
      }
    }
  }

  setAppFavicon(appSettingModel: any) {
    const fav = this.document.getElementById('appFavicon');
    if (fav) {
      if (appSettingModel) {
        const appFavicon = appSettingModel.find((x: any) => x.name === AdminAppFaviconText);
        if (appFavicon && appFavicon.value) {
           fav.setAttribute('href', `data:image/jpg;base64,${appFavicon.value}`);
        } else {
          fav.setAttribute('href', this.defaultFivicon);
        }
      }
    }
  }
}
