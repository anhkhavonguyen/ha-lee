import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService, ResetPasswordRequest} from '../../auth.service';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from '@ngx-translate/core';
import {SharedService} from '../../../shared/services/shared.service';
import { Subscription } from 'rxjs';
import { AppSettingService } from 'src/app/shared/services/app-setting.service';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';


@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup = new FormGroup({});

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private authService: AuthService,
              private toast: ToastrService,
              private translate: TranslateService,
              private shareService: SharedService,
              private appSettingService: AppSettingService) {
  }

  ngOnInit() {
    this.resetPasswordForm = this.formBuilder.group({
      passwordGroup: this.formBuilder.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      }, {validator: this.shareService.crossFieldValidator('password', 'confirmPassword')})
    });
  }

  resetPassword() {
    const resetCode = this.route.snapshot.paramMap.get('code');
    const resetPasswordRequest: ResetPasswordRequest = {
      code: resetCode,
      password: this.resetPasswordForm.get('passwordGroup.password').value,
      confirmPassword: this.resetPasswordForm.get('passwordGroup.confirmPassword').value,
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue)
    };
    this.authService.resetPassword(resetPasswordRequest).subscribe((response: any) => {
      this.translate.get('MESSAGE.reset-password-success').subscribe(message => {
        this.toast.success(message);
      });
      this.router.navigate(['/auth/login']);
    }, () => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.success(message);
      });
    });
  }
}
