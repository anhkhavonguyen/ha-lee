import {Injectable} from '@angular/core';
import {AbstractControl, ValidatorFn} from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor() {
  }

  crossFieldValidator(targetText: string, toMatchText: string): ValidatorFn {
    return (check: AbstractControl): { [key: string]: boolean } | null => {
      const targetField = check.get(targetText);
      const confirmField = check.get(toMatchText);
      if (targetField.pristine || confirmField.pristine) {
        return null;
      }
      if (targetField.value === confirmField.value) {
        return null;
      }
      return {'notMatch': true};
    };
  }
}
