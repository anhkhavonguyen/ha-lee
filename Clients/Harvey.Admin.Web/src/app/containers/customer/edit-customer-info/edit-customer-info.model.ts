import * as moment from 'moment';

export class ChangeCustomerMobileRequest {
    public newPhoneCountryCode = '';
    public newPhoneNumber = '';
    public customerCode = '';
    public customerId = '';
    public comment = '';
    public memberOriginalUrl = '';
}

export class ChangeCustomerInfoRequest {
    public customerId: string | null = null;
    public customerCode: string | null = null;
    public memberOriginalUrl: string | null = null;
    public newPhoneCountryCode: string | null = null;
    public newPhoneNumber: string | null = null;
    public lastName: string | null = null;
    public firstName: string | null = null;
    public dateOfBirth: any | null = null;
    public email: string | null = null;
    public gender: Gender | string | null = null;
    public postalCode: string | null = null;
    public acronymBrandName: string | null = null;
}

export enum Gender {
    Male,
    Female
}

export enum Fields {
    LastName,
    FirstName,
    Birthday,
    Phone,
    Email,
    Gender,
    PostalCode
}
