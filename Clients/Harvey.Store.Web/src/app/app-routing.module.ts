import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/component/login/login.component';
import { ServingPageComponent } from './containers/serving-page/serving-page.component';
import { CustomerProfilePageComponent } from './containers/customer-profile-page/customer-profile-page.component';
import { HomePageComponent } from './containers/home-page/home-page.component';
import { CustomerListingPageComponent } from './containers/customer-listing-page/customer-listing-page.component';
import { ChangePasswordPageComponent } from './containers/change-password-page/change-password-page.component';
import { AuthGuardService } from './auth/auth-guard.service';
import { PageName } from '../app/shared/constants/routing.constant';
import { TransactionsHistoryComponent } from './containers/transactions-history/transactions-history.component';
import { TermsAndPrivacyPageComponent } from './containers/terms-and-privacy-page/terms-and-privacy-page.component';
import { DashboardPageComponent } from './containers/dashboard-page/dashboard-page.component';
import { UpdateCustomerProfilePageComponent } from './containers/update-customer-profile-page/update-customer-profile-page.component';

const appRoutes: Routes = [
  { path: PageName.LOGIN_PAGE, component: LoginComponent },
  { path: '', redirectTo: PageName.LOGIN_PAGE, pathMatch: 'full', canActivate: [AuthGuardService] },
  { path: PageName.HOME_PAGE, component: HomePageComponent, canActivate: [AuthGuardService] },
  { path: PageName.SERVING_PAGE, component: ServingPageComponent, canActivate: [AuthGuardService] },
  { path: PageName.CUSTOMER_PROFILE_PAGE, component: CustomerProfilePageComponent, canActivate: [AuthGuardService] },
  {
    path: PageName.CUSTOMER_PROFILE_PAGE + '/:uid',
    component: CustomerProfilePageComponent,
    canActivate: [AuthGuardService]
  },
  { path: PageName.CUSTOMER_LISTING_PAGE, component: CustomerListingPageComponent, canActivate: [AuthGuardService] },
  { path: PageName.CHANGE_PASSWORD_PAGE, component: ChangePasswordPageComponent, canActivate: [AuthGuardService] },
  { path: PageName.TRANSACTIONS_HISTORY_PAGE, component: TransactionsHistoryComponent, canActivate: [AuthGuardService] },
  { path: PageName.TERMS_AND_PRIVACY_PAGE, component: TermsAndPrivacyPageComponent, canActivate: [AuthGuardService] },
  {
    path: PageName.TERMS_AND_PRIVACY_PAGE + '/:phone',
    component: TermsAndPrivacyPageComponent,
    canActivate: [AuthGuardService]
  },
  { path: PageName.DASHBOARD_PAGE, component: DashboardPageComponent, canActivate: [AuthGuardService] },
  { path: PageName.UPDATE_CUSTOMER_PROFILE_PAGE, component: UpdateCustomerProfilePageComponent, canActivate: [AuthGuardService] },
  {
    path: PageName.UPDATE_CUSTOMER_PROFILE_PAGE + '/:uid',
    component: UpdateCustomerProfilePageComponent,
    canActivate: [AuthGuardService]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
