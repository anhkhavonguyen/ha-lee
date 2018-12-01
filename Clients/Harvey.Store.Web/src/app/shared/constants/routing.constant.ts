export class PageName {
  public static LOGIN_PAGE = 'login';
  public static HOME_PAGE = 'home';
  public static SERVING_PAGE = 'serving';
  public static CUSTOMER_PROFILE_PAGE = 'customerprofile';
  public static CUSTOMER_LISTING_PAGE = 'customerlisting';
  public static CHANGE_PASSWORD_PAGE = 'changepassword';
  public static FORGOT_PASSWORD_PAGE = 'forgotpassword';
  public static TRANSACTIONS_HISTORY_PAGE = 'transactionshistory';
  public static TERMS_AND_PRIVACY_PAGE = 'termsandprivacy';
  public static DASHBOARD_PAGE = 'dashboard';
  public static UPDATE_CUSTOMER_PROFILE_PAGE = 'updateprofile';
}

export class RoutingParamUser {
  public customerId: string;

  constructor(data?: any) {
    this.customerId = data.customerId;
  }
}

export class RoutingParamPhone {
  public phoneNumber: string;
  public countryCode: string;

  constructor(data?: any) {
    this.phoneNumber = data.phoneNumber;
    this.countryCode = data.countryCode;
  }
}
