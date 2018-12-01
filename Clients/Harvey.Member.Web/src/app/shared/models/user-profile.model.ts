export class UserProfileModel {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  gender: Gender;
  avatar: string;
  zipCode: string;
  phoneNumber: string;
  phoneCountryCode: string;
}

export enum Gender {
  Male,
  Female
}

export class PhoneVerifiedResponse {
  fullName: string;
  isMigrateData: boolean;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
  passwordHash: string;
}

export class MembershipType {
  basic = 'Basic';
  premium = 'Premium+';
}
