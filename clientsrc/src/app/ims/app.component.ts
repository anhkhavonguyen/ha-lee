import { Component, ViewEncapsulation, OnInit } from '@angular/core';
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

  MenuItems = [
    {
      label: 'Products',
      url: 'products',
      subMenu: [
        { label: 'Products', url: 'products' },
        { label: 'Categories', url: 'categories' },
        { label: 'Brands', url: 'brands' },
        { label: 'Fields', url: 'fields' },
        { label: 'Field Templates', url: 'field-templates' },
        { label: 'Assortments', url: 'assortments' },
        { label: 'Locations', url: 'locations' },
        { label: 'Channels', url: 'channels' },
        { label: 'GIW(Mock Transfer)', url: 'allocations' }]
    },
    {
      label: 'Admin',
      url: 'activities',
      subMenu: [{ label: 'Activities', url: 'activities' }]
    }
  ];

  ListItem: any = [];
  activeLink = 'Products';
  title = 'IMS APP';
  userName = '';
  logo = '/assets/img/300_dpi_white.png';

  ngOnInit() {
    this.getSubmenu();
    this.store
      .pipe(select(fromAuths.getUserName))
      .subscribe(userName => (this.userName = userName));
  }

  getSubmenu() {
    for (const item of this.MenuItems) {
      if (item.label === this.activeLink) {
        this.ListItem = item.subMenu;
        break;
      }
    }
  }

  private configureWithNewConfigApi() {
    this.oauthService.configureWithNewConfigApi();
    this.oauthService.login();
  }

  switchFeature(e: any) {
    this.activeLink = e;
    this.getSubmenu();
  }
}
