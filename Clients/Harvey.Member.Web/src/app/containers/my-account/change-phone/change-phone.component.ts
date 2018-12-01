import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserProfileModel } from '../../../shared/models/user-profile.model';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { ChangePhoneNumberRequest, MyAccountService } from '../my-account.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { MessageCode } from '../../../shared/models/page-message.model';
import { AppSettingService } from '../../../shared/services/app-setting.service';
import { PhoneValidatorModal } from '../../../shared/models/phone-validator.modal';

enum DisplayView {
  enterPhone,
  confirm
}

@Component({
  selector: 'app-change-phone',
  templateUrl: './change-phone.component.html',
  styleUrls: ['./change-phone.component.scss']
})


export class ChangePhoneComponent implements OnInit {
  changePhoneForm: FormGroup = new FormGroup({});
  confirmPINForm: FormGroup = new FormGroup({});
  isUpdatePhone = false;
  userProfile: UserProfileModel = new UserProfileModel();
  listPhoneValidator: Array<PhoneValidatorModal> = [];
  displayView = DisplayView;
  currentView = DisplayView.enterPhone;
  isLoading = true;
  private userCurrentNumber = '';
  private currentNumber = '';
  private newPhoneCode = '';
  private newPhoneNumber = '';
  private messageCode = '';
  private getPhoneRules = 4;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private oauthService: OAuthService,
    private oauthStorage: OAuthStorage,
    private accountService: MyAccountService,
    private toast: ToastrService,
    private translate: TranslateService,
    private appSettingSerivce: AppSettingService) {
    if (!this.oauthService.hasValidAccessToken()) {
      this.oauthStorage.removeItem('access_token');
      this.oauthStorage.removeItem('expires_at');
      this.toast.warning('Your session has timed out. Please sign in again');
      this.router.navigate(['/auth/login']);
    }
  }

  ngOnInit() {
    this.appSettingSerivce.getAppSettingByType(this.getPhoneRules).subscribe((response: any) => {
      if (response && response.appSettingModels && response.appSettingModels.length) {
        this.listPhoneValidator = this.appSettingSerivce.getValidatePhone(response['appSettingModels']);
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
    this.accountService.getUserProfile().subscribe((response: any) => {
      this.userProfile = response;
      this.isLoading = false;
    });
    this.changePhoneForm = this.formBuilder.group({
      currentCode: '65',
      currentNumber: ['', Validators.required],
      newCode: '65',
      newNumber: ['', Validators.required]
    });
    this.confirmPINForm = this.formBuilder.group({
      inputPIN: ['', [Validators.required, Validators.minLength(4)]]
    });
  }

  backToAccount() {
    this.router.navigate(['/app/my-account']);
  }

  enterPIN() {
    this.userCurrentNumber = this.userProfile.phoneCountryCode + this.userProfile.phoneNumber;
    this.currentNumber = this.changePhoneForm.get('currentCode').value + this.changePhoneForm.get('currentNumber').value;
    const currentCode = this.changePhoneForm.get('currentCode').value;
    const currentPhone = this.changePhoneForm.get('currentNumber').value;
    this.newPhoneCode = this.changePhoneForm.get('newCode').value;
    this.newPhoneNumber = this.changePhoneForm.get('newNumber').value;
    if (this.appSettingSerivce.checkPhoneRegex(this.listPhoneValidator, currentCode, currentPhone) &&
      this.appSettingSerivce.checkPhoneRegex(this.listPhoneValidator, this.newPhoneCode, this.newPhoneNumber)) {
      if (this.currentNumber !== this.userCurrentNumber) {
        this.currentView = this.displayView.enterPhone;
        this.translate.get('ERROR.current-number-wrong').subscribe(message => {
          this.toast.error(message);
        });
      }
      if ((this.currentNumber === this.userCurrentNumber) && (this.currentNumber === (this.newPhoneCode + this.newPhoneNumber))) {
        this.currentView = this.displayView.enterPhone;
        this.translate.get('ERROR.same-phone-number').subscribe(message => {
          this.toast.error(message);
          this.currentView = this.displayView.enterPhone;
        });
      }
      if ((this.currentNumber === this.userCurrentNumber) && (this.currentNumber !== (this.newPhoneCode + this.newPhoneNumber))) {
        this.currentView = this.displayView.confirm;
      }
    } else {
      this.translate.get('ERROR.phone-regex').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  UpdatePhone() {
    this.isUpdatePhone = true;
    const pinCode = this.confirmPINForm.get('inputPIN').value;
    const changePhoneRequest: ChangePhoneNumberRequest = {
      pin: pinCode,
      phoneCountryCode: this.newPhoneCode,
      phoneNumber: this.newPhoneNumber
    };
    this.accountService.changePhoneNumber(changePhoneRequest).subscribe(() => {
      this.isUpdatePhone = false;
      this.oauthStorage.removeItem('access_token');
      this.messageCode = MessageCode.changePhoneNumber;
      this.router.navigate(['/auth/messages', this.messageCode]);
    }, () => {
      this.translate.get('ERROR.input-wrong-pin-code').subscribe(message => {
        this.toast.error(message);
      });
      this.isUpdatePhone = false;
    });
  }

  backToChangePhoneView() {
    this.currentView = this.displayView.enterPhone;
  }
}

