import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { authConfig } from 'src/app/auth/auth.config';
import { BehaviorSubject, Subject } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { UserLogin } from '../shared/models/user.model';
import { CommonConstants } from '../shared/constants/common.constant';
import { UserService } from '../shared/services/user.service';
import { TranslateService } from '@ngx-translate/core';

const authorityUri = environment.authorityUri;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public loginSubcription = new Subject<any>();
  public redirectUrl = authConfig.redirectUri;
  public announceLoginError = new BehaviorSubject<string>('');
  public intervalCheckValidToken: any;


  constructor(private httpClient: HttpClient,
    private router: Router,
    private oauthService: OAuthService,
    private oAuthStorage: OAuthStorage,
    private userService: UserService,
    private translate: TranslateService) {

  }

  public getToken(user: UserLogin) {
    const body = new URLSearchParams();
    const options = {
      headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
    };
    const authUrl = authorityUri + '/connect/token';

    body.set('client_id', authConfig.clientId);
    body.set('client_secret', authConfig.client_secret);
    body.set('username', user.username);
    body.set('password', user.password);
    body.set('grant_type', authConfig.grant_type);

    return this.httpClient.post(authUrl, body.toString(), options);
  }

  public login(user?: UserLogin) {
    if (!user) {
      this.router.navigate([authConfig.issuer]);
    } else {
      this.getToken(user).subscribe((res: any) => {
        this.oAuthStorage.setItem(CommonConstants.userTokenKey, res.access_token);
        this.oAuthStorage.setItem(CommonConstants.tokenExpired, (new Date().getTime() + (res.expires_in * 1000)).toString());
        this.oAuthStorage.setItem(CommonConstants.expired_token, (res.expires_in * 1000).toString());

        if (this.oauthService.hasValidAccessToken()) {
          const expiredToken = this.getExpiredToken() !== '' ? +this.getExpiredToken() : 0;
          if (expiredToken > 0) {
            this.setIntervalCheckValidToken(expiredToken);
          }
          this.userService.getCurrentUser(res.access_token).subscribe(userInfo => {
            this.oAuthStorage.setItem(CommonConstants.currentUser, JSON.stringify(userInfo));
            this.router.navigate([authConfig.redirectUri]);
            this.announceLoginError.next('');
          });
        }
      }, err => {
        this.translate.get('ERROR.invalid-account').subscribe(message => {
          this.announceLoginError.next(message);
        });
      });
    }
  }

  public logout() {
    this.oAuthStorage.removeItem(CommonConstants.userTokenKey);
    this.oAuthStorage.removeItem(CommonConstants.currentOutlet);
    this.oAuthStorage.removeItem(CommonConstants.currentCustomer);
    this.oAuthStorage.removeItem(CommonConstants.currentUser);
    this.oAuthStorage.removeItem(CommonConstants.appSettings);
    this.oAuthStorage.removeItem(CommonConstants.redirectUrlKey);
    this.oAuthStorage.removeItem(CommonConstants.tokenExpired);
    this.oAuthStorage.removeItem(CommonConstants.expired_token);
    this.clearIntervalCheckValidToken();
    this.loginSubcription.next();
    this.router.navigate([authConfig.issuer]);
  }

  public checkAuthenticated() {
    const data = this.oAuthStorage.getItem(CommonConstants.userTokenKey);
    if (!data) {
      return false;
    }
    return true;

  }

  public setPreviousUrlBeforeLogging(url: string): void {
    this.oAuthStorage.removeItem(CommonConstants.redirectUrlKey);
    this.oAuthStorage.setItem(CommonConstants.redirectUrlKey, url);
  }

  public getPreviousUrlBeforeLogging(): string {
    const previousUrl: string = this.oAuthStorage.getItem(CommonConstants.redirectUrlKey) || '';
    return previousUrl;
  }

  public getTokenFormStorage() {
    return this.oAuthStorage.getItem(CommonConstants.userTokenKey);
  }

  public setIntervalCheckValidToken(expiredToken: number): void {
    this.intervalCheckValidToken = setInterval(() => {
      if (!this.oauthService.hasValidAccessToken()) {
        this.logout();
      }
    }, expiredToken);
  }

  public clearIntervalCheckValidToken() {
    if (this.intervalCheckValidToken) {
      clearInterval(this.intervalCheckValidToken);
    }
  }

  public getExpiredToken() {
    return this.oAuthStorage.getItem(CommonConstants.expired_token) || '';
  }

  public getExpiredTokenAt() {
    return this.oAuthStorage.getItem(CommonConstants.tokenExpired) || '';
  }
}
