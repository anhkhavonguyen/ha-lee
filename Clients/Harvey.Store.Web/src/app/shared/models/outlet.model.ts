export class Outlet {
  public id: string;
  public name: string;
  public outletImage: string;
  public address: string;
  public phone: string;
  public phoneCountryCode: string;
  public email: string;

  constructor(outlet?: any) {
    if (!outlet) {
      return;
    }
    this.id = outlet.id;
    this.name = outlet.name;
    this.outletImage = outlet.outletImage;
    this.address = outlet.address;
    this.phone = outlet.phone;
    this.phoneCountryCode = outlet.phoneCountryCode;
    this.email = outlet.email;
  }
}

export class GetTransactionsByOutletRequest {
  public outletId: string;
  public pageNumber: number;
  public pageSize: number;
}
