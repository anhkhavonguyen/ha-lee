import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AppSettingModel } from '../models/app-setting.model';

@Injectable()
export class AppSettingService {
  appSettingData: any;
  public appSettingsDataByStoreTypeSubject = new Subject<any>();

  public appSettingsDataByContentType: any;
  public appSettingsDataByContentTypeSubject = new Subject<any>();

  constructor(private http: HttpClient) {
  }

  public getAppSettings(userToken: string): Observable<Array<AppSettingModel>> {
    const apiURL = environment.apiGatewayUri + '/api/AppSettings/gets';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.http.get<Array<AppSettingModel>>(apiURL, { headers });
  }

  public getAppSettingsByType(type: Number): Observable<any> {
    const apiURL = environment.apiGatewayUri + `/api/AppSettings/getsbytype?type=${type}`;
    const headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(apiURL, { headers });
  }

  public triggerAppSettingsData(appSettings: any) {
    if (appSettings && Array.isArray(appSettings)) {
      this.appSettingData = appSettings;
      this.appSettingsDataByStoreTypeSubject.next(appSettings);
    }
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
