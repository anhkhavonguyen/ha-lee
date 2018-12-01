import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable()
export class HttpService {
  constructor(private httpClient: HttpClient, private authService: AuthService) {
  }

  public readonly authorizationKey = 'Authorization';
  public readonly tokenType = 'Bearer';

  private setDefaultOptions(options?: any) {
    options = options ? options : {};
    if (!options.headers) {
      const headers = new HttpHeaders(
        { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.authService.getTokenFormStorage() });
      options.headers = headers;
    }
    return options;
  }

  public get(url: string, options: any): Observable<any> {
    options = this.setDefaultOptions(options);
    return this.httpClient.get(url, options);
  }

  public post(url: string, data: any, options?: any): Observable<any> {
    options = this.setDefaultOptions(options);
    const body = JSON.stringify(data);
    return this.httpClient.post(url, body, options);
  }

  public put(url: string, data: any, options?: any): Observable<any> {
    options = this.setDefaultOptions(options);
    const body = JSON.stringify(data);
    return this.httpClient.put(url, body, options);
  }

  public delete(url: string, options?: any): Observable<any> {
    options = this.setDefaultOptions(options);
    return this.httpClient.delete(url, options);
  }

  public postFormData(url: string, formData: FormData, options?: any): Observable<any> {
    if (!options.headers) {
      const headers = new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getTokenFormStorage() });
      options.headers = headers;
    }
    const uploadReq = new HttpRequest('POST', url, formData, options);
    return this.httpClient.request(uploadReq);
  }

  private catchAuthError(self: HttpService) {
    return (res: Response) => {
      console.log(res);
      if (res.status === 401 || res.status === 403) {
        console.log(res);
      }
      return throwError(res);
    };
  }
}
