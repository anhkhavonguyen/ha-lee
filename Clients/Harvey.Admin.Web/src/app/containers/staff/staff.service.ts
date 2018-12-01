import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '../../shared/services/http.service';
import { GetStaffRequest, GetStaffReponse, GetStaffsByOutletRequest, GetStaffsByOutletResponse, UserRole } from './staff.model';
import { environment } from '../../../environments/environment';
import HttpParamsHelper from '../../shared/helper/http-params.helper';

const StaffResourceUrl = 'api/staffs/gets';
const StaffByOutlet = 'api/staffs/getsByOutlet';
const userRole = 'api/get-roles-current-user';
@Injectable()
export class StaffService {

    private apiUrl = `${environment.apiGatewayUri}/${StaffResourceUrl}`;

    constructor(private httpService: HttpService) { }

    getStaffs(request: GetStaffRequest): Observable<GetStaffReponse> {
        return this.httpService.get(this.apiUrl, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }

    getStaffsByOutlet(request: GetStaffsByOutletRequest): Observable<GetStaffsByOutletResponse> {
        const url = `${environment.apiGatewayUri}/${StaffByOutlet}`;
        return this.httpService.get(url, { params: HttpParamsHelper.parseObjectToHttpParams(request)});
    }

    public getUserRole(): Observable<UserRole> {
        const url = `${environment.apiGatewayUri}/${userRole}`;
        return this.httpService.get( url, undefined);
    }
}
