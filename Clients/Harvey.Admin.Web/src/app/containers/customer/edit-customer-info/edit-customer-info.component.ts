import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { NgbModal, NgbActiveModal, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, Validators, FormBuilder, AbstractControl, ValidatorFn } from '@angular/forms';
import { GeneralValidation, blankSpaceValidator } from 'src/app/shared/utils/validation.util';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { ValidatePhoneModel } from 'src/app/containers/customer/edit-customer-info/validate-phone.model';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { ValidationService } from 'src/app/shared/services/validation.service';
import { ChangeCustomerMobileRequest, Fields, ChangeCustomerInfoRequest, Gender } from './edit-customer-info.model';
import { Customer } from 'src/app/containers/customer/customer.model';
import { environment } from 'src/environments/environment';
import { Output } from '@angular/core';
import * as moment from 'moment';
import { NgbDateCustomParserFormatter } from '../../../shared/helper/ngb-datepicker-formatter';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-edit-customer-info',
  templateUrl: './edit-customer-info.component.html',
  styleUrls: ['./edit-customer-info.component.scss'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ],
})
export class EditCustomerInfoComponent implements OnInit {

  @Input() validatePhones: Array<ValidatePhoneModel> = [];
  @Input() customer = new Customer();
  @Output() clickSaveEvent = new EventEmitter<ChangeCustomerInfoRequest>();
  isUpdating = false;

  editCustomerInfoForm: FormGroup = new FormGroup({});
  changingFiledsArray: Array<number> = [];

  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private appSettingService: AppSettingsService,
    private toast: ToastrService,
    private translate: TranslateService,
    private validationService: ValidationService
  ) {
  }

  ngOnInit() {

    this.editCustomerInfoForm = this.formBuilder.group({
      email: ['', [Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+')]],
      dateOfBirth: '',
      postalCode: '',
      genderGroup: this.formBuilder.group({
        gender: ''
      }),
      nameGroup: this.formBuilder.group({
        lastName: [''],
        firstName: ['']
      }),
      phoneGroup: this.formBuilder.group({
        phoneCountryCode: [''],
        phoneNumber: ['', [Validators.required, blankSpaceValidator, Validators.pattern(GeneralValidation.NUMBER_REGEX)]]
      }, { validator: this.validationService.validatePhoneNumber('phoneCountryCode', 'phoneNumber', this.validatePhones) }),
    });

    if (this.customer) {
      const currentBirthday = this.customer.dateOfBirth !== '-' ?
        new Date(moment.utc(this.customer.dateOfBirth).local().format('LL')) : null;
      const birthday = currentBirthday ?
        { year: currentBirthday.getFullYear(), month: currentBirthday.getMonth() + 1, day: currentBirthday.getDate() } : null;

      this.editCustomerInfoForm.patchValue({
        email: this.customer.email,
        dateOfBirth: birthday,
        postalCode: this.customer.zipCode,
        genderGroup: {
          gender: this.customer.gender
        },
        nameGroup: { lastName: this.customer.lastName, firstName: this.customer.firstName },
        phoneGroup: { phoneCountryCode: this.customer.phoneCountryCode, phoneNumber: this.customer.phoneNumber }
      });
    }
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClickSaveBtn(e: any) {
    this.isUpdating = true;
    const request: ChangeCustomerInfoRequest | null = this.getRequest();
    if (request) {
      this.clickSaveEvent.emit(request);
    } else {
      this.translate.get('APP.CUSTOMER_DETAIL_COMPONENT.NOTHING_CHANGE').subscribe(message => {
        this.toast.warning(message);
      });
    }
    this.isUpdating = false;
    this.activeModal.dismiss();
  }

  getRequest(): ChangeCustomerInfoRequest | null {
    let isChanging = false;

    const controlFirstName = this.editCustomerInfoForm.get('nameGroup.firstName');
    const controlLastName = this.editCustomerInfoForm.get('nameGroup.lastName');
    const controlEmail = this.editCustomerInfoForm.get('email');
    const controlBirthday = this.editCustomerInfoForm.get('dateOfBirth');
    const controlGender = this.editCustomerInfoForm.get('genderGroup.gender');
    const controlCountryCode = this.editCustomerInfoForm.get('phoneGroup.phoneCountryCode');
    const controlPhoneNumber = this.editCustomerInfoForm.get('phoneGroup.phoneNumber');
    const controlPostalCode = this.editCustomerInfoForm.get('postalCode');

    const newFirstName = controlFirstName ?
      (controlFirstName.value ? (controlFirstName.value.trim() !== '' ? controlFirstName.value.trim() : null) : null) : null;
    const newLastName = controlLastName ?
      (controlLastName.value ? (controlLastName.value.trim() !== '' ? controlLastName.value.trim() : null) : null) : null;
    const newEmail = controlEmail ?
      (controlEmail.value ? (controlEmail.value.trim() !== '' ? controlEmail.value.trim() : null) : null) : null;
    const newCountryCode = controlCountryCode ? controlCountryCode.value : null;
    const newPhoneNumber = controlPhoneNumber ? controlPhoneNumber.value.trim() : null;
    const newGender = controlGender ?
      (controlGender.value === 'Male' ? Gender.Male : (controlGender.value === 'Female' ? Gender.Female : null)) : null;
    const newPostalCode = controlPostalCode ?
      (controlPostalCode.value ? (controlPostalCode.value.trim() ? controlPostalCode.value.trim() : null) : null) : null;

    const tempCurrentBirthday = this.customer.dateOfBirth !== '-' ?
      new Date(moment.utc(this.customer.dateOfBirth).local().format('MM/DD/YYYY HH:mm:ss')) : null;
    const currentBirthday = tempCurrentBirthday ? new Date(
      tempCurrentBirthday.getFullYear(), tempCurrentBirthday.getMonth(), tempCurrentBirthday.getDate(), 23, 59, 59).toDateString() : null;

    const controlBirthdayValue = controlBirthday ? controlBirthday.value : null;
    const tempNewBirthday = controlBirthdayValue ?
      new Date(controlBirthdayValue.year, controlBirthdayValue.month - 1, controlBirthdayValue.day, 23, 59, 59).toDateString() : null;
    const newBirthday = tempNewBirthday !== currentBirthday ? tempNewBirthday : currentBirthday;

    const compareNewGender = newGender === Gender.Female ?
      Gender[Gender.Female] : (newGender === Gender.Male ? Gender[Gender.Male] : null);

    if (newLastName !== this.customer.lastName
      || newFirstName !== this.customer.firstName
      || newEmail !== this.customer.email
      || newPhoneNumber !== this.customer.phoneNumber
      || compareNewGender !== this.customer.gender
      || newCountryCode !== this.customer.phoneCountryCode
      || newPostalCode !== this.customer.zipCode
      || tempNewBirthday !== currentBirthday) {
      isChanging = true;
    }

    if (isChanging) {
      const request: ChangeCustomerInfoRequest = new ChangeCustomerInfoRequest();
      request.customerId = this.customer.id;
      request.customerCode = this.customer.customerCode;
      request.memberOriginalUrl = environment.memberPageUrl;
      request.firstName = newFirstName;
      request.lastName = newLastName;
      request.email = newEmail;
      request.newPhoneCountryCode = newCountryCode;
      request.newPhoneNumber = newPhoneNumber;
      request.gender = newGender;
      request.dateOfBirth = newBirthday;
      request.postalCode = newPostalCode;
      request.acronymBrandName = this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue);
      return request;
    }

    return null;
  }
}
