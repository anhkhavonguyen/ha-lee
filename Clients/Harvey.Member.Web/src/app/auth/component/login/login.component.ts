import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, ForgotPasswordRequest, UserLoginModel } from '../../auth.service';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { PhoneVerifiedResponse } from '../../../shared/models/user-profile.model';
import { MessageCode } from '../../../shared/models/page-message.model';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AppSettingService } from '../../../shared/services/app-setting.service';
import { PhoneValidatorModal } from '../../../shared/models/phone-validator.modal';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  enterPhoneForm: FormGroup = new FormGroup({});
  enterPasswordForm: FormGroup = new FormGroup({});
  isEnterPhoneView = true;
  isEnterPassView = false;
  isVerifyNumber = false;
  isLogin = false;
  listPhoneValidator: Array<PhoneValidatorModal> = [];
  private getPhoneRules = 4;
  private userPhoneNumber: '';
  private phoneCountryCode: '';
  private phoneVerifyResponse: PhoneVerifiedResponse;
  private messageCode: MessageCode;
  private appSettingMemberApp = 3;
  private appSettings: any;
  mainLogo = '';
  private defaultMainLogo = '../../../../assets/img/logo/300_dpi_black.png';

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private oauthService: OAuthService,
    private authStorage: OAuthStorage,
    private toast: ToastrService,
    private translate: TranslateService,
    private appSettingService: AppSettingService) {

  }

  ngOnInit() {
    this.appSettingService.getAppSettingByType(this.getPhoneRules).subscribe((response: any) => {
      if (response && response.appSettingModels && response.appSettingModels.length) {
        this.listPhoneValidator = this.appSettingService.getValidatePhone(response['appSettingModels']);
      } else {
        this.translate.get('ERROR.try-again').subscribe(res => {
          this.toast.error(res);
        });
      }
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(res => {
        this.toast.error(res);
      });
    });

    this.appSettingService.getAppSettingByType(this.appSettingMemberApp).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettings = res;
        this.mainLogo = this.appSettings.appSettingModels ?
          `data:image/png;base64,${this.appSettings.appSettingModels.find(a => a.name === 'MemberAppLoginLogoImage').value}` :
          this.defaultMainLogo;
      } else {
        this.translate.get('ERROR.try-again').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
      });
    });

    this.enterPhoneForm = this.formBuilder.group({
      countryCode: '65',
      phoneNum: ['', [Validators.required]],
    });

    this.enterPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required]],
    });
  }

  verifyPhoneNumber() {
    this.isVerifyNumber = true;
    this.phoneCountryCode = this.enterPhoneForm.get('countryCode').value;
    this.userPhoneNumber = this.enterPhoneForm.get('phoneNum').value;
    if (this.appSettingService.checkPhoneRegex(this.listPhoneValidator, this.phoneCountryCode, this.userPhoneNumber)) {
      this.authService.verifyPhoneNumber(this.phoneCountryCode, this.userPhoneNumber)
        .subscribe((response: any) => {
          this.phoneVerifyResponse = response;
          this.isVerifyNumber = false;
          if (this.phoneVerifyResponse) {
            if (this.phoneVerifyResponse.isMigrateData
              || (this.phoneVerifyResponse.emailConfirmed
                && this.phoneVerifyResponse.phoneNumberConfirmed
                && this.phoneVerifyResponse.passwordHash == null)) {
              const reNewPasswordRequest: ForgotPasswordRequest = {
                userName: this.phoneCountryCode + this.userPhoneNumber,
                originalUrl: location.origin,
                brandName: this.appSettingService.getTitleFromAppSettingsData(
                  this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.BrandTitleValue),
                acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
                  this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue)
              };
              this.authService.forgotPassword(reNewPasswordRequest).subscribe((res: any) => {
                this.messageCode = MessageCode.renewPasswordForMigrateUser;
                this.router.navigate(['/auth/messages', this.messageCode]);
              });
            }
            if (!this.phoneVerifyResponse.isMigrateData &&
              this.phoneVerifyResponse.emailConfirmed &&
              this.phoneVerifyResponse.phoneNumberConfirmed &&
              this.phoneVerifyResponse.passwordHash != null) {
              this.isEnterPassView = true;
              this.isEnterPhoneView = false;
            }
            if (!(this.phoneVerifyResponse.emailConfirmed && this.phoneVerifyResponse.phoneNumberConfirmed)) {
              this.messageCode = MessageCode.userNotUpdateProfile;
              this.router.navigate(['/auth/messages', this.messageCode]);
            }
          } else {
            // const requestInitNewAccount: InitNewAccountRequest = {
            //   phoneCountryCode: this.phoneCountryCode,
            //   phoneNumber: this.userPhoneNumber,
            //   originalUrl: location.origin
            // };
            // this.authService.initNewAccount(requestInitNewAccount).subscribe((response: any) => {
            this.messageCode = MessageCode.newUserRegistration;
            this.router.navigate(['/auth/messages', this.messageCode]);
            // });
          }
        }, () => {
          this.translate.get('ERROR.try-again').subscribe(message => {
            this.toast.error(message);
          });
          this.isVerifyNumber = false;
        });
    } else {
      this.translate.get('ERROR.phone-regex').subscribe(message => {
        this.toast.error(message);
      });
      this.isVerifyNumber = false;
    }

  }

  enterPhoneNumber() {
    this.isEnterPhoneView = true;
    this.isEnterPassView = false;
    this.isVerifyNumber = false;
  }

  signIn() {
    this.isLogin = true;
    const userPassword = this.enterPasswordForm.get('password').value;
    const signInRequest: UserLoginModel = {
      phoneNumber: this.phoneCountryCode + this.userPhoneNumber,
      password: userPassword
    };

    this.authService.getToken(signInRequest).subscribe((response: any) => {
      this.authStorage.setItem('access_token', response.access_token);
      this.authStorage.setItem('expires_at', (new Date().getTime() + (1000 * response.expires_in)).toString());
      this.isLogin = false;
      if (this.oauthService.hasValidAccessToken()) {
        this.router.navigate(['app/my-account']);
      }
    }, () => {
      this.translate.get('ERROR.wrong-password').subscribe(message => {
        this.toast.error(message);
      });
      this.isLogin = false;
    });

  }

  forgotPassword() {
    const reNewPasswordRequest: ForgotPasswordRequest = {
      userName: this.phoneCountryCode + this.userPhoneNumber,
      originalUrl: location.origin,
      brandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.BrandTitleValue),
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue)
    };
    this.authService.forgotPassword(reNewPasswordRequest).subscribe((res: any) => {
      this.messageCode = MessageCode.forgotPassword;
      this.router.navigate(['/auth/messages', this.messageCode]);
    });
  }
}
