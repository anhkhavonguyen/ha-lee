import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, RegNewAccountRequest } from '../../auth.service';
import { Gender } from '../../../shared/models/user-profile.model';
import { MessageCode } from '../../../shared/models/page-message.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  signUpForm: FormGroup = new FormGroup({});
  confirmProfileForm: FormGroup = new FormGroup({});
  isConfirmProfile = false;
  userProfile: any;
  private password = '';
  private messageCode: MessageCode;
  public isSignUp;
  public isLoading = true;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private translate: TranslateService) {
    this.checkSignUpLinkIsValid();
  }

  checkSignUpLinkIsValid() {
    this.isLoading = true;
    this.route.params.subscribe(params => {
      this.authService.checkSignUpLink(params['code']).subscribe(res => {
        this.isLoading = false;
        if (!res.isValidLink) {
          this.isSignUp = true;
        } else {
          this.isSignUp = false;
        }
      }, error => {
        this.isLoading = false;
      });
    });
  }

  ngOnInit() {
    this.signUpForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required]],
      dateOfBirth: '',
      genderGroup: this.formBuilder.group({
        gender: ''
      }),
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.confirmProfileForm = this.formBuilder.group({
      firstName: '',
      lastName: '',
      email: '',
      dateOfBirth: '',
      gender: ''
    });

  }

  nextToConfirmProfile() {
    this.isConfirmProfile = true;
    this.userProfile = this.signUpForm.value;
    this.confirmProfileForm.patchValue({
      firstName: this.userProfile.firstName,
      lastName: this.userProfile.lastName,
      email: this.userProfile.email,
      dateOfBirth: this.userProfile.dateOfBirth,
      gender: this.userProfile.genderGroup.gender
    });
    this.password = this.userProfile.password;
  }

  registerNewAccount() {
    const regCode = this.route.snapshot.paramMap.get('code');
    const date = this.signUpForm.get('dateOfBirth').value;
    const regAccountRequest: RegNewAccountRequest = {
      code: regCode,
      firstName: this.confirmProfileForm.get('firstName').value,
      lastName: this.confirmProfileForm.get('lastName').value,
      email: this.confirmProfileForm.get('email').value,
      dateOfBirth: new Date(date.year, date.month - 1, date.day, 23, 59, 59, 999),
      password: this.password,
      gender: this.confirmProfileForm.get('gender').value ?
        (this.confirmProfileForm.get('gender').value === 'Male' ? Gender.Male : Gender.Female) : null,
      avatar: ''
    };
    this.authService.regNewAccount(regAccountRequest).subscribe((response: any) => {
      this.messageCode = MessageCode.signUp;
      this.router.navigate(['/auth/messages', this.messageCode]);
    }, error => {
      this.translate.get('ERROR.try-again').subscribe();
    });
  }

  backToUserProfile() {
    this.isConfirmProfile = false;
    this.signUpForm.patchValue({
      firstName: this.userProfile.firstName,
      lastName: this.userProfile.lastName,
      email: this.userProfile.email,
      dateOfBirth: this.userProfile.dateOfBirth,
      gender: this.userProfile.gender,
      password: ''
    });
  }

  backToSignIn() {
    this.router.navigate(['/auth/login']);
  }

  isCustomerSignUp() {
    if (!this.isLoading && this.isSignUp) {
      return true;
    }
    if (!this.isLoading && !this.isSignUp) {
      return false;
    }
    return false;
  }
}

