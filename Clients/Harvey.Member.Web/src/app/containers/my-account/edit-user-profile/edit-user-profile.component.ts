import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ChangePasswordRequest, ChangePinRequest, MyAccountService, UpdateUserProfileRequest } from '../my-account.service';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { Gender, UserProfileModel } from '../../../shared/models/user-profile.model';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { SharedService } from '../../../shared/services/shared.service';

@Component({
  selector: 'app-edit-user-profile',
  templateUrl: './edit-user-profile.component.html',
  styleUrls: ['./edit-user-profile.component.scss']
})
export class EditUserProfileComponent implements OnInit {
  userProfileForm: FormGroup = new FormGroup({});
  changePasswordForm: FormGroup = new FormGroup({});
  changePinForm: FormGroup = new FormGroup({});
  isUpdateProfile = false;
  isChangePassword = false;
  isChangePin = false;
  imgConvert = '';
  avatarImg = '';
  isLoading = true;
  isSendPin = false;
  defaultPicture = '/assets/img/icon/tog_profile_icon.png';
  displayPicture = '';
  private userProfile: UserProfileModel = new UserProfileModel();
  isEditDateOfBirth = false;
  checkExistMember = false;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private accountService: MyAccountService,
    private oauthStorage: OAuthStorage,
    private oauthService: OAuthService,
    private datePipe: DatePipe,
    private toast: ToastrService,
    private translate: TranslateService,
    private shareSerivce: SharedService) {
    if (!this.oauthService.hasValidAccessToken()) {
      this.oauthStorage.removeItem('access_token');
      this.oauthStorage.removeItem('expires_at');
      this.toast.warning('Your session has timed out. Please sign in again');
      this.router.navigate(['/auth/login']);
    }
  }

  ngOnInit() {
    this.userProfileForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+')]],
      genderGroup: this.formBuilder.group({
        gender: ''
      }),
      dateOfBirth: ['', Validators.required],
      postalCode: '',
    });

    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      newPasswordGroup: this.formBuilder.group({
        newPassword: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      }, { validator: this.shareSerivce.crossFieldValidator('newPassword', 'confirmPassword') })
    });

    this.changePinForm = this.formBuilder.group({
      currentPin: ['', [Validators.required, Validators.minLength(4)]],
      newPinGroup: this.formBuilder.group({
        newPin: ['', [Validators.required, Validators.minLength(4)]],
        confirmNewPin: ['', [Validators.required, Validators.minLength(4)]],
      }, { validator: this.shareSerivce.crossFieldValidator('newPin', 'confirmNewPin') })
    });

    this.accountService.getUserProfile().subscribe((response: any) => {
      if (!response) {
        this.checkExistMember = false;
        this.isLoading = false;
        return;
      } else {
        this.checkExistMember = true;
        this.userProfile = response;
        this.avatarImg = this.userProfile.avatar ? this.userProfile.avatar : '';
        this.displayPicture = this.userProfile.avatar ? `data:image/jpg;base64,${this.userProfile.avatar}` : this.defaultPicture;
        const date = this.userProfile.dateOfBirth ? new Date(this.userProfile.dateOfBirth) : null;
        this.isEditDateOfBirth = this.userProfile.dateOfBirth ? false : true;
        this.userProfileForm.patchValue({
          firstName: this.userProfile.firstName,
          lastName: this.userProfile.lastName,
          email: this.userProfile.email,
          genderGroup: { gender: this.userProfile.gender != null ? (this.userProfile.gender === Gender.Female ? 'Female' : 'Male') : '' },
          dateOfBirth: date ? { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() } : null,
          postalCode: this.userProfile.zipCode
        });
        this.isLoading = false;
      }
    });
  }

  cancel() {
    this.router.navigate(['/app/my-account']);
  }

  back() {
    this.router.navigate(['/app/my-account']);
  }

  changePassword() {
    this.isLoading = true;
    this.isChangePassword = true;
    const request: ChangePasswordRequest = {
      currentPassword: this.changePasswordForm.get('currentPassword').value,
      newPassword: this.changePasswordForm.get('newPasswordGroup.newPassword').value
    };

    this.accountService.changePassword(request).subscribe((response: any) => {
      this.translate.get('MESSAGE.update-password-success').subscribe(message => {
        this.toast.success(message);
        this.changePasswordForm.reset();
        this.isChangePassword = false;
        this.isLoading = false;
      });
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
        this.isChangePassword = false;
        this.isLoading = false;
      });
    });
  }

  changePIN() {
    this.isLoading = true;
    this.isChangePin = true;
    const request: ChangePinRequest = {
      oldPin: this.changePinForm.get('currentPin').value,
      newPin: this.changePinForm.get('newPinGroup.newPin').value,
      confirmPin: this.changePinForm.get('newPinGroup.confirmNewPin').value
    };
    this.accountService.changePin(request).subscribe((response: any) => {
      this.translate.get('MESSAGE.update-pin-success').subscribe(message => {
        this.toast.success(message);
        this.changePinForm.reset();
        this.isChangePin = false;
        this.isLoading = false;
      });
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
        this.isChangePin = false;
        this.isLoading = false;
      });
    });
  }

  senPINToSMS() {
    this.isLoading = true;
    this.isSendPin = true;
    this.accountService.sendPINToPhone(this.userProfile.phoneCountryCode, this.userProfile.phoneNumber)
      .subscribe((response: any) => {
        this.translate.get('MESSAGE.send-PIN-to-Phone').subscribe(message => {
          this.toast.success(message);
          this.isLoading = false;
        });
      }, error => {
        this.translate.get('MESSAGE.try-again').subscribe(message => {
          this.toast.success(message);
          this.isLoading = false;
        });
      });
  }

  updateProfile() {
    this.isLoading = true;
    this.isUpdateProfile = true;
    const date = this.userProfileForm.get('dateOfBirth').value;
    const updateRequest: UpdateUserProfileRequest = {
      firstName: this.userProfileForm.get('firstName').value,
      lastName: this.userProfileForm.get('lastName').value,
      email: this.userProfileForm.get('email').value,
      gender: this.userProfileForm.get('genderGroup.gender').value ?
        (this.userProfileForm.get('genderGroup.gender').value === 'Female' ? Gender.Female : Gender.Male) : null,
      zipCode: this.userProfileForm.get('postalCode').value,
      avatar: this.avatarImg,
      dateOfBirth: new Date(date.year, date.month - 1, date.day, 23, 59, 59, 999),
    };
    this.accountService.updateProfile(updateRequest).subscribe((response: any) => {
      this.translate.get('MESSAGE.update-profile-success').subscribe(message => {
        this.toast.success(message);
        this.isUpdateProfile = false;
        this.isLoading = false;
        this.isEditDateOfBirth = date ? false : true;
      });
    });
  }

  handleFileSelect(fileSelected) {
    const imgFile = fileSelected.target.files;
    const file = imgFile[0];
    const fileSize = file.size;
    const fileType = file.type;
    const allowedSize = 2097152;
    const allowedType = 'image';
    if (imgFile && file) {
      if (fileSize <= allowedSize && fileType.includes(allowedType)) {
        const reader = new FileReader();
        reader.onload = this.converFiletToBase64.bind(this);
        reader.readAsBinaryString(file);
      } else {
        if (fileSize > allowedSize) {
          this.translate.get('ERROR.LIMIT_FILE_SIZE').subscribe(result => {
            this.toast.error(result);
          });
        }
        if (!fileType.includes(allowedType)) {
          this.translate.get('ERROR.ALLOWED_FILE_TYPE').subscribe(result => {
            this.toast.error(result);
          });
        }
      }
    }
  }

  removeProfilePicture() {
    this.displayPicture = this.defaultPicture;
    this.avatarImg = '';
  }

  private converFiletToBase64(readerEvent) {
    const binaryString = readerEvent.target.result;
    this.imgConvert = btoa(binaryString);
    this.avatarImg = this.imgConvert;
    this.displayPicture = `data:image/jpg;base64,${this.avatarImg}`;
  }
}
