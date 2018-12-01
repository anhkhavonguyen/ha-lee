import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { authConfig, getTokenConfig } from './auth.authen-config';
import { environment } from '../../environments/environment';
import { Gender } from '../shared/models/user-profile.model';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient,
    private oAuthStorage: OAuthStorage) {
  }

  verifyPhoneNumber(countrycode: string, phoneNumber: string) {
    const apiURL = `${environment.apiGatewayUri}/api/account/get-basic-account-info?countryCode=${countrycode}&phoneNumber=${phoneNumber}`;
    return this.http.get(apiURL);
  }

  initNewAccount(request: InitNewAccountRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/init`;
    return this.http.post(apiURL, request);
  }

  regNewAccount(request: RegNewAccountRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/signup`;
    return this.http.post(apiURL, request);
  }

  forgotPassword(request: ForgotPasswordRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/forgot-password-via-email`;
    return this.http.post(apiURL, request);
  }

  resetPassword(request: ResetPasswordRequest) {
    const apiURL = `${environment.apiGatewayUri}/api/account/reset-password`;
    return this.http.post(apiURL, request);
  }

  checkSignUpLink(request: string): Observable<any> {
    const apiURL = `${environment.apiGatewayUri}/api/account/CheckSignUpLink?code=${request}`;
    return this.http.get(apiURL);
  }

  getToken(user: UserLoginModel) {
    const body = new URLSearchParams();
    const options = {
      headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
    };

    const url = getTokenConfig.GET_TOKEN_URL;

    body.set('client_id', authConfig.clientId);
    body.set('client_secret', getTokenConfig.CLIENT_SECRET);
    body.set('username', user.phoneNumber);
    body.set('password', user.password);
    body.set('grant_type', getTokenConfig.Grant_Type);

    return this.http.post(url, body.toString(), options);
  }

  getTokenFormStorage() {
    return this.oAuthStorage.getItem('access_token');
  }
}

export class UserLoginModel {
  phoneNumber: string;
  password: string;
}

export class ForgotPasswordRequest {
  userName: string;
  originalUrl: string;
  brandName: string;
  acronymBrandName: string;
}

export class ResetPasswordRequest {
  code: string;
  password: string;
  confirmPassword: string;
  acronymBrandName: string;
}

export class InitNewAccountRequest {
  phoneCountryCode: string;
  phoneNumber: string;
  originalUrl: string;
}

export class RegNewAccountRequest {
  code: string;
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  password: string;
  gender: Gender;
  avatar: string;
}
