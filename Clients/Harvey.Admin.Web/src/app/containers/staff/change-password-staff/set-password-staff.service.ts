import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import { Subject } from 'rxjs';
import { GetSMSNotificationsRequest, GetSMSNotificationsReponse } from 'src/app/containers/sms-report/sms-report.model';
import { SetPasswordRequest } from 'src/app/containers/staff/change-password-staff/set-password-staff.model';

const SetPasswordResourceUrl = 'api/account/setPasswordForStoreAccount';

@Injectable()
export class SetPasswordService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${SetPasswordResourceUrl}`;
    public SetPassword(request: SetPasswordRequest) {
        return this.httpService.post(this.apiUrl, request);
    }
}
