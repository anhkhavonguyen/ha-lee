import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/shared/models/user.model';
import { authConfig } from 'src/app/auth/auth.config';
import { HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

const userTokenKey = 'access_token';
const redirectUrlKey = 'redirectUrl';
const issuer = environment.authorityUri;
const client_secret = 'secret';
const grant_type = 'password';
const expired_token_at = 'expires_at';
const expired_token = 'expires_token';
const GetUserRoleUrl = 'api/get-roles-current-user';
const NotificationMessageConstant = {
    errorMessage: 'Something went wrong. Please try again later.',

};

const userRoles = {
    Administrator: 'Administrator',
    AdminStaff: 'AdminStaff',
    Staff: 'Staff'
};

@Injectable()
export class AuthService {
    constructor(
        private httpClient: HttpClient,
        private oauthService: OAuthService,
        private router: Router,
        private oAuthStorage: OAuthStorage,
        private route: ActivatedRoute,
        private toast: ToastrService) {

    }

    public loginSubcription = new Subject<any>();
    public isFailLoginSubcription = new Subject<any>();
    public redirectUrl = authConfig.redirectUri;
    public intervalCheckValidToken: any;
    public isLoginSuccess = false;

    public getToken(user: User): Observable<any> {
        const body = new URLSearchParams();
        const options = {
            headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
        };
        const url = issuer + '/connect/token';

        body.set('client_id', authConfig.clientId ? authConfig.clientId : '');
        body.set('client_secret', client_secret);
        body.set('username', user.email);
        body.set('password', user.password);
        body.set('grant_type', grant_type);

        return this.httpClient.post(url, body.toString(), options);
    }

    public login(user?: User): void {
        if (!user) {
            this.router.navigate([authConfig.loginUrl]);
        } else {
            this.getToken(user).subscribe(data => {
                this.oAuthStorage.setItem(userTokenKey, data.access_token);
                this.oAuthStorage.setItem(expired_token_at, (new Date().getTime() + (data.expires_in * 1000)).toString());
                this.oAuthStorage.setItem(expired_token, (data.expires_in * 1000).toString());

                this.oauthService.getAccessToken();

                if (this.oauthService.hasValidAccessToken()) {
                    this.getCurrentUserRole(data.access_token).subscribe(res => {
                        if (!res.roles.includes(userRoles.Administrator) && !res.roles.includes(userRoles.AdminStaff)) {
                            this.toast.error('You do not have permission!');
                            this.logout();
                        } else {
                            const expiredToken = this.getExpiredToken() !== '' ? +this.getExpiredToken() : 0;
                            if (expiredToken > 0) {
                                this.setIntervalCheckValidToken(expiredToken);
                            }
                            this.isLoginSuccess = true;
                            this.loginSubcription.next(this.isLoginSuccess);
                            this.redirectUrl = this.getPreviousUrlBeforeLogging() === ''
                                               ? this.redirectUrl : this.getPreviousUrlBeforeLogging();
                            this.router.navigate([this.redirectUrl]);
                        }
                    }, error => {
                        this.toast.error('Something went wrong. Please try again!');
                        this.logout();
                    });
                } else {
                    this.router.navigate([authConfig.loginUrl]);
                }
            }, err => {
                const errorMessage = err.error && err.error.error_description
                ? err.error.error_description : NotificationMessageConstant.errorMessage;
                this.isFailLoginSubcription.next(errorMessage);
            });
        }
    }

    public logout() {
        this.oAuthStorage.removeItem(userTokenKey);
        this.oAuthStorage.removeItem(expired_token_at);
        this.oAuthStorage.removeItem(expired_token);
        this.clearIntervalCheckValidToken();
        this.isLoginSuccess = false;
        this.loginSubcription.next(this.isLoginSuccess);
        this.router.navigate([authConfig.loginUrl]);
    }

    public checkAuthenticated() {
        this.oauthService.getAccessToken();
        return this.oauthService.hasValidAccessToken();
    }

    public setPreviousUrlBeforeLogging(url: string): void {
        this.oAuthStorage.removeItem(redirectUrlKey);
        this.oAuthStorage.setItem(redirectUrlKey, url);
    }

    public getPreviousUrlBeforeLogging(): string {
        return this.oAuthStorage.getItem(redirectUrlKey) || '';
    }

    public getTokenFromStorage(): string {
        return this.oauthService.getAccessToken() || '';
    }

    public getExpiredToken() {
        return this.oAuthStorage.getItem(expired_token) || '';
    }

    public getExpiredTokenAt() {
        return this.oAuthStorage.getItem(expired_token_at) || '';
    }

    public getCurrentUserRole(userToken: string): Observable<any> {
        const apiURL = `${environment.apiGatewayUri}/${GetUserRoleUrl}`;
        const headers: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + userToken});
        return this.httpClient.get(apiURL, {headers});
    }

    public setIntervalCheckValidToken(expiredToken: number): void {
        this.intervalCheckValidToken = setInterval(() => {
            if (!this.checkAuthenticated()) {
                this.logout();
            }
        }, expiredToken);
    }

    public clearIntervalCheckValidToken() {
        if (this.intervalCheckValidToken) {
            clearInterval(this.intervalCheckValidToken);
        }
    }
}
