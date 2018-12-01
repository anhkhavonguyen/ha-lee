import { Component, OnInit, Input } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SetPasswordRequest } from 'src/app/containers/staff/change-password-staff/set-password-staff.model';
import { SetPasswordService } from 'src/app/containers/staff/change-password-staff/set-password-staff.service';
import { ToastrService } from 'ngx-toastr';

function passwordMatcher(check: AbstractControl) {
  const passwordControl = check.get('password');
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
  selector: 'app-set-password-staff',
  templateUrl: './set-password-staff.component.html',
  styleUrls: ['./set-password-staff.component.scss']
})
export class SetPasswordStaffComponent implements OnInit {

  setPasswordForm: FormGroup = new FormGroup({});
  @Input() email!: string;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private setPasswordService: SetPasswordService,
    private activeModal: NgbActiveModal,
    private toast: ToastrService
  ) { }

  ngOnInit() {
  this.setPasswordForm = this.formBuilder.group({
    passwordGroup: this.formBuilder.group({
      emailControl: '',
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: passwordMatcher })
  });
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onClickSetPasword() {
    const getPass = this.setPasswordForm.get('passwordGroup.password');
    const setPasswordRequest: SetPasswordRequest = {
      email: this.email,
      newPassword: getPass ? getPass.value : ''
    };

    this.setPasswordService.SetPassword(setPasswordRequest).subscribe((response: any) => {
      this.toast.success('Set password success!');
      this.onClose();
    }, err => {
      this.toast.error(err);
    });
  }

}
