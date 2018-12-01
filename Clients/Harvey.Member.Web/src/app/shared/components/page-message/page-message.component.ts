import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {MessageCode} from '../../models/page-message.model';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-page-message',
  templateUrl: './page-message.component.html',
  styleUrls: ['./page-message.component.scss']
})
export class PageMessageComponent implements OnInit {
  messageContent = '';


  constructor(private router: Router,
              private route: ActivatedRoute,
              private translateService: TranslateService) {
  }

  ngOnInit() {
    const messageCode = this.route.snapshot.paramMap.get('code');
    switch (messageCode) {
      case MessageCode.newUserRegistration: {
        this.translateService.get('MESSAGE.new-user-registration').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
      case MessageCode.userNotUpdateProfile: {
        this.translateService.get('MESSAGE.user-not-complete-profile').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
      case MessageCode.forgotPassword: {
        this.translateService.get('MESSAGE.reset-password').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
      case MessageCode.renewPasswordForMigrateUser: {
        this.translateService.get('MESSAGE.migrate-user-renew-password').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
      case MessageCode.signUp: {
        this.translateService.get('MESSAGE.sign-up-success').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
      case MessageCode.changePhoneNumber: {
        this.translateService.get('MESSAGE.change-phone-success').subscribe((message: string) => {
          this.messageContent = message;
        });
        break;
      }
    }
  }

  backToSignIn() {
    this.router.navigate(['/auth/login']);
  }
}
