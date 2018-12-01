import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { ErrorRequest } from 'src/app/shared/models/error-request.Model';
import { LoggingService } from 'src/app/shared/services/logging.service';

const source = 'AdminApp';

@Injectable()
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
