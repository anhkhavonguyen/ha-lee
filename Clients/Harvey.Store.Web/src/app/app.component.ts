import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './auth/auth.service';
import { authConfig } from './auth/auth.config';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { TranslateService } from '@ngx-translate/core';
import { Title, DOCUMENT } from '@angular/platform-browser';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';

const StoreTypeAppsetting = 2;
const StoreAppTitleText = 'StoreAppTitle';
const StoreAppFaviconText = 'StoreAppFavicon';
const StoreAppLoginLogo = 'StoreAppLoginLogoImage';

const ContentTypeAppsetting = 6;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  title = 'app';
  public isAuthenticated = false;
  public loginSubcription = new Subscription();
  public defaultFavicon = 'favicon.ico';

  constructor(private router: Router,
    private authService: AuthService,
    private oauthService: OAuthService,
    private translate: TranslateService,
    private titleService: Title,
    private appSettingService: AppSettingService,
    @Inject(DOCUMENT) private document: HTMLDocument) {
    this.getInfoFromAppSetting();
    translate.addLangs(['en', 'fr']);
    translate.setDefaultLang('en');
    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');
    this.loginSubcription = this.authService.loginSubcription.subscribe(info => {
      this.isAuthenticated = this.authService.checkAuthenticated();
    });
  }

  ngOnInit() {
    this.isAuthenticated = this.authService.checkAuthenticated();
    const expiredToken = this.authService.getExpiredToken() !== '' ? +this.authService.getExpiredToken() : 0;
    const now = new Date();
    const expiredTokenAt = +this.authService.getExpiredTokenAt();
    const timeInterval = expiredToken - (expiredTokenAt - now.getTime());
    if (timeInterval > 0) {
      this.authService.setIntervalCheckValidToken(timeInterval);
    }
  }

  ngOnDestroy(): void {
    this.loginSubcription.unsubscribe();
  }

  private configureWithNewConfigApi() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }

  getInfoFromAppSetting() {
    this.appSettingService.getAppSettingsByType(StoreTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.setAppTitle(res.appSettingModels);
        this.setAppFavicon(res.appSettingModels);
        this.setAppSettingTrigger(res.appSettingModels);
      }
    });

    this.appSettingService.getAppSettingsByType(ContentTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingService.triggerAppSettingsDataByContentType(res.appSettingModels);
      }
    });
  }

  setAppTitle(appSettingModel: any) {
    if (appSettingModel) {
      const appTitle = appSettingModel.find((x: any) => x.name === StoreAppTitleText);
      if (appTitle && appTitle.value) {
        this.titleService.setTitle(appTitle.value);
      }
    }
  }

  setAppFavicon(appSettingModel: any) {
    const fav = this.document.getElementById('appFavicon');
    if (fav) {
      if (appSettingModel) {
        const appFavicon = appSettingModel.find((x: any) => x.name === StoreAppFaviconText);
        if (appFavicon && appFavicon.value) {
          fav.setAttribute('href', `data:image/jpg;base64,${appFavicon.value}`);
        } else {
          fav.setAttribute('href', this.defaultFavicon);
        }
      }
    }
  }

  setAppSettingTrigger(appSettingModel: any) {
    this.appSettingService.triggerAppSettingsData(appSettingModel);
  }
}
