import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from 'src/app/auth/login/login.component';
import {HomepageComponent} from 'src/app/containers/homepage/homepage.component';
import {CustomerComponent} from 'src/app/containers/customer/customer.component';
import {StaffComponent} from 'src/app/containers/staff/staff.component';
import {OutletComponent} from 'src/app/containers/outlet/outlet.component';
import {CustomerDetailComponent} from 'src/app/containers/customer/customer-detail/customer-detail.component';
import {AuthGuardService} from 'src/app/auth/auth-guard.service';
import {SmsReportComponent} from 'src/app/containers/sms-report/sms-report.component';
import {ErrorLogComponent} from 'src/app/containers/error-log/error-log.component';
import {SettingsComponent} from './containers/settings/settings.component';
import { ActivityComponent } from './containers/activity/activity.component';

const routes: Routes = [
  {path: '', redirectTo: '/homepage', pathMatch: 'full', canActivate: [AuthGuardService]},
  {path: 'login', component: LoginComponent},
  {path: 'homepage', component: HomepageComponent, canActivate: [AuthGuardService]},
  {path: 'staff', component: StaffComponent, canActivate: [AuthGuardService]},
  {path: 'customer', component: CustomerComponent, canActivate: [AuthGuardService]},
  {path: 'outlet', component: OutletComponent, canActivate: [AuthGuardService]},
  {path: 'customer-detail/:customerId/:customerCode', component: CustomerDetailComponent, canActivate: [AuthGuardService]},
  {path: 'sms-report', component: SmsReportComponent, canActivate: [AuthGuardService]},
  {path: 'error-log', component: ErrorLogComponent, canActivate: [AuthGuardService]},
  {path: 'settings', component: SettingsComponent, canActivate: [AuthGuardService]},
  {path: 'activity', component: ActivityComponent, canActivate: [AuthGuardService]},
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule {
}
