import * as moment from 'moment';

export class ErrorLogRequest {
    constructor(errorLogrequest?: any) {
        if (!errorLogrequest) { return; }
        this.pageNumber = errorLogrequest.pageNumber;
        this.pageSize = errorLogrequest.pageSize;
    }

    public pageNumber = 0;
    public pageSize = 0;
}

export class ErrorLogResponse {
    constructor(errorLogResponse?: any) {
        if (!errorLogResponse) { return; }
        this.listError = errorLogResponse.listError;
        this.pageNumber = errorLogResponse.pageNumber;
        this.pageSize = errorLogResponse.pageSize;
        this.totalItem = errorLogResponse.totalItem;
    }
    public listError!: Array<ErrorLogEntry>;
    public pageNumber = 0;
    public pageSize = 0;
    public totalItem = 0;
}

export class ErrorLogEntry {
    constructor(errorLogEntry?: any) {
        if (!errorLogEntry) { return; }
        this.id = errorLogEntry.id;
        this.detail = errorLogEntry.detail;
        this.caption = errorLogEntry.caption;
        this.createdDate = errorLogEntry.createdDate;
        this.createdBy = errorLogEntry.createdBy;
        this.errorLogSource = errorLogEntry.errorLogSource;
    }

    public id = '';
    public detail = '';
    public caption = '';
    public createdDate = '';
    public createdBy = '';
    public errorLogSource = '';

    static buildLogEntry(item: ErrorLogEntry): ErrorLogEntry {
        const errorLogEntry = new ErrorLogEntry();
        errorLogEntry.id = item.id;
        errorLogEntry.detail = item.detail ? item.detail : '-';
        errorLogEntry.caption = item.caption ? item.caption : '-';
        errorLogEntry.createdDate = (item.createdDate && new Date(item.createdDate).getFullYear() !== 1)
                                    ? moment.utc(item.createdDate).local().format('DD/MM/YYYY HH:mm A') : '-';
        errorLogEntry.createdBy = item.createdBy ? item.createdBy : '-';
        errorLogEntry.errorLogSource = item.errorLogSource ? item.errorLogSource : '-';
        return errorLogEntry;
    }
}

