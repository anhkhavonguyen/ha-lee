import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import HttpParamsHelper from 'src/app/shared/helper/http-params.helper';
import { ErrorLogRequest, ErrorLogResponse } from 'src/app/containers/error-log/error-log.model';

const ErrorLogResourceUrl = 'api/loggingError/gets';
@Injectable()
export class ErrorLogService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${ErrorLogResourceUrl}`;

    public GetErrorLog(request: ErrorLogRequest): Observable<ErrorLogResponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }
}
