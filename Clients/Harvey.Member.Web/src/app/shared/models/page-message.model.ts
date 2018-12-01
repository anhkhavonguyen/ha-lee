export class MessageConfigModel {
  messageContent: string;
  button?: string;
  buttonNavigationLink?: string;
}

export enum MessageCode {
  newUserRegistration = 'New-User-Registration',
  userNotUpdateProfile = 'Not-Update-Profile',
  requestResendPin = 'Send-PIN-Request',
  renewPasswordForMigrateUser = 'Migrate-User-Renew-Password',
  forgotPassword = 'Request-New-Password',
  signUp = 'Sign-Up-Success',
  changePhoneNumber = 'Change-Phone-Number'
}
