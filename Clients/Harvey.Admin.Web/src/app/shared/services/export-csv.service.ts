import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpService } from 'src/app/shared/services/http.service';
import { Observable } from 'rxjs';

const exportCSVResousce = 'api/customers/exportcsv';

@Injectable()
export class ExportCSVService {

    private apiUrl = `${environment.apiGatewayUri}/${exportCSVResousce}`;

    constructor(private httpService: HttpService) { }

    public ExportCustomerCSV(): Observable<any> {
        return this.httpService.get(this.apiUrl, {responseType: 'text/csv'});
    }
}
