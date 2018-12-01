import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ChangePasswordRequest } from 'src/app/shared/components/change-password/change-password.model';
import { ChangePasswordService } from 'src/app/shared/components/change-password/change-password.service';
import { ToastrService } from 'ngx-toastr';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpService } from 'src/app/shared/services/http.service';

function passwordMatcher(check: AbstractControl) {
  const passwordControl = check.get('newPassword');
  const passwordConfirmControl = check.get('confirmPassword');

  if (passwordControl && passwordControl.pristine || passwordConfirmControl && passwordConfirmControl.pristine) {
    return null;
  }
  if (passwordControl && passwordConfirmControl && passwordControl.value === passwordConfirmControl.value) {
    return null;
  }
  return {
    'match': true
  };
}

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private changePasswordService: ChangePasswordService,
    private toast: ToastrService,
    private activeModal: NgbActiveModal,
    private httpService: HttpService
  ) {
    this.httpService.getLoadingState().subscribe(e => {
      this.isLoading = e;
    });
  }

  changePasswordForm: FormGroup = new FormGroup({});
  isLoading = false;

  ngOnInit() {
    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      newPasswordGroup: this.formBuilder.group({
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      }, {validator: passwordMatcher})
    });
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onClickChangePasword() {
    this.isLoading = true;
    const getPass = this.changePasswordForm.get('currentPassword');
    const getNewPass = this.changePasswordForm.get('newPasswordGroup.newPassword');
    const changePasswordRequest: ChangePasswordRequest = {
      currentPassword: getPass ? getPass.value : '',
      newPassword: getNewPass ? getNewPass.value : ''
    };

    this.changePasswordService.ChangePassword(changePasswordRequest).subscribe((response: any) => {
      this.toast.success('Change password success!');
      this.isLoading = false;
      this.onClose();
    }, err => {
      this.isLoading = false;
      this.toast.error(err);
    });
  }
}
