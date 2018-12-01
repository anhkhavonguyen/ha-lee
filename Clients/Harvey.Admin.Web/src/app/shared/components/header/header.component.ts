import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from 'src/app/shared/components/change-password/change-password.component';
import { AppSettingsService } from '../../services/app-settings.service';
import { Subscription } from 'rxjs';
import { AppSettingLabelConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  pushRightClass = 'push-right';
  @Input()
  userName!: string;
  constructor(
    public router: Router,
    private authService: AuthService,
    private appSettingsService: AppSettingsService,
    private modalService: NgbModal) {
    this.router.events.subscribe(val => {
      if (
        val instanceof NavigationEnd &&
        window.innerWidth <= 992 &&
        this.isToggled()
      ) {
        this.toggleSidebar();
      }
    });

    this.headerLogoUrl = this.getHeaderLogoUrl(this.appSettingsService.appSettingsDataByAdminType);
    this.appSettingsDataSubcription = this.appSettingsService.appSettingsDataByAdminTypeSubject.subscribe(data => {
      this.headerLogoUrl = this.getHeaderLogoUrl(data);
    });
  }

  public appSettingsDataSubcription = new Subscription();
  public headerLogoUrl = '';

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.appSettingsDataSubcription.unsubscribe();
  }

  isToggled(): boolean {
    const dom = document.querySelector('body');
    return dom ? dom.classList.contains(this.pushRightClass) : false;
  }

  toggleSidebar() {
    const dom: any = document.querySelector('body');
    dom.classList.toggle(this.pushRightClass);
  }

  onLoggedout() {
    this.authService.logout();
  }

  onChangePassword() {
    this.modalService.open(ChangePasswordComponent, { size: 'lg', centered: true, backdrop: 'static' });
  }

  private getHeaderLogoUrl(appSettingsData: any) {
    if (appSettingsData && Array.isArray(appSettingsData)) {
      const appHeaderLogo = appSettingsData.find((x: any) => x.name === AppSettingLabelConstants.AdminAppHeaderLogoImageText);
      if (appHeaderLogo && appHeaderLogo.value) {
        return `data:image/jpg;base64,${appHeaderLogo.value}`;
      }
    }

    return '';
  }
}
