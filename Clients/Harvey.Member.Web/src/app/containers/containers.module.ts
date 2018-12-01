import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContainersRoutingModule } from './containers-routing.module';
import { MyAccountComponent } from './my-account/my-account.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditUserProfileComponent } from './my-account/edit-user-profile/edit-user-profile.component';
import { TranslateModule } from '@ngx-translate/core';
import {
  MyAccountNavigationButtonComponent
} from '../shared/components/my-account-navigation-button/my-account-navigation-button.component';
import { ChangePhoneComponent } from './my-account/change-phone/change-phone.component';
import { HomeComponent } from './my-account/home/home.component';
import { LoadingPageComponent } from '../shared/components/loading-page/loading-page.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  CustomPaginatorNgxDatatableComponent
} from '../shared/components/custom-paginator-ngx-datatable/custom-paginator-ngx-datatable.component';
import { NgxQRCodeModule } from 'ngx-qrcode2';
import { QrCodeComponent } from './my-account/qr-code/qr-code.component';

@NgModule({
  imports: [
    CommonModule,
    ContainersRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    NgxDatatableModule,
    NgxQRCodeModule
  ],
  declarations: [MyAccountComponent,
    EditUserProfileComponent,
    MyAccountNavigationButtonComponent,
    ChangePhoneComponent,
    HomeComponent,
    LoadingPageComponent,
    CustomPaginatorNgxDatatableComponent,
    QrCodeComponent],
})
export class ContainersModule {
}
