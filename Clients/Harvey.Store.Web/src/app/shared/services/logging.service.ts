import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { environment } from 'src/environments/environment';
import { ErrorRequest } from 'src/app/shared/models/error-request.model';

const LoggingResourceUrl = 'api/loggingError/logError';

@Injectable()
export class LoggingService {

  constructor(private httpService: HttpService) {
  }

  public url = `${environment.apiGatewayUri}/${LoggingResourceUrl}`;

  public logError(request: ErrorRequest) {
    const logErrorUrl = this.url;
    return this.httpService.post(logErrorUrl, request);
  }
}
