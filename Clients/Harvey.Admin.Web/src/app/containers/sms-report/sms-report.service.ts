import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import { GetSMSNotificationsRequest, GetSMSNotificationsReponse } from 'src/app/containers/sms-report/sms-report.model';

const SMSReportResourceUrl = 'api/notifications/gets';

@Injectable()
export class SMSReportService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${SMSReportResourceUrl}`;

    public GetSMSNotifications(request: GetSMSNotificationsRequest): Observable<GetSMSNotificationsReponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }
}
