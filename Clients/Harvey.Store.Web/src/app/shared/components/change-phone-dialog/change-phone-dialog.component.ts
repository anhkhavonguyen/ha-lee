import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { blankSpaceValidator, GeneralValidation } from '../../utils/validation.util';
import { ValidatePhoneModel } from '../../models/validate-phone.model';
import { Customer, ChangePhoneNumberRequest } from '../../models/customer.model';
import { User } from '../../models/user.model';
import { environment } from '../../../../environments/environment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-phone-dialog',
  templateUrl: './change-phone-dialog.component.html',
  styleUrls: ['./change-phone-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChangePhoneDialogComponent implements OnInit {

  @Input() validatePhones: Array<ValidatePhoneModel> = [];
  @Input() currentCustomer: Customer;
  @Input() currentUser: User;

  @Output() clickSaveEvent = new EventEmitter<ChangePhoneNumberRequest>();

  changeMobilePhoneNumberForm: FormGroup = new FormGroup({});

  constructor(private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    private toast: ToastrService) { }

  ngOnInit() {
    this.changeMobilePhoneNumberForm = this.formBuilder.group({
      phoneGroup: this.formBuilder.group({
        phoneCountryCode: [this.currentCustomer.phoneCountryCode],
        phoneNumber: [this.currentCustomer.phone,
        [Validators.required, blankSpaceValidator, Validators.pattern(GeneralValidation.NUMBER_REGEX),
        ]]
      }, { validator: this.validatePhoneNumber('phoneCountryCode', 'phoneNumber') }),
    });
  }

  validatePhoneNumber(countryCode: string, phone: string): ValidatorFn {
    return (check: AbstractControl): { [key: string]: boolean } | null => {
      const phoneCountryCodeField = check.get(countryCode);
      const phoneNumberField = check.get(phone);
      let isValid = false;
      if (this.validatePhones && phoneCountryCodeField && phoneNumberField) {
        this.validatePhones.forEach(item => {
          if (item.countryCode === phoneCountryCodeField.value) {
            isValid = new RegExp(item.regex).test(phoneNumberField.value);
          }
        });
      }
      return isValid ? null : { 'invalidPhone': true };
    };
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClickSaveButton() {
    const request: ChangePhoneNumberRequest = this.getRequest();
    if (request) {
      this.clickSaveEvent.emit(request);
    } else {
      this.translate.get('ERROR.nothing-change').subscribe(message => {
        this.toast.warning(message);
      });
    }
    this.activeModal.dismiss();
  }

  getRequest(): ChangePhoneNumberRequest | null {
    const controlCountryCode = this.changeMobilePhoneNumberForm.get('phoneGroup.phoneCountryCode');
    const controlPhoneNumber = this.changeMobilePhoneNumberForm.get('phoneGroup.phoneNumber');

    const newCountryCode = controlCountryCode ? controlCountryCode.value : null;
    const newPhoneNumber = controlPhoneNumber ? controlPhoneNumber.value.trim() : null;

    if (newCountryCode !== this.currentCustomer.phoneCountryCode || newPhoneNumber !== this.currentCustomer.phone) {
      const request: ChangePhoneNumberRequest = {
        customerId: this.currentCustomer.id,
        customerCode: this.currentCustomer.customerCode,
        updatedBy: this.currentUser.id,
        newPhoneCountryCode: newCountryCode,
        newPhoneNumber: newPhoneNumber,
        memberOriginalUrl: environment.memberPageUrl
      };
      return request;
    } else {
      return null;
    }
  }
}
