import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-my-account-navigation-button',
  templateUrl: './my-account-navigation-button.component.html',
  styleUrls: ['./my-account-navigation-button.component.scss']
})
export class MyAccountNavigationButtonComponent implements OnInit {

  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  navigateToHome() {
    this.router.navigate(['/app/home']);
  }

  navigateToAccount() {
    this.router.navigate(['/app/my-account']);
  }

  navigateToEditProfile() {
    this.router.navigate(['/app/edit-profile']);
  }

  navigateToQRCode() {
    this.router.navigate(['/app/my-qr']);
  }
}
