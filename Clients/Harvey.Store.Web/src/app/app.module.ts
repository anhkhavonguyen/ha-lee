import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { LoginComponent } from './auth/component/login/login.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from '../app/app-routing.module';
import { ServingPageComponent } from './containers/serving-page/serving-page.component';
import { NumericVirtualKeyboardComponent } from './shared/components/numeric-virtual-keyboard/numeric-virtual-keyboard.component';
import { CustomerProfilePageComponent } from './containers/customer-profile-page/customer-profile-page.component';
import { BalanceCardComponent } from './shared/components/balance-card/balance-card.component';
import { HomePageComponent } from './containers/home-page/home-page.component';
import { OutletInfoCardComponent } from './shared/components/outlet-info-card/outlet-info-card.component';
import { CustomerListingPageComponent } from './containers/customer-listing-page/customer-listing-page.component';
import { ChangePasswordPageComponent } from './containers/change-password-page/change-password-page.component';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import { HttpService } from './shared/services/http.service';
import { AuthService } from './auth/auth.service';
import { AuthGuardService } from './auth/auth-guard.service';
import { SharedService } from './shared/services/shared.service';
import { UserService } from './shared/services/user.service';
import { CustomerService } from './shared/services/customer.service';
import { EditMembershipComponent } from './containers/customer-profile-page/components/edit-membership/edit-membership.component';
import { AddLoyaltyPointComponent } from './containers/customer-profile-page/components/add-loyalty-point/add-loyalty-point.component';
import { PointTransactionService } from './shared/services/point-transactions.service';
import { RedeemPointComponent } from './containers/customer-profile-page/components/redeem-point/redeem-point.component';
import { LoadingComponent } from './shared/components/loading/loading.component';
import { TransactionsHistoryComponent } from './containers/transactions-history/transactions-history.component';
import { AppSettingService } from './shared/services/app-setting.service';
import { WalletTransactionService } from './shared/services/wallet-transactions.service';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { TopUpWalletComponent } from './containers/customer-profile-page/components/top-up-wallet/top-up-wallet.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TermsAndPrivacyPageComponent } from './containers/terms-and-privacy-page/terms-and-privacy-page.component';
import { SpendWalletComponent } from './containers/customer-profile-page/components/spend-wallet/spend-wallet.component';
import { AccountService } from './shared/services/account.service';
import { DashboardPageComponent } from './containers/dashboard-page/dashboard-page.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { GlobalExceptionHandlerService } from './shared/services/global-exception-handler.service';
import { LoggingService } from './shared/services/logging.service';
import { InfoDialogComponent } from 'src/app/shared/components/info-dialog/info-dialog.component';
import { UpdateCustomerProfilePageComponent } from './containers/update-customer-profile-page/update-customer-profile-page.component';
import {
  CustomPaginatorNgxDatatableComponent
} from './shared/components/custom-paginator-ngx-datatable/custom-paginator-ngx-datatable.component';
import { ChangePhoneDialogComponent } from './shared/components/change-phone-dialog/change-phone-dialog.component';
import { ExpiryPointDialogComponent } from './shared/components/expiry-point-dialog/expiry-point-dialog.component';
import { QrCodeComponent } from './containers/customer-profile-page/components/qr-code/qr-code.component';
import { NgxQRCodeModule } from 'ngx-qrcode2';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ServingPageComponent,
    NumericVirtualKeyboardComponent,
    CustomerProfilePageComponent,
    BalanceCardComponent,
    HomePageComponent,
    OutletInfoCardComponent,
    CustomerListingPageComponent,
    ChangePasswordPageComponent,
    EditMembershipComponent,
    AddLoyaltyPointComponent,
    RedeemPointComponent,
    LoadingComponent,
    TransactionsHistoryComponent,
    TopUpWalletComponent,
    TermsAndPrivacyPageComponent,
    SpendWalletComponent,
    DashboardPageComponent,
    InfoDialogComponent,
    UpdateCustomerProfilePageComponent,
    CustomPaginatorNgxDatatableComponent,
    ChangePhoneDialogComponent,
    ExpiryPointDialogComponent,
    QrCodeComponent
  ],
  imports: [
    BrowserModule,
    NgbModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    OAuthModule.forRoot(),
    ReactiveFormsModule,
    NgxDatatableModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      closeButton: true,
      positionClass: 'toast-top-center',
      preventDuplicates: true,
      maxOpened: 3,
    }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    NgxQRCodeModule
  ],
  providers: [
    HttpService,
    AuthService,
    AuthGuardService,
    SharedService,
    UserService,
    CustomerService,
    PointTransactionService,
    { provide: OAuthStorage, useValue: localStorage },
    { provide: ErrorHandler, useClass: GlobalExceptionHandlerService },
    AppSettingService,
    WalletTransactionService,
    AccountService,
    LoggingService
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    EditMembershipComponent,
    AddLoyaltyPointComponent,
    RedeemPointComponent,
    TopUpWalletComponent,
    SpendWalletComponent,
    InfoDialogComponent,
    ChangePhoneDialogComponent,
    ExpiryPointDialogComponent,
    QrCodeComponent
  ]
})
export class AppModule {
}

export function HttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}
