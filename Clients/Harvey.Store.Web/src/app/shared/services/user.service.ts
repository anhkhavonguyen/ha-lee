import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import { ChangePasswordRequest, User, UserRole } from 'src/app/shared/models/user.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable()
export class UserService {

  constructor(private oauthService: OAuthService, private http: HttpClient) {
  }

  public getCurrentUser(userToken: string): Observable<User> {
    const apiURL = environment.apiGatewayUri + '/api/account/get-user-profile';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.http.get<User>(apiURL, { headers });
  }

  public changePassword(userToken: string, request: ChangePasswordRequest) {
    const apiURL = environment.apiGatewayUri + '/api/account/change-password';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.http.put(apiURL, request, { headers });
  }

  public getOutletByStaff(userToken: string, staffId: string): Observable<any> {
    const apiUrl = environment.apiGatewayUri + '/api/outlets/getsbystaff' + '?UserId=' + staffId + '&PageNumber=0&PageSize=10';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.http.get(apiUrl, { headers });
  }

  public getUserRole(userToken: string): Observable<UserRole> {
    const apiURL = environment.apiGatewayUri + '/api/get-roles-current-user';
    const headers: HttpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + userToken
    });
    return this.http.get<UserRole>(apiURL, { headers });
  }
}
