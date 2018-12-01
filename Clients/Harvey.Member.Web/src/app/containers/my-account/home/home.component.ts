import { Component, OnInit } from '@angular/core';
import { MyAccountService } from '../my-account.service';
import { OutletModel } from '../../../shared/models/outlet.model';
import { AppSettingService } from '../../../shared/services/app-setting.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  outletResponse: OutletModel = new OutletModel();
  isLoading = true;
  memberHomeContentHowItWork = '';
  howItWorkHtmlContent: any;
  howItWorkTitle = '';
  memberHomeContentHowGetBenefit = '';
  memberHomeContentTermOfUse = '';
  memberHomeContentPrivacyPolicy = '';
  howGetBenefitHtmlContents: any;
  howGetBenefitHtmlContent = '';
  howGetBenefitTitle = '';
  memberHomeContentContactInfo = '';
  contactInfoHtmlContents: any;
  contactInfoHtmlContent = '';
  contactInfoTitle = '';
  contactInfoOpenTime = '';
  contactInfoPhone = '';
  contactInfoEmail = '';
  contactInfoClock = '';
  termOfUseTitle = '';
  termOfUseContent: any;
  privacyPolicyTitle = '';
  privacyPolicyContent: any;
  openPanel = 'how-it-works';


  private appSettingMemberApp = 3;
  private appSettings: any;

  constructor(private myAccountService: MyAccountService,
    private appSettingService: AppSettingService,
    private toast: ToastrService,
    private translate: TranslateService,
    private route: ActivatedRoute,
    private sanitized: DomSanitizer
  ) {
  }

  ngOnInit() {
    this.route.data.subscribe(result => {
      if (result.openPanel) {
        this.openPanel = result.openPanel;
      }
    });
    this.myAccountService.getAllOutlets().subscribe((response: any) => {
      this.outletResponse = response['outletModels'];
      this.isLoading = false;
    });

    this.appSettingService.getAppSettingByType(this.appSettingMemberApp).subscribe(res => {
      if (res && res.appSettingModels && res.appSettingModels.length) {
        this.appSettings = res;
        this.memberHomeContentHowItWork = this.appSettings.appSettingModels ?
          this.appSettings.appSettingModels.find(a => a.name === 'MemberHomeContentHowItWork').value : '';
        this.memberHomeContentContactInfo = this.appSettings.appSettingModels ?
          this.appSettings.appSettingModels.find(a => a.name === 'MemberHomeContentContactInfo').value : '';
        this.memberHomeContentHowGetBenefit = this.appSettings.appSettingModels ?
          this.appSettings.appSettingModels.find(a => a.name === 'MemberHomeContentHowGetBenefit').value : '';
        this.memberHomeContentTermOfUse = this.appSettings.appSettingModels ?
          this.appSettings.appSettingModels.find(a => a.name === 'MemberHomeContentTermOfUse').value : '';
        this.memberHomeContentPrivacyPolicy = this.appSettings.appSettingModels ?
          this.appSettings.appSettingModels.find(a => a.name === 'MemberHomeContentPrivacyPolicy').value : '';

        if (this.memberHomeContentHowItWork) {
          this.howItWorkTitle = this.memberHomeContentHowItWork.split('|||')[0];
          this.howItWorkHtmlContent = this.memberHomeContentHowItWork.split('|||')[1];
        }
        if (this.memberHomeContentHowGetBenefit) {
          this.howGetBenefitTitle = this.memberHomeContentHowGetBenefit.split('|||')[0];
          this.howGetBenefitHtmlContent = this.memberHomeContentHowGetBenefit.split('|||')[1].replace(/<[^>]+>/gm, '|');
          this.howGetBenefitHtmlContents = this.howGetBenefitHtmlContent.split('|').filter(val => val);
        }
        if (this.memberHomeContentContactInfo) {
          const ContentContactInfo = JSON.parse(this.memberHomeContentContactInfo);
          this.contactInfoTitle = ContentContactInfo.title;
          this.contactInfoPhone = ContentContactInfo.phone;
          this.contactInfoOpenTime = ContentContactInfo.openTime;
          this.contactInfoEmail = ContentContactInfo.email;
        }
        if (this.memberHomeContentHowItWork) {
          this.howItWorkTitle = this.memberHomeContentHowItWork.split('|||')[0];
          this.howItWorkHtmlContent = this.sanitized.bypassSecurityTrustHtml(this.memberHomeContentHowItWork.split('|||')[1]);
        }
        if (this.memberHomeContentHowGetBenefit) {
          this.howGetBenefitTitle = this.memberHomeContentHowGetBenefit.split('|||')[0];
          this.howGetBenefitHtmlContent = this.memberHomeContentHowGetBenefit.split('|||')[1].replace(/<[^>]+>/gm, '|');
          this.howGetBenefitHtmlContents = this.howGetBenefitHtmlContent.split('|').filter(val => val);
        }
        if (this.memberHomeContentTermOfUse) {
          this.termOfUseTitle = this.memberHomeContentTermOfUse.split('|||')[0];
          this.termOfUseContent = this.memberHomeContentTermOfUse.split('|||')[1];
        }
        if (this.memberHomeContentPrivacyPolicy) {
          this.privacyPolicyTitle = this.memberHomeContentPrivacyPolicy.split('|||')[0];
          this.privacyPolicyContent = this.memberHomeContentPrivacyPolicy.split('|||')[1];
        }
      }
    }, error => {
      this.translate.get('ERROR.try-again').subscribe(message => {
        this.toast.error(message);
      });
    });
  }
}
