import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

const issuer = environment.authorityUri;

export const authConfig: AuthConfig = {
    issuer: issuer,
    loginUrl: 'login',
    redirectUri: 'homepage',
    clientId: 'Harvey-administrator-page',
    scope: 'openid profile email voucher',
    sessionChecksEnabled: true
  };


