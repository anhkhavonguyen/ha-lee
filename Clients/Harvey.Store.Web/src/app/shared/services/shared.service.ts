import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Outlet } from '../models/outlet.model';
import { Router } from '@angular/router';
import { User } from '../models/user.model';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../constants/common.constant';
import { AuthService } from '../../auth/auth.service';
import { AppSettingService } from './app-setting.service';
import { ValidatePhoneModel } from '../models/validate-phone.model';
import { AnnounceNewMembership, AnnounceEditComment } from '../models/membershipTransaction.model';
import { AppSettingModel } from '../models/app-setting.model';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  public isValidRedeemSubcription = new Subject<any>();
  public redeemBalanceSubscription = new Subject<any>();
  public addBalanceSubscription = new Subject<any>();
  public refreshCustomerInfoSubscription = new Subject<any>();

  public topUpWalletBalanceSubcription = new Subject<any>();
  public spendWalletBalanceSubcription = new Subject<any>();
  public membershipEditSubcription = new Subject<AnnounceNewMembership>();
  public editCommentSubcription = new Subject<AnnounceEditComment>();

  constructor(private router: Router,
    private oAuthStorage: OAuthStorage,
    private oAuthService: OAuthService) {
  }

  public setAnnounceOutlet(outlet: Outlet) {
    this.oAuthStorage.setItem(CommonConstants.currentOutlet, JSON.stringify(outlet));
  }

  public getAnnounceOutlet() {
    return JSON.parse(this.oAuthStorage.getItem(CommonConstants.currentOutlet));
  }

  public setAnnounceCurrentUser(user: User) {
    this.oAuthStorage.setItem(CommonConstants.currentUser, JSON.stringify(user));
  }

  public getAnnounceCurrentUser() {
    return JSON.parse(this.oAuthStorage.getItem(CommonConstants.currentUser));
  }

  public routingToPage(page: string) {
    this.router.navigateByUrl('/' + page);
  }

  public routingToPageWithParam(page: string, param: any) {
    this.router.navigate([`/${page}`], { queryParams: param });
  }

  public getAppSettings() {
    return this.oAuthStorage.getItem(CommonConstants.appSettings) ?
      JSON.parse(this.oAuthStorage.getItem(CommonConstants.appSettings)) : null;
  }

  public checkExpToken() {
    return this.oAuthService.hasValidAccessToken();
  }

  public getValidatePhone(phoneValidates: any) {
    const listValidate: Array<ValidatePhoneModel> = [];
    phoneValidates.appSettingModels
      .forEach(item => {
        if (item.appSettingTypeId === 4) {
          const validate: ValidatePhoneModel = {
            countryCode: item.name,
            name: item.value.substr(0, 2),
            regex: item.value.substr(3)
          };
          listValidate.push(validate);
        }
      });
    return listValidate;
  }

  public removeLocalStorage(item: string) {
    this.oAuthStorage.removeItem(item);
  }

  public getAppsettingByName(appSettingName: string) {
    const appSettings = this.getAppSettings();

    if (!appSettings) {
      return null;
    }

    const appsetting = appSettings.appSettingModels.find(x => x.name === appSettingName);
    return appsetting;
  }

  public parseJsonValueAppsetting(appsetting: AppSettingModel | null | undefined) {
    return appsetting ? JSON.parse(appsetting.value) : '';
  }

  public getJsonValueAppsetting(appsettingName: string) {
    const appsetting = this.getAppsettingByName(appsettingName);
    return this.parseJsonValueAppsetting(appsetting);
  }
}
