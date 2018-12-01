import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {ErrorRequest} from '../models/error-request.modal';

const LoggingResourceUrl = 'api/loggingError/logError';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {

  constructor(private http: HttpClient) {
  }

  public url = `${environment.apiGatewayUri}/${LoggingResourceUrl}`;

  public logError(request: ErrorRequest) {
    const logErrorUrl = this.url;
    return this.http.post(logErrorUrl, request);
  }
}
