
import { Injectable } from '@angular/core';
import { HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/shared/services/http.service';
import { environment } from 'src/environments/environment';

const uploadCRMUri = 'api/customers/uploadfile';
const uploadIdsUri = 'api/account/uploadfile';

@Injectable()
export class UploadFileService {
    constructor(private httpService: HttpService) { }
    private apiUrl = `${environment.apiGatewayUri}/${uploadCRMUri}`;
    private apiIdsUrl = `${environment.apiGatewayUri}/${uploadIdsUri}`;
    pushFileToStorage(file: File | null): Observable<HttpEvent<{}>> {
        const formdata: FormData = new FormData();
        if (file) {
            formdata.append('file', file, file.name);
        }

        return this.httpService.postFormData(this.apiUrl, formdata, {
            reportProgress: true,
            responseType: 'text'
        });
    }

    pushFileToIds(file: File | null): Observable<HttpEvent<{}>> {
        const formdata: FormData = new FormData();
        if (file) {
            formdata.append('file', file, file.name);
        }

        return this.httpService.postFormData(this.apiIdsUrl, formdata, {
            reportProgress: true,
            responseType: 'text'
        });
    }

}
