export class APIsURL {
  // Memberships Tranasaction APIs
  public static ADD_MEMBERSHIP_TRANSACTION = 'api/MembershipTransactions/add';
  public static GET_MEMBERSHIP_TRANSACTION = 'api/MembershipTransactions/getsbyoutlet';
  public static GET_CUSTOMER_MEMBERSHIP_TRANSACTION = 'api/membershipTransactions/gets';
  public static GET_VOID_MEMBERSHIP_SUMMARY = 'api/membershipTransactions/getvoidmembershiptransactions';

  // Customer APIs
  public static GET_NEW_CUSTOMERS = 'api/customers/getnewcustomers';
  public static GET_NEW_PREMIUM_CUSTOMERS = 'api/customers/getpremiumcustomers';
  public static GET_UPGRADED_CUSTOMERS = 'api/customers/getupgradedcustomers';
  public static GET_RENEWED_CUSTOMERS = 'api/customers/getrenewedcustomers';
  public static GET_EXTENDED_CUSTOMERS = 'api/customers/getextendedcustomers';
  public static GET_EXPIRED_CUSTOMERS = 'api/customers/getexpiredcustomers';
  public static GET_DOWNGRADED_CUSTOMERS = 'api/customers/getdowngradedcustomers';
  public static GET_CUSTOMER_INFO = 'api/customers/getcustomerbyphone';
  public static INIT_NEW_CUSTOMER = 'api/Customers/initcustomerprofile';
  public static GET_BALANCE_POINTS_CUSTOMER = 'api/PointTransactions/getpointbalance';
  public static GET_CUSTOMERS = 'api/customers/gets';
  public static GET_BASIC_INFO_CUSTOMER = 'api/account/get-basic-account-info';
  public static RESEND_SIGN_UP_LINK = 'api/account/ReSendSignUpLink';
  public static RESEND_RESET_PASSWORD = 'api/account/forgot-password-via-sms';
  public static RESEND_PIN = 'api/send-pin';
  public static GET_BASIC_CUSTOMER_INFO = 'api/account/get-basic-account-info';
  public static UPDATE_CUSTOMER_PROFILE = 'api/account/UpdateCustomerProfile';
  public static GET_CUSTOMER_INFO_BY_ID = 'api/customers/getcustomerbyid';
  public static CHANGE_PHONE_NUMBER = 'api/account/changephonenumber';

  // Points Transaction APIs
  public static GET_POINT_DEBIT_SUMMARY = 'api/pointTransactions/getsdebitsummary';
  public static GET_POINT_CREDIT_SUMMARY = 'api/pointTransactions/getscreditsummary';
  public static GET_TOTAL_BALANCE_POINT_SUMMARY = 'api/pointTransactions/getstotalbalance';
  public static GET_VOID_OF_CREDIT_POINT_SUMMARY = 'api/pointTransactions/getsvoidofcredit';
  public static GET_VOID_OF_DEBIT_POINT_SUMMARY = 'api/pointTransactions/getsvoidofdebit';
  public static GET_CUSTOMER_POINT_TRANSACTION = 'api/pointTransactions/getsbycustomer';
  public static GET_EXPIRY_POINT = 'api/pointTransactions/getexpirypoints';

  // Wallet Transaction APIs
  public static GET_TOTAL_BALANCE_WALLET_SUMMARY = 'api/walletTransactions/getstotalbalance';
  public static GET_WALLET_DEBIT_SUMMARY = 'api/walletTransactions/getsdebitsummary';
  public static GET_WALLET_CREDIT_SUMMARY = 'api/walletTransactions/getscreditsummary';
  public static GET_VOID_OF_CREDIT_WALLET_SUMMARY = 'api/walletTransactions/getsvoidofcredit';
  public static GET_VOID_OF_DEBIT_WALLET_SUMMARY = 'api/walletTransactions/getsvoidofdebit';
  public static GET_CUSTOMER_WALLET_TRANSACTION = 'api/walletTransactions/getsbycustomer';
}
