import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class AppTranslateService {
    public  defaultLang  = 'en';

    constructor(private translateSerive: TranslateService) {
    }

    public setup() {
        this.translateSerive.addLangs(['en']);
        this.translateSerive.setDefaultLang(this.defaultLang);
        const browserLang = this.translateSerive.getBrowserLang();
        const availableLang = this.translateSerive.getLangs();
        this.translateSerive.use(availableLang.includes(browserLang) ? browserLang : this.defaultLang);
    }
}
