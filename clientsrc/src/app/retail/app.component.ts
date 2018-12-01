import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ItemNavbar } from 'src/app/shared/components/navbar/nav-item.model';
import { Store, select } from '@ngrx/store';
import { AuthService } from 'src/app/shared/services/auth.service';
import * as fromRoot from '../shared/state/app.state';
import * as fromAuths from '../shared/components/auth/state';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  constructor(
    private oauthService: AuthService,
    private store: Store<fromRoot.State>
  ) {
    this.configureWithNewConfigApi();
  }

  MenuItems: ItemNavbar[] = [{ label: 'home-retail', url: 'welcome' }];
  title = 'RETAIL APP';
  userName = '';

  ngOnInit() {
    this.store
      .pipe(select(fromAuths.getUserName))
      .subscribe(userName => (this.userName = userName));
  }

  private configureWithNewConfigApi() {
    this.oauthService.configureWithNewConfigApi();
    this.oauthService.login();
  }
}
