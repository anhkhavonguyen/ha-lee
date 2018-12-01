export class ValidatePhoneModel {
  public name: string;
  public countryCode: string;
  public regex: string;

  constructor(data?: any) {
    this.name = data.name;
    this.countryCode = data.countryCode;
    this.regex = data.regex;
  }
}
