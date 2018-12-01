export class PhoneUtil {
  static FormatPhoneNumber(phoneCountryCode: string, number: string) {
    const last4digits = 4;
    let temp = '';
    if (phoneCountryCode && number) {
      for (let i = 0; i < number.length - last4digits; i++) {
        temp += '*';
      }
      return `+${phoneCountryCode} ${temp} ${number.substring(number.length - 4)}`;
    }
    return '-';
  }
}
