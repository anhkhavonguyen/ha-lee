import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/shared/services/http.service';
import { environment } from 'src/environments/environment';
import { ChangePasswordRequest } from 'src/app/shared/components/change-password/change-password.model';

const ChangePasswordResourceUrl = 'api/account/change-password';

@Injectable()
export class ChangePasswordService {
    constructor(private httpService: HttpService) {
    }

    private apiUrl = `${environment.apiGatewayUri}/${ChangePasswordResourceUrl}`;

    public ChangePassword(request: ChangePasswordRequest) {
        return this.httpService.put(this.apiUrl, request);
    }
}
