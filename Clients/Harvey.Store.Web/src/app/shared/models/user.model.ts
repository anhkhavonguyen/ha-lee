export class User {
  public id: string;
  public lastName: string;
  public firstName: string;
  public email: string;
  public phoneNumber: string;

  constructor(user?: any) {
    if (!user) {
      return;
    }
    this.id = user.id;
    this.lastName = user.lastName;
    this.firstName = user.firstName;
    this.email = user.email;
    this.phoneNumber = user.phoneNumber;
  }
}

export class UserLogin {
  username: string;
  password: string;
}

export class ChangePasswordRequest {
  public currentPassword: string;
  public newPassword: string;

  constructor(request?: any) {
    if (!request) {
      return;
    }
    this.currentPassword = request.currentPassword;
    this.newPassword = request.newPassword;
  }
}

export class UserRole {
  public roles: Array<string>;
}

export class CustomerTransactionRequest {
  public customerId = '';
  public pageNumber = 0;
  public pageSize = 0;

  constructor(customerTransactionRequest?: any) {
    if (!customerTransactionRequest) { return; }
    this.customerId = customerTransactionRequest.customerId;
    this.pageNumber = customerTransactionRequest.pageNumber;
    this.pageSize = customerTransactionRequest.pageSize;
  }
}
