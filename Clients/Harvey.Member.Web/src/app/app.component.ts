import { Component, Inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { authConfig } from './auth/auth.authen-config';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { Title, DOCUMENT } from '@angular/platform-browser';

const MemberTypeAppsetting = 3;
const MemberAppTitleText = 'MemberAppTitle';
const MemberAppFaviconText = 'MemberAppFavicon';

const ContentTypeAppsetting = 6;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(
    private translate: TranslateService,
    private oauthService: OAuthService,
    private router: Router,
    private titleService: Title,
    private appSettingService: AppSettingService,
    @Inject(DOCUMENT) private document: HTMLDocument) {
    this.getInfoFromAppSetting();
    translate.addLangs(['en', 'fr']);
    translate.setDefaultLang('en');
    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');
    this.configureWithNewConfigApi();
  }

  public defaultFavicon = 'favicon.ico';

  private configureWithNewConfigApi() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
  }

  getInfoFromAppSetting() {
    this.appSettingService.getAppSettingByType(MemberTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.setAppTitle(res.appSettingModels);
        this.setAppFavicon(res.appSettingModels);
      }
    });

    this.appSettingService.getAppSettingByType(ContentTypeAppsetting).subscribe((res: any) => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettingService.triggerAppSettingsDataByContentType(res.appSettingModels);
      }
    });
  }

  setAppTitle(appSettingModel: any) {
    if (appSettingModel) {
      const appTitle = appSettingModel.find((x: any) => x.name === MemberAppTitleText);
      if (appTitle && appTitle.value) {
        this.titleService.setTitle(appTitle.value);
      }
    }
  }

  setAppFavicon(appSettingModel: any) {
    const fav = this.document.getElementById('appFavicon');
    if (fav) {
      if (appSettingModel) {
        const appFavicon = appSettingModel.find((x: any) => x.name === MemberAppFaviconText);
        if (appFavicon && appFavicon.value) {
           fav.setAttribute('href', `data:image/jpg;base64,${appFavicon.value}`);
        } else {
          fav.setAttribute('href', this.defaultFavicon);
        }
      }
    }
  }

}
