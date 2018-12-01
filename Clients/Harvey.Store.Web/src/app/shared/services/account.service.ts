import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable()
export class AccountService {

  constructor(private http: HttpClient) {
  }

  public getTime(): Observable<any> {
    const apiURL = environment.apiGatewayUri + '/api/account/GetTime';
    const headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(apiURL, { headers });
  }
}
