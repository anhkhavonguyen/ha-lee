import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as userActions from './state/user.actions';
import * as fromRoot from '../../state/app.state';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private oauthService: OAuthService,
        private store: Store<fromRoot.State>) {
    }

    async canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Promise<boolean> {
        let canActive = false;
        await this.oauthService
            .loadDiscoveryDocument(environment.ids.loadDocumentUrl)
            .then(doc => {
                canActive = this.oauthService.hasValidAccessToken();
                if (canActive) {
                    this.oauthService.loadUserProfile()
                        .then((userProfile: any) => {
                            if (userProfile) {
                                this.store.dispatch(new userActions.LoginSuccessul(userProfile));
                            }
                        });
                }
            });
        return canActive;
    }
}
