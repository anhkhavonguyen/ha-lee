import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/auth/auth.service';
import { HttpRequest } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

const NotificationMessageConstant = {
    errorMessage: 'Something went wrong. Please try again!',

};

@Injectable()
export class HttpService {
    constructor(
        private authService: AuthService,
        private httpClient: HttpClient,
        private toast: ToastrService) {
    }

    public isLoading = false;
    public isLoadingSubcription = new Subject<any>();
    public readonly authorizationKey = 'Authorization';
    public readonly tokenType = 'Bearer';

    private setDefaultOptions(options?: any) {
        options = options ? options : {};
        if (!options.headers) {
            const headers = new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + this.authService.getTokenFromStorage()
                });
            options.headers = headers;
        }
        return options;
    }

    public get(url: string, options?: any): Observable<any> {
        options = this.setDefaultOptions(options);
        return this.httpClient.get(url, options)
            .pipe(
            catchError((error: HttpErrorResponse) => this.catchHttpError(error))
            );
    }

    public post(url: string, data: any, options?: any): Observable<any> {
        options = this.setDefaultOptions(options);
        const body = JSON.stringify(data);
        return this.httpClient.post(url, body, options)
            .pipe(
            catchError((error: HttpErrorResponse) => this.catchHttpError(error))
            );
    }

    public put(url: string, data: any, options?: any): Observable<any> {
        options = this.setDefaultOptions(options);
        const body = JSON.stringify(data);
        return this.httpClient.put(url, body, options).pipe(
            catchError((error: HttpErrorResponse) => this.catchHttpError(error))
        );
    }

    public postFormData(url: string, formData: FormData, options?: any): Observable<any> {
        if (!options.headers) {
            const headers = new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getTokenFromStorage() });
            options.headers = headers;
        }
        const uploadReq = new HttpRequest('POST', url, formData, options);
        return this.httpClient.request(uploadReq).pipe(
            catchError((error: HttpErrorResponse) => this.catchHttpError(error))
        );
    }

    public delete(url: string, options?: any): Observable<any> {
        options = this.setDefaultOptions(options);
        return this.httpClient.delete(url, options).pipe(
            catchError((error: HttpErrorResponse) => this.catchHttpError(error))
        );
    }

    resetLoadingState() {
        this.isLoadingSubcription.next(this.isLoading);
    }

    getLoadingState(): Observable<any> {
        return this.isLoadingSubcription.asObservable();
    }

    private catchHttpError(errorResponse: any) {
        if (errorResponse instanceof HttpErrorResponse) {
            this.httpErrorResponseHanlder(errorResponse);
        } else if (errorResponse instanceof String) {
            this.toast.error(errorResponse.toString());
        } else if (errorResponse instanceof Error) {
            const errorAsError: Error = errorResponse as Error;
            if (errorAsError.name !== undefined && errorAsError.message !== undefined) {
                const errorMsg = errorAsError.message ? errorAsError.message : errorResponse.toString();
                this.toast.error(errorMsg);
            }
        } else {
            this.toast.error(NotificationMessageConstant.errorMessage);
        }
        this.resetLoadingState();
        return errorResponse;
    }

    private httpErrorResponseHanlder(httpErrorResponse: HttpErrorResponse) {
        switch (httpErrorResponse.status) {
            case 0:
            case 500:
                this.toast.error(NotificationMessageConstant.errorMessage);
                break;
            default:
                this.toast.error(httpErrorResponse.error.message, httpErrorResponse.statusText);
                break;
        }
    }
}
