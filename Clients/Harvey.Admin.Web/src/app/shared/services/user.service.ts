import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import HttpParamsHelper from '../helper/http-params.helper';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

const GetUserProfileUrl = 'api/Account/get-user-profile';

@Injectable()
export class UserService {
    constructor(private httpService: HttpService) {

    }
    public getUserProfile(): Observable<User> {
        const apiUrl = `${environment.apiGatewayUri}/${GetUserProfileUrl}`;
        return this.httpService.get(apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams()});
    }
}
