export class PhoneModel {
  public countryCode: string;
  public phoneNumber: string;

  constructor(data?: any) {
    if (!data) {
      return;
    }
    this.countryCode = data.countryCode;
    this.phoneNumber = data.phoneNumber;
  }
}
