export class OutletModel {
    constructor(outletModel?: any) {
        if (!outletModel) { return; }
        this.id = outletModel.id;
        this.phone = outletModel.phone;
        this.name = outletModel.name;
        this.account = outletModel.account;
        this.outletImage = outletModel.outletImage;
        this.address = outletModel.address;
        this.firstNameAccount = outletModel.firstNameAccount;
        this.lastNameAccount = outletModel.lastNameAccount;
        this.phoneCountryCode = outletModel.phoneCountryCode;
        this.code = outletModel.code;
    }

    public id = '';
    public phone = '';
    public name = '';
    public account = '';
    public outletImage = '';
    public address = '';
    public firstNameAccount = '';
    public lastNameAccount = '';
    public phoneCountryCode = '';
    public code = '';

    static buildOutlet(item: OutletModel): OutletModel {
        const outlet = new OutletModel();
        outlet.id = item.id;
        outlet.phone =  item.phone ? item.phone : '-';
        outlet.phoneCountryCode = item.phoneCountryCode ? item.phoneCountryCode : '';
        outlet.name = item.name;
        outlet.outletImage = item.outletImage;
        outlet.account = (item.firstNameAccount && item.lastNameAccount) ? item.firstNameAccount + ' ' + item.lastNameAccount : '-';
        outlet.address = item.address;
        outlet.code = item.code;
        return outlet;
    }
}


export class GetOutletsRequest {
    public pageNumber = 0;
    public pageSize = 0;
}

export class GetOutletsReponse {
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
    public outletModels = [];
}
