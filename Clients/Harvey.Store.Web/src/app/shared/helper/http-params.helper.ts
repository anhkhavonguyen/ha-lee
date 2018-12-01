import { HttpParams } from '@angular/common/http';

export default class HttpParamsHelper {
  public static parseObjectToHttpParams(data?: any) {
    let httpParams = new HttpParams();
    if (data) {
      Object.keys(data).forEach(function (key) {
        if (Array.isArray(data[key])) {
          data[key].forEach(item => {
            httpParams = httpParams.append(key, item);
          });
        } else {
          httpParams = httpParams.append(key, data[key]);
        }
      });
    }

    return httpParams;
  }
}
