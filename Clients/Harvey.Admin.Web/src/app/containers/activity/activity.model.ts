import * as moment from 'moment';

export enum ActionType {
    AddPoint = 0,
    RedeemPoint = 1,
    TopUp = 2,
    Spending = 3,
    Void = 4,
    ExpiryPoint = 5,
    UpdateAppSetting = 6,
    InitCustomer = 7,
    LogInStoreApp = 8,
    LogInAdminApp = 9,
    LogInMemberApp = 10,
    ActivateCustomer = 11,
    DeactivateCustomer = 12,
    LoginServingCustomer = 13,
    UpdateCustomerInfomation = 14,
    UpdateCustomerProfile = 15,
    ChangeMobilePhoneNumber = 16,
    DeleteAppSetting = 17,
    AddAppSetting = 18,
    UpdateOutlet = 19
}

export enum ActionArea {
    StoreApp = 0,
    MemberApp = 1,
    AdminApp = 2,
}

export class Activity {
    constructor(activity?: any) {
        if (!activity) { return; }
    }

    public id = '';
    public actionArea: any = '';
    public actionType: any = '';
    public description = '';
    public comment = '';
    public updatedDate = '';
    public updatedBy = '';
    public createdDate = '';
    public createdBy = '';
    public createdByName = '';

    static buildActivity(item: Activity): Activity {
        const activity = new Activity();
        activity.id = item.id;
        activity.actionArea = ActionArea[item.actionArea];
        activity.actionType = ActionType[item.actionType];
        activity.description = item.description;
        activity.updatedBy = item.updatedBy;
        activity.comment = item.comment;
        activity.updatedDate = (item.updatedDate && new Date(item.updatedDate).getFullYear() !== 1)
            ? moment.utc(item.updatedDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        activity.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
            ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        activity.createdBy = item.createdBy;
        activity.createdByName = item.createdByName;
        return activity;
    }
}

export class ActivitiesRequest {
    constructor(request?: any) {
        if (!request) { return; }
        this.pageNumber = request.pageNumber;
        this.pageSize = request.pageSize;
        this.searchText = request.searchText;
        this.dateFilter = request.dateFilter;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public searchText = '';
    public dateFilter = '';
}

export class ActivitiesResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.actionActivityModels = response.actionActivityModels;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public actionActivityModels!: Array<Activity>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}


export class GetHistoryChangeNumberCustomerRequest {
    constructor(request?: any) {
        if (!request) { return; }
        this.pageNumber = request.pageNumber;
        this.pageSize = request.pageSize;
        this.actionType = request.actionType;
        this.customerCode = request.customerCode;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public actionType = '';
    public customerCode = '';
}

export class GetHistoryChangeNumberCustomerResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.actionModels = response.ActionModels;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public actionModels!: Array<Activity>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class GetHistoryCustomerActivitiesRequest {
    constructor(request?: any) {
        if (!request) { return; }
        this.pageNumber = request.pageNumber;
        this.pageSize = request.pageSize;
        this.customerCode = request.customerCode;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public customerCode = '';
}

export class GetHistoryCustomerActivitiesResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.actionModels = response.ActionModels;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public actionModels!: Array<Activity>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class FilterHistoryCustomerActivitiesRequest {
    constructor(request?: any) {
        if (!request) { return; }
        this.pageNumber = request.pageNumber;
        this.pageSize = request.pageSize;
        this.fromDateFilter = request.fromDateFilter;
        this.toDateFilter = request.toDateFilter;
    }

    public pageNumber = 0;
    public pageSize = 0;
    public fromDateFilter = '';
    public toDateFilter = '';
}

export class FilterHistoryCustomerActivitiesResponse {
    constructor(response?: any) {
        if (!response) { return; }
        this.actionModels = response.actionModels;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItem = response.totalItem;
    }
    public actionModels!: Array<Activity>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}
