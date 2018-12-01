import { Component, Input, OnInit } from '@angular/core';
import { Outlet } from '../../models/outlet.model';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { CommonConstants } from '../../constants/common.constant';
import { Subscription } from 'rxjs';
import { AppSettingService } from '../../services/app-setting.service';

@Component({
  selector: 'app-outlet-info-card',
  templateUrl: './outlet-info-card.component.html',
  styleUrls: ['./outlet-info-card.component.scss']
})
export class OutletInfoCardComponent implements OnInit {

  @Input() ouletInfo: Outlet;
  @Input() outletImageDefault: string;
  public appSettings: any;
  public appSettingsDataSubcription: Subscription;
  constructor() {
  }

  ngOnInit() {
    if (!this.ouletInfo.outletImage) {
      this.ouletInfo.outletImage = this.outletImageDefault;
    } else {
      this.ouletInfo.outletImage =  `data:image/jpg;base64,${this.ouletInfo.outletImage}`;
    }
  }
}
