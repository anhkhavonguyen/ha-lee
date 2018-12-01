import * as moment from 'moment';
import { Gender } from './edit-customer-info/edit-customer-info.model';

export class Customer {
    constructor(customer?: any) {
        if (!customer) { return; }
        this.id = customer.id;
        this.firstName = customer.firstName;
        this.lastName = customer.lastName;
        this.email = customer.email;
        this.phone = customer.phone;
        this.dateOfBirth = customer.dateOfBirth;
        this.profileImage = customer.profileImage;
        this.profileImage = customer.profileImage;
        this.lastUsed = customer.lastUsed;
        this.status = customer.status;
        this.name = customer.name;
        this.phoneCountryCode = customer.phoneCountryCode;
        this.expiredDate = customer.expiredDate;
        this.voidedDate = customer.voidedDate;
        this.activatedDate = customer.activatedDate;
        this.deactivatedDate = customer.deactivatedDate;
        this.comment = customer.comment;
        this.upgradedDate = customer.upgradedDate;
        this.extendedDate = customer.extendedDate;
        this.renewedDate = customer.renewedDate;
        this.phoneNumber = customer.phoneNumber;
        this.membership = customer.membership;
        this.customerCode = customer.customerCode;
        this.gender = customer.gender;
        this.zipCode = customer.zipCode;
    }

    public id = '';
    public firstName = '';
    public lastName: string | null = '';
    public email: string | null = '';
    public phone = '';
    public dateOfBirth = '';
    public profileImage: string | null = '';
    public joinedDate = '';
    public lastUsed = '';
    public status = '';
    public name: string | null = '';
    public phoneCountryCode = '';
    public expiredDate = '';
    public voidedDate = '';
    public activatedDate = '';
    public deactivatedDate = '';
    public comment = '';
    public upgradedDate = '';
    public extendedDate = '';
    public renewedDate = '';
    public phoneNumber = '';
    public membership = '';
    public customerCode = '';
    public gender: Gender | string | null | any = '';
    public zipCode: string | null = '';

    static buildCustomer(item: Customer): Customer {
        const customer = new Customer();
        customer.id = item.id;
        customer.lastName = item.lastName;
        customer.firstName = item.firstName;
        customer.name =
            (`${item.firstName ? item.firstName : ''} ${item.lastName ? item.lastName : ''}`).trim() !== ''
                ? `${item.firstName ? item.firstName : ''} ${item.lastName ? item.lastName : ''}` : 'Unknown';
        customer.email = item.email ? item.email : null;
        customer.phone = item.phoneCountryCode && item.phone ? `+${item.phoneCountryCode} ${item.phone}` : '-';
        customer.phoneNumber = item.phone;
        customer.gender = item.gender === null ? null : (Gender[item.gender]);
        customer.zipCode = item.zipCode ? item.zipCode : null;
        customer.phoneCountryCode = item.phoneCountryCode;
        customer.profileImage = item.profileImage || item.profileImage === '' ? item.profileImage : '';
        customer.dateOfBirth = (item.dateOfBirth && new Date(item.dateOfBirth).getFullYear() !== 1)
            ? moment.utc(item.dateOfBirth).local().format('LL') : '-';
        customer.lastUsed = (item.lastUsed && new Date(item.lastUsed).getFullYear() !== 1)
            ? moment.utc(item.lastUsed).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.joinedDate = (item.joinedDate && new Date(item.joinedDate).getFullYear() !== 1)
            ? moment.utc(item.joinedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.status = item.status;
        customer.expiredDate = (item.expiredDate && new Date(item.expiredDate).getFullYear() !== 1)
            ? moment.utc(item.expiredDate).local().format('LL') : '-';
        customer.voidedDate = (item.voidedDate && new Date(item.voidedDate).getFullYear() !== 1)
            ? moment.utc(item.voidedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.activatedDate = (item.activatedDate && new Date(item.activatedDate).getFullYear() !== 1)
            ? moment.utc(item.activatedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.deactivatedDate = (item.deactivatedDate && new Date(item.deactivatedDate).getFullYear() !== 1)
            ? moment.utc(item.deactivatedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.upgradedDate = (item.upgradedDate && new Date(item.upgradedDate).getFullYear() !== 1)
            ? moment.utc(item.upgradedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.extendedDate = (item.extendedDate && new Date(item.extendedDate).getFullYear() !== 1)
            ? moment.utc(item.extendedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.renewedDate = (item.renewedDate && new Date(item.renewedDate).getFullYear() !== 1)
            ? moment.utc(item.renewedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        customer.comment = item.comment ? item.comment : '-';
        customer.membership = item.membership;
        customer.customerCode = item.customerCode;
        return customer;
    }
}

export class CustomersRequest {
    constructor(Customersrequest?: any) {
        if (!Customersrequest) { return; }
        this.pageNumber = Customersrequest.pageNumber;
        this.pageSize = Customersrequest.pageSize;
        this.searchText = Customersrequest.searchText;
        this.dateFilter = Customersrequest.dateFilter;
        this.outletId = Customersrequest.outletId;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public searchText = '';
    public dateFilter = '';
    public outletId?: string;
}

export class CustomersResponse {
    constructor(customersResponse?: any) {
        if (!customersResponse) { return; }
        this.customerListResponse = customersResponse.customerListResponse;
        this.pageNumber = customersResponse.pageNumber;
        this.pageSize = customersResponse.pageSize;
        this.totalItem = customersResponse.totalItem;
    }
    public customerListResponse!: Array<Customer>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export enum MembershipActionType {
    Migration,
    New,
    Upgrade,
    Renew,
    Extend,
    Downgrade,
    Void,
    ChangeExpiredDate,
    Comment
}
