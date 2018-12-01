import * as moment from 'moment';

export class SMSNotificationModel {
    constructor(smsNotificationModel?: any) {
        if (!smsNotificationModel) { return; }
        this.id = smsNotificationModel.id;
        this.content = smsNotificationModel.content;
        this.action = smsNotificationModel.action;
        this.receivers = smsNotificationModel.receivers;
        this.createdDate = smsNotificationModel.createdDate;
        this.status = smsNotificationModel.status;
    }

    public id = 0;
    public content = '';
    public action = '';
    public receivers = '';
    public createdDate = '';
    public status = '';

    static buildSMSNotification(item: SMSNotificationModel): SMSNotificationModel {
        const smsNotification = new SMSNotificationModel();
        smsNotification.id = item.id;
        smsNotification.content = item.content ? item.content : '-';
        smsNotification.action = item.action ? item.action : '-';
        smsNotification.receivers = item.receivers ? item.receivers : '-';
        smsNotification.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
            ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A')
            : '-';
        smsNotification.status = item.status ? item.status : '-';
        return smsNotification;
    }
}


export class GetSMSNotificationsRequest {
    public pageNumber = 0;
    public pageSize = 0;
    public searchText = '';
    public dateFilter = '';
}

export class GetSMSNotificationsReponse {
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
    public listNotification!: Array<SMSNotificationModel>;
}
