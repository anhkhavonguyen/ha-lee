import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable, Subject} from 'rxjs';
import {PhoneValidatorModal} from '../models/phone-validator.modal';

@Injectable({
  providedIn: 'root'
})
export class AppSettingService {
  constructor(private http: HttpClient) {
  }

  public appSettingsDataByContentType: any;
  public appSettingsDataByContentTypeSubject = new Subject<any>();

  getAppSettingByType(type: number): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/AppSettings/getsbytype?type=${type}`;
    return this.http.get(apiURL);
  }

  getValidatePhone(appSettingResponse: any) {
    const listPhoneValidator: Array<PhoneValidatorModal> = [];
    appSettingResponse.forEach(item => {
      if (item.appSettingTypeId === 4) {
        const phoneValidator: PhoneValidatorModal = {
          countryCode: item.name,
          name: item.value.substr(0, 2),
          regex: item.value.substr(3)
        };
        listPhoneValidator.push(phoneValidator);
      }
    });
    return listPhoneValidator;
  }

  checkPhoneRegex(phoneValidator: Array<PhoneValidatorModal>, countryCode: string, phoneNumber: string) {
    let phoneValid = false;
    phoneValidator.forEach(item => {
      if (item.countryCode === countryCode) {
        phoneValid = new RegExp(item.regex).test(phoneNumber);
      }
    });
    return phoneValid;
  }

  public triggerAppSettingsDataByContentType(appSettings: any) {
    if (appSettings && Array.isArray(appSettings)) {
      this.appSettingsDataByContentType = appSettings;
      this.appSettingsDataByContentTypeSubject.next(this.appSettingsDataByContentType);
    }
  }

  public getTitleFromAppSettingsData(appSettingsData: any, appSettingName: string) {
    if (appSettingsData && Array.isArray(appSettingsData)) {
      const selectedTitle = appSettingsData.find((x: any) => x.name === appSettingName);
      if (selectedTitle && selectedTitle.value) {
        return selectedTitle.value;
      }
    }

    return '';
  }
}
