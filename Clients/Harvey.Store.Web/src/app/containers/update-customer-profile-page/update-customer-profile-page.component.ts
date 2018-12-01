import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageName, RoutingParamUser } from '../../shared/constants/routing.constant';
import { SharedService } from '../../shared/services/shared.service';
import { ActivatedRoute } from '@angular/router';
import { UpdateCustomerProfileRequest, Customer, Gender } from '../../shared/models/customer.model';
import { User } from '../../shared/models/user.model';
import { AuthService } from '../../auth/auth.service';
import { CustomerService } from '../../shared/services/customer.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-customer-profile-page',
  templateUrl: './update-customer-profile-page.component.html',
  styleUrls: ['./update-customer-profile-page.component.scss']
})
export class UpdateCustomerProfilePageComponent implements OnInit {

  public currentUser = new User();
  public currentCustomer = new Customer();

  public staffImage = '/assets/img/tog_profile_icon.png';
  public togLogo = '/assets/img/300_dpi_black.png';

  updateProfileForm: FormGroup = new FormGroup({});
  customerProfile: any;
  private password = '';
  public isConfirmUpdate = false;
  public customerId;

  constructor(private formBuilder: FormBuilder,
    private translate: TranslateService,
    private sharedService: SharedService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private customerService: CustomerService,
    private toast: ToastrService) {
    if (!this.sharedService.checkExpToken()) {
      this.authService.logout();
    } else {
      this.initPage();
    }
  }

  initPage() {
    this.currentUser = this.sharedService.getAnnounceCurrentUser();
    if (!this.currentUser) {
      this.authService.logout();
    } else {
      this.activatedRoute.queryParams.subscribe(param => {
        this.customerId = param.customerId;
        const userToken = this.authService.getTokenFormStorage();
        this.customerService.getByCustomerId(userToken, this.customerId).subscribe(res => {
          if (res) {
            this.currentCustomer = res;
          } else {
            this.translate.get('ERROR.something-wrong').subscribe(message => {
              this.toast.error(message);
            });
          }
        });
      }, error => {
        this.translate.get('ERROR.something-wrong').subscribe(message => {
          this.toast.error(message);
        });
      });
    }
  }

  ngOnInit() {
    this.updateProfileForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+')]],
      dateOfBirth: '',
      genderGroup: this.formBuilder.group({
        gender: ''
      }),
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onClickNextButton() {
    this.customerProfile = this.updateProfileForm.value;
    this.password = this.customerProfile.password;
    this.isConfirmUpdate = !this.isConfirmUpdate;
  }

  onClickConfirmProfileButton() {
    this.updateCustomerProfile();
  }

  onClickSkipButton() {
    const routingParam: RoutingParamUser = { customerId: this.customerId };
    this.sharedService.routingToPageWithParam(PageName.CUSTOMER_PROFILE_PAGE, routingParam);
  }

  onClickBack() {
    this.isConfirmUpdate = !this.isConfirmUpdate;
    this.password = null;
    this.updateProfileForm.patchValue({
      firstName: this.customerProfile.firstName,
      lastName: this.customerProfile.lastName,
      email: this.customerProfile.email,
      dateOfBirth: this.customerProfile.dateOfBirth,
      gender: this.customerProfile.genderGroup.gender,
      password: ''
    });
  }

  updateCustomerProfile() {
    const birthday = this.updateProfileForm.get('dateOfBirth').value;
    const nowTime = new Date(Date.now());
    const request: UpdateCustomerProfileRequest = {
      customerId: this.customerId,
      customerCode: this.currentCustomer.customerCode,
      dateOfBirth: new Date(birthday.year, birthday.month - 1, birthday.day, 23, 59, 59, 999),
      email: this.customerProfile.email,
      firstName: this.customerProfile.firstName,
      lastName: this.customerProfile.lastName,
      password: this.password,
      staffId: this.currentUser.id,
      updatedDate: nowTime,
      gender: this.customerProfile.genderGroup.gender ?
        (this.customerProfile.genderGroup.gender === 'Male' ? Gender.Male : Gender.Female) : null
    };

    this.customerService.updateCustomerProfile(request).subscribe(res => {
      this.translate.get('MESSAGE.customer-profile-updated-success').subscribe(message => {
        this.toast.success(message);
        setTimeout(() => {
          const routingParam: RoutingParamUser = { customerId: this.customerId };
          this.sharedService.routingToPageWithParam(PageName.CUSTOMER_PROFILE_PAGE, routingParam);
        }, 500);
      });
    }, error => {
      this.translate.get('ERROR.customer-profile-updated-failed').subscribe(message => {
        this.toast.error(message);
        const routingParam: RoutingParamUser = { customerId: this.customerId };
        this.sharedService.routingToPageWithParam(PageName.CUSTOMER_PROFILE_PAGE, routingParam);
      });
    });

  }
}
