import {ErrorHandler, Injectable, Injector} from '@angular/core';
import {ErrorRequest} from '../models/error-request.modal';
import {LoggingService} from './logging.service';

const source = 'MemberApp';

@Injectable({
  providedIn: 'root'
})
export class GlobalExceptionHandlerService implements ErrorHandler {

  constructor(private injector: Injector) {
  }

  handleError(error: Error) {
    const loggingService = this.injector.get(LoggingService);
    const errorRequest = new ErrorRequest();
    errorRequest.Source = source;
    errorRequest.ErrorCaption = error.message ? error.message : error.toString();
    errorRequest.ErrorMessage = error.stack ? error.stack : error.toString();
    loggingService.logError(errorRequest).subscribe();
    throw error;
  }
}
