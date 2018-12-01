import {environment} from '../../environments/environment';
import {AuthConfig} from 'angular-oauth2-oidc';

export const getTokenConfig = {
  GET_TOKEN_URL: `${environment.authorityUri}/connect/token`,
  CLIENT_SECRET: 'secret',
  Grant_Type: 'password'
};

export const authConfig: AuthConfig = {
  issuer: environment.authorityUri,
  redirectUri: '/auth/login',
  clientId : 'Harvey-member-page',
  scope: 'openid profile email role',
  sessionChecksEnabled: true,
};
