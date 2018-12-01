export class User {
    constructor(user?: any) {
        if (!user) { return; }
        this.id = user.Id;
        this.firstName = user.firstName;
        this.lastName = user.lastName;
        this.email = user.email;
        this.phoneNumber = user.phoneNumber;
        this.fullName = user.fullName;
        this.password = user.password;
    }

    public id = '';
    public firstName = '';
    public lastName = '';
    public email = '';
    public phoneNumber = '';
    public fullName = '';
    public password = '';
}
