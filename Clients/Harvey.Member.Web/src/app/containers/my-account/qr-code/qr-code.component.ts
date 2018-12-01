import { Component, OnInit } from '@angular/core';
import { MyAccountService } from '../my-account.service';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-qr-code',
  templateUrl: './qr-code.component.html',
  styleUrls: ['./qr-code.component.scss']
})
export class QrCodeComponent implements OnInit {
  public avatar = 'avatar';
  public customerCode: string;
  public displayPicture: string;
  public defaultPicture = '/assets/img/icon/tog_profile_icon.png';
  constructor(private accountService: MyAccountService,
    private router: Router,
    private oauthService: OAuthService,
    private toast: ToastrService,
    private authStorage: OAuthStorage) {
    if (!this.oauthService.hasValidAccessToken()) {
      this.authStorage.removeItem('access_token');
      this.authStorage.removeItem('expires_at');
      localStorage.removeItem(this.avatar);
      this.toast.warning('Your session has timed out. Please sign in again');
      this.router.navigate(['/auth/login']);
    }
  }

  ngOnInit() {
    this.accountService.getUserCodeById().subscribe(result => {
      this.customerCode = result;
    });
    this.getSourceAvatar();
  }

  getSourceAvatar() {
    this.displayPicture = localStorage.getItem(this.avatar);

    this.accountService.getUserProfile().subscribe((result: any) => {
      if (result.avatar) {
        const tempDisplayPicture = `data:image/jpg;base64,${result.avatar}`;
        if (tempDisplayPicture !== this.displayPicture) {
          localStorage.setItem(this.avatar, tempDisplayPicture);
        }
      } else {
        localStorage.setItem(this.avatar, this.defaultPicture);
      }
      this.displayPicture = localStorage.getItem(this.avatar);
    });
  }
}
