import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, ForgotPasswordRequest } from '../../auth.service';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup = new FormGroup({});

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private appSettingService: AppSettingService) {
  }

  ngOnInit() {
    this.forgotPasswordForm = this.formBuilder.group({
      countryCode: '65',
      phoneNum: ['', [Validators.required]],
    });
  }

  backToSignIn() {
    this.router.navigate(['auth/login']);
  }

  forgotPassword() {
    const phoneNum = this.forgotPasswordForm.get('countryCode').value + this.forgotPasswordForm.get('phoneNum').value;
    const forgotRequest: ForgotPasswordRequest = {
      userName: phoneNum,
      originalUrl: location.origin,
      brandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.BrandTitleValue),
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue)
    };
    this.authService.forgotPassword(forgotRequest).subscribe((response: any) => {
      window.alert('Please check your email to reset your password');
    });
  }
}
