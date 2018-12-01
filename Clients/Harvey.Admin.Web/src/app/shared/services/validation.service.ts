import { Injectable } from '@angular/core';
import { ValidatePhoneModel } from 'src/app/containers/customer/edit-customer-info/validate-phone.model';
import { AbstractControl, ValidatorFn } from '@angular/forms';

@Injectable()
export class ValidationService {
    constructor() {
    }

    validatePhoneNumber(countryCodeText: string, phoneText: string, validatePhonesArray: Array<ValidatePhoneModel>): ValidatorFn {
        return (check: AbstractControl): { [key: string]: boolean } | null => {
            const phoneCountryCodeField = check.get(countryCodeText);
            const phoneNumberField = check.get(phoneText);
            let isValid = false;
            if (validatePhonesArray && phoneCountryCodeField && phoneNumberField) {
              validatePhonesArray.forEach(item => {
                if (item.countryCode === phoneCountryCodeField.value) {
                  isValid = new RegExp(item.regex).test(phoneNumberField.value);
                }
              });
            }
            return isValid ? null : {'invalidPhone': true};
        };
      }
}
