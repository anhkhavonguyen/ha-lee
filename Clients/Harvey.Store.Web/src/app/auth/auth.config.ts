import { PageName } from '../../app/shared/constants/routing.constant';

export const authConfig = {

  issuer: PageName.LOGIN_PAGE,

  redirectUri: PageName.HOME_PAGE,

  clientId: 'Harvey-staff-page',

  scope: 'openid profile email voucher',

  grant_type: 'password',

  client_secret: 'secret'
};
