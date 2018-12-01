import * as moment from 'moment';

export class StaffModel {
    constructor(staffModel?: any) {
        if (!staffModel) { return; }
        this.id = staffModel.id;
        this.firstName = staffModel.firstname;
        this.lastName = staffModel.lastname;
        this.email = staffModel.email;
        this.password = staffModel.password;
        this.lastUsed = staffModel.lastUsed;
        this.dateOfBirth = staffModel.dateOfBirth;
        this.phone = staffModel.phone;
        this.phoneCountryCode = staffModel.phoneCountryCode;
        this.typeOfStaff = staffModel.typeOfStaff;
    }

    public id = '';
    public firstName = '';
    public lastName = '';
    public email = '';
    public password = '';
    public lastUsed = '';
    public dateOfBirth = '';
    public phone = '';
    public name = '';
    public phoneCountryCode = '';
    public typeOfStaff = '';

    static buildStaff(item: StaffModel): StaffModel {
        const staff = new StaffModel();
        staff.id = item.id;
        staff.name = item.firstName + ' ' + item.lastName;
        staff.email = item.email;
        staff.dateOfBirth = (item.dateOfBirth && new Date(item.dateOfBirth).getFullYear() !== 1)
        ?  moment.utc(item.dateOfBirth).local().format('DD/MM/YYYY') : '-';
        staff.lastUsed = (item.lastUsed && new Date(item.lastUsed).getFullYear() !== 1)
        ? moment.utc(item.lastUsed).local().format('DD/MM/YYYY HH:mm A') : '-';
        staff.phone = item.phone ? item.phone : '-';
        staff.phoneCountryCode = item.phoneCountryCode ? item.phoneCountryCode : '';
        staff.typeOfStaff = item.typeOfStaff ? item.typeOfStaff : '';
        return staff;
    }
}


export class GetStaffRequest {
    public pageNumber = 0;
    public pageSize = 0;
    public searchString = '';
}

export class GetStaffReponse {
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
    public staffModels!: Array<StaffModel>;
}

export class GetStaffsByOutletRequest {
    public pageNumber = 0;
    public pageSize = 0;
    public outletId = '';
}

export class GetStaffsByOutletResponse {
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
    public staffModels!: Array<StaffModel>;
}

export class UserRole {
    public roles!: Array<string>;
}
