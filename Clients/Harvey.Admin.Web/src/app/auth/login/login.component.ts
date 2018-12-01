import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/shared/models/user.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { authConfig } from 'src/app/auth/auth.config';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { ToastrService } from 'ngx-toastr';
import { AppSettingsService } from '../../shared/services/app-settings.service';
import { AppSettingLabelConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  constructor(private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private toast: ToastrService,
    private appSettingsService: AppSettingsService) {
    this.loginFailSubcription = this.authService.isFailLoginSubcription.subscribe(data => {
      this.isLoginFail = true;
      this.error = data;
      this.isLoading = false;
    });

    this.loginSubcription = this.authService.loginSubcription.subscribe(info => {
      this.isLoading = false;
    });

    this.loginLogoUrl = this.getLoginLogoUrl(this.appSettingsService.appSettingsDataByAdminType);
    this.appSettingsDataSubcription = this.appSettingsService.appSettingsDataByAdminTypeSubject.subscribe(data => {
      this.loginLogoUrl = this.getLoginLogoUrl(data);
    });
  }

  loginForm: FormGroup = new FormGroup({});
  public user = new User();
  public isLoginFail = false;
  public error = '';
  public isAuthenticated = false;
  public isLoading = false;
  public loginLogoUrl = '';
  public loginFailSubcription: Subscription;
  public loginSubcription = new Subscription();
  public appSettingsDataSubcription = new Subscription();

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      emailControl: ['', [Validators.required]],
      passwordControl: ['', [Validators.required]]
    });
    this.isAuthenticated = this.authService.checkAuthenticated();
    const urlPage = this.authService.getPreviousUrlBeforeLogging();
    this.navigatePage(urlPage);
  }

  ngOnDestroy(): void {
    this.loginFailSubcription.unsubscribe();
    this.appSettingsDataSubcription.unsubscribe();
  }

  public login() {
    this.isLoading = true;
    this.authService.login(this.user);
  }

  private navigatePage(urlPage: string) {
    return this.isAuthenticated ? this.router.navigateByUrl(urlPage) : this.router.navigate([authConfig.loginUrl]);
  }

  private getLoginLogoUrl(appSettingsData: any) {
    if (appSettingsData && Array.isArray(appSettingsData)) {
      const appLoginLogo = appSettingsData.find((x: any) => x.name === AppSettingLabelConstants.AdminAppLoginLogoImageText);
      if (appLoginLogo && appLoginLogo.value) {
        return `data:image/jpg;base64,${appLoginLogo.value}`;
      }
    }

    return '';
  }
}
