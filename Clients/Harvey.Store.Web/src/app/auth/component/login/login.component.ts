import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { authConfig } from '../../auth.config';
import { UserLogin } from '../../../shared/models/user.model';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
import { AccountService } from '../../../shared/services/account.service';
import { AppSettingService } from '../../../shared/services/app-setting.service';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { CommonConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

  loginForm: FormGroup = new FormGroup({});
  public user = new UserLogin();
  public isLoginFail = false;
  public isAuthenticated = false;
  public redirectUrl = authConfig.redirectUri;
  public error = '';
  public validTimeForLogin = true;
  public dateTimeNow = null;
  public startWorkingTime = null;
  public endWorkingTime = null;
  public appSettings: any;
  public isLoading = false;
  public timeZoneValue = null;
  public loginLogo: string;
  public appSettingsDataSubcription: Subscription;

  constructor(private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private toast: ToastrService,
    private accountService: AccountService,
    private appSettingService: AppSettingService,
    private translate: TranslateService) {
    this.authService.announceLoginError.subscribe(res => {
      this.error = res;
      this.isLoading = false;
    });
  }

  ngOnInit() {
    this.getDateTimeNow();
    this.getWorkingTime();

    this.loginForm = this.formBuilder.group({
      usernameControl: ['', [Validators.required]],
      passwordControl: ['', [Validators.required]]
    });

    this.loginLogo = this.getLoginLogo(this.appSettingService.appSettingData);
    this.appSettingsDataSubcription = this.appSettingService.appSettingsDataByStoreTypeSubject.subscribe(data => {
      this.loginLogo = this.getLoginLogo(data);
    });
    this.isAuthenticated = this.authService.checkAuthenticated();
    this.redirectUrl = this.authService.getPreviousUrlBeforeLogging();
    return this.isAuthenticated ? this.router.navigateByUrl(this.redirectUrl) : this.router.navigate([authConfig.issuer]);
  }

  ngOnDestroy() {
    this.appSettingsDataSubcription.unsubscribe();
  }

  public onClickLoginButton() {
    this.isLoading = true;
    if (!this.startWorkingTime || !this.endWorkingTime) {
      this.isLoading = false;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    }
    if (this.isValidTimeForLogin()) {
      this.authService.login(this.user);
    } else {
      this.isLoading = false;
    }
  }

  private getDateTimeNow() {
    this.accountService.getTime().subscribe(res => {
      this.dateTimeNow = moment(res).utc().toDate();
    });
  }

  private getTimeZone(offset) {
    const value = Math.abs(offset);
    return (Math.floor(value / 60));
  }

  private getWorkingTime() {
    const workingTimeType = 5;
    const timeZoneOffset = new Date().getTimezoneOffset();
    this.timeZoneValue = timeZoneOffset < 0 ? +this.getTimeZone(timeZoneOffset) : -this.getTimeZone(timeZoneOffset);

    this.appSettingService.getAppSettingsByType(workingTimeType).subscribe(res => {
      if (res) {
        this.appSettings = res;
        this.appSettings.appSettingModels.forEach(element => {
          if (element.groupName === 'WorkingTime' && element.name === 'StartWorkingTime') {
            this.startWorkingTime = moment(element.value, ['h:mm A']).utc().toDate();
          }
          if (element.groupName === 'WorkingTime' && element.name === 'EndWorkingTime') {
            this.endWorkingTime = moment(element.value, ['h:mm A']).utc().toDate();
          }
        });
      } else {
        this.isLoading = false;
        this.translate.get('ERROR.something-wrong').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      this.isLoading = false;
      this.translate.get('ERROR.something-wrong').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  private isValidTimeForLogin() {
    if (this.dateTimeNow.getTime().valueOf() + this.timeZoneValue < this.startWorkingTime.getTime().valueOf() + this.timeZoneValue) {
      this.validTimeForLogin = false;
      return false;
    }

    if (this.dateTimeNow.getTime().valueOf() + this.timeZoneValue > this.endWorkingTime.getTime().valueOf() + this.timeZoneValue) {
      this.validTimeForLogin = false;
      return false;
    }
    return true;
  }

  getLoginLogo(appSettingData: any) {
    if (appSettingData && Array.isArray(appSettingData)) {
        const app = appSettingData.find((x: any) => x.name === CommonConstants.storeAppLoginLogoImage);
        if (app.value) {
          return `data:image/jpg;base64,${app.value}`;
        }
        return '';
    }
    return '';
  }
}
