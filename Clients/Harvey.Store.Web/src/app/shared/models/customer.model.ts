import * as moment from 'moment';
import { PhoneUtil } from '../utils/phone-util';

export class Customer {
  public id: string;
  public customerCode: string;
  public firstName: string;
  public lastName: string;
  public email: string;
  public phone: string;
  public membership: string;
  public profileImage: string;
  public expiredDate: Date;
  public dateOfBirth: string;
  public lastUsed: string;
  public totalStranstion: number;
  public phoneCountryCode: string;
  public name: string;
  public status: string;
  public joinedDate: string;
  public commentMembership: string;
  public hasPreminumMembershipTransaction: boolean;

  constructor(customer?: any) {
    if (!customer) {
      return;
    }
    this.id = customer.id ? customer.id : null;
    this.customerCode = customer.customerCode ? this.customerCode : null;
    this.firstName = customer.firstName ? customer.firstName : null;
    this.lastName = customer.lastName ? customer.lastName : null;
    this.email = customer.email ? customer.email : null;
    this.phone = customer.phone ? customer.phone : null;
    this.membership = customer.membership ? customer.membership : null;
    this.profileImage = customer.profileImage ? customer.profileImage : null;
    this.expiredDate = customer.expiredDate ? customer.expiredDate : null;
    this.dateOfBirth = customer.dateOfBirth ? customer.dateOfBirth : null;
    this.lastUsed = customer.lastUsed ? customer.lastUsed : null;
    this.totalStranstion = customer.totalStranstion ? customer.totalStranstion : null;
    this.phoneCountryCode = customer.phoneCountryCode ? customer.phoneCountryCode : null;
    this.name = customer.name;
    this.status = customer.status ? customer.status : 'New';
    this.commentMembership = customer.commentMembership;
    this.hasPreminumMembershipTransaction = customer.hasPreminumMembershipTransaction;
  }

  static buildCustomer(item: Customer): Customer {
    const customer = new Customer();
    customer.id = item.id;
    customer.customerCode = item.customerCode;
    customer.name = (item.firstName && item.lastName) ? item.firstName + ' ' + item.lastName : 'Unknown';
    customer.email = item.email ? item.email : '-';
    customer.phone = PhoneUtil.FormatPhoneNumber(item.phoneCountryCode, item.phone);
    customer.profileImage = item.profileImage ? item.profileImage : '-';
    customer.status = item.status ? item.status : 'New';
    customer.dateOfBirth = item.dateOfBirth ? (new Date(item.dateOfBirth).getFullYear() !== 1 ?
      moment.utc(item.dateOfBirth).local().format('DD MMMM') : '-') : '-';
    customer.lastUsed = item.lastUsed ? (new Date(item.lastUsed).getFullYear() !== 1 ?
      moment.utc(item.lastUsed).local().format('DD/MM/YYYY HH:mm A') : '-') : '-';
    customer.joinedDate = item.joinedDate ? (new Date(item.joinedDate).getFullYear() !== 1 ?
      moment.utc(item.joinedDate).local().format('DD/MM/YYYY HH:mm A') : '-') : '-';
    customer.hasPreminumMembershipTransaction = item.hasPreminumMembershipTransaction;
    return customer;
  }
}

export class NewCustomerRequest {
  public phoneCountryCode: string;
  public phoneNumber: string;
  public originalUrl: string;
  public outletId: string;
  public outletName: string;
  public createdBy: string;
  public staffId: string;
}

export class CustomersResponse {
  public customerListResponse: Array<Customer>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public customerCode: string;

  constructor(customersResponse?: any) {
    if (!customersResponse) {
      return;
    }
    this.customerListResponse = customersResponse.customerListResponse;
    this.pageNumber = customersResponse.pageNumber;
    this.pageSize = customersResponse.pageSize;
    this.totalItem = customersResponse.totalItem;
    this.customerCode = this.customerCode;
  }
}

export class ResendPINRequest {
  public countryCode: string;
  public numberPhone: string;
  public outletName: string;
  public acronymBrandName: string;
}

export class CheckPINRequest {
  public userId: string;
  public PIN: string;
}

export class CheckPINResponse {
  public isValidPIN: Boolean;
}

export class FilterCustomerRequest {
  constructor(customersRequest?: any) {
    if (!customersRequest) {
      return;
    }
    this.pageNumber = customersRequest.pageNumber;
    this.pageSize = customersRequest.pageSize;
    this.fromDateFilter = customersRequest.fromDateFilter;
    this.toDateFilter = customersRequest.toDateFilter;
    this.outletId = customersRequest.outletId;
  }

  public pageNumber: number;
  public pageSize: number;
  public fromDateFilter: string;
  public toDateFilter: string;
  public outletId?: string;
}

export class FilterCustomerResponse {
  constructor(customersResponse?: any) {
    if (!customersResponse) {
      return;
    }
    this.customerListResponse = customersResponse.customerListResponse;
    this.pageNumber = customersResponse.pageNumber;
    this.pageSize = customersResponse.pageSize;
    this.totalItem = customersResponse.totalItem;
  }

  public customerListResponse: Array<Customer>;
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
}

export class CustomersRequest {
  constructor(Customersrequest?: any) {
    if (!Customersrequest) {
      return;
    }
    this.pageNumber = Customersrequest.pageNumber;
    this.pageSize = Customersrequest.pageSize;
    this.searchText = Customersrequest.searchText;
    this.dateFilter = Customersrequest.dateFilter;
    this.outletId = Customersrequest.outletId;
  }

  public pageNumber: number;
  public pageSize: number;
  public searchText: string;
  public dateFilter?: string;
  public outletId?: string;
}

export class BasicInfoCustomerRequest {
  public countryCode: string;
  public phoneNumber: string;
}

export class BasicInfoCustomerResponse {
  public fullName: string;
  public isMigrateData: boolean;
  public emailConfirmed: boolean;
  public phoneNumberConfirmed: boolean;
}

export class ResendSignUpRequest {
  public countryCode: string;
  public phoneNumber: string;
  public outletName: string;
  public originalUrl: string;
  public userId: string;

}

export class ResendResetPasswordRequest {
  public userName: string;
  public originalUrl: string;
  public acronymBrandName: string;
  public outletName: string;
}

export class GetBasicCustomerInfoRequest {
  public countryCode: string;
  public phoneNumber: string;
}

export class GetBasicCustomerInfoResponse {
  public userId: string;
  public fullName: string;
  public isMigrateData: boolean;
  public emailConfirmed: boolean;
  public phoneNumberConfirmed: boolean;
}

export enum Gender {
  Male,
  Female
}

export class UpdateCustomerProfileRequest {
  public customerId: string;
  public customerCode: string;
  public firstName: string;
  public lastName: string;
  public email: string;
  public dateOfBirth: Date;
  public password: string;
  public staffId: string;
  public updatedDate: Date;
  public gender ?: Gender;
}

export enum Status {
  active = 0,
  inActive = 1
}

export class CheckValidCustomerServing {
  public countryCode: string;
  public phone: string;
  public staffId: string;
  public isServing: boolean;
  public outletId: string;
}

export class ChangePhoneNumberRequest {
  public customerId: string;
  public customerCode: string;
  public updatedBy: string;
  public newPhoneNumber: string;
  public newPhoneCountryCode: string;
  public memberOriginalUrl: string;
}
