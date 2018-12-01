import { HttpService } from 'src/app/shared/services/http.service';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AppSettingModel, AppSettingDeleteRequest } from '../../shared/models/app-settings.model';
import { GetAppSettingsRequest, GetAppSettingsReponse } from '../../shared/models/app-settings.model';
import { UpdateAppSettingValueRequest } from 'src/app/containers/settings/update-setting-value/update-setting-value.model';
import { AddAppSettingValueRequest } from '../../containers/settings/add-setting-value/add-setting-value.model';
import HttpParamsHelper from '../helper/http-params.helper';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {

  constructor(private http: HttpService) {
  }

  public appSettingsDataByAdminType: any;
  public appSettingsDataByAdminTypeSubject = new Subject<any>();

  public appSettingsDataByContentType: any;
  public appSettingsDataByContentTypeSubject = new Subject<any>();

  public appSettingModelsByStoreType: any;
  public appSettingsDataByStoreTypeSubject = new Subject<any>();


  getAppSettingsData(request: GetAppSettingsRequest): Observable<GetAppSettingsReponse> {
    const apiURL = `${environment.apiGatewayUri}/api/AppSettings/getsData`;
    return this.http.get(apiURL, { params: HttpParamsHelper.parseObjectToHttpParams(request) });
  }

  addAppSettingValue(request: AddAppSettingValueRequest): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/AppSettings/add`;
    return this.http.post(apiURL, request);
  }

  updateAppSettingValue(request: UpdateAppSettingValueRequest): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/AppSettings/update`;
    return this.http.post(apiURL, request);
  }

  deleteAppSetting(request: AppSettingDeleteRequest): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/AppSettings`;
    return this.http.delete(apiURL, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
  }

  pushAppSettingData(appSettingResponse: any) {
    const listAppSettingData: Array<AppSettingModel> = [];
    appSettingResponse.forEach((item: any) => {
      const settingData: AppSettingModel = {
        id: item.id,
        name: item.name,
        value: item.value,
        groupName: item.groupName,
        appSettingTypeId: item.appSettingTypeId,
        appSettingType: item.appSettingType
      };
      listAppSettingData.push(settingData);
    });
    return listAppSettingData;
  }

  public getAppSettings(userToken: string): Observable<Array<AppSettingModel>> {
    const apiURL = environment.apiGatewayUri + '/api/AppSettings/gets';
    return this.http.get(apiURL, undefined);
  }

  public getAppSettingsByType(type: Number): Observable<any> {
    const apiURL = environment.apiGatewayUri + `/api/AppSettings/getsbytype?type=${type}`;
    return this.http.get(apiURL, undefined);
  }

  public triggerAppSettingsData(appSettings: any) {
    if (appSettings && Array.isArray(appSettings)) {
      this.appSettingsDataByAdminType = appSettings;
      this.appSettingsDataByAdminTypeSubject.next(this.appSettingsDataByAdminType);
    }
  }

  public triggerAppSettingsDataByContentType(appSettings: any) {
    if (appSettings && Array.isArray(appSettings)) {
      this.appSettingsDataByContentType = appSettings;
      this.appSettingsDataByContentTypeSubject.next(this.appSettingsDataByContentType);
    }
  }

  public triggerAppSettingsDataByStoreType(appSettings: any) {
    if (appSettings && Array.isArray(appSettings)) {
      this.appSettingModelsByStoreType = appSettings;
      this.appSettingsDataByStoreTypeSubject.next(this.appSettingModelsByStoreType);
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

