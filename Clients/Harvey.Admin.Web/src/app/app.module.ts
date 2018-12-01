import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './/app-routing.module';
import { CustomerComponent } from './containers/customer/customer.component';
import { HomepageComponent } from './containers/homepage/homepage.component';
import { OutletComponent } from './containers/outlet/outlet.component';
import { StaffComponent } from './containers/staff/staff.component';
import { SmsReportComponent } from './containers/sms-report/sms-report.component';
import { LoginComponent } from './auth/login/login.component';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';
import { NgbModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './shared/components/header/header.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CustomerDetailComponent } from './containers/customer/customer-detail/customer-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'src/app/auth/auth.service';
import { HttpService } from 'src/app/shared/services/http.service';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppTranslateService } from 'src/app/shared/services/translate.service';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import { AuthGuardService } from 'src/app/auth/auth-guard.service';
import { ErrorHandler } from '@angular/core';
import { GlobalExceptionHandlerService } from 'src/app/shared/services/global-exception-handler.service';
import { LoggingService } from 'src/app/shared/services/logging.service';
import { UserService } from 'src/app/shared/services/user.service';
import { StaffService } from './containers/staff/staff.service';
import { CustomerService } from 'src/app/containers/customer/customer.service';
import { PagerService } from 'src/app/shared/services/pager.service';
import { CustomerDetailService } from 'src/app/containers/customer/customer-detail/customer-detail.service';
import { OutletService } from './containers/outlet/outlet.service';
import { UploadFileComponent } from 'src/app/shared/components/upload-file/upload-file.component';
import { UploadFileService } from 'src/app/shared/components/upload-file/upload-file.service';
import { ToastrModule } from 'ngx-toastr';
import { ExportCSVService } from 'src/app/shared/services/export-csv.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SMSReportService } from 'src/app/containers/sms-report/sms-report.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { HomepageService } from 'src/app/containers/homepage/homepage.service';
import { LoadingIndicatorComponent } from './shared/components/loading-indicator/loading-indicator.component';
import { ErrorLogComponent } from './containers/error-log/error-log.component';
import { ErrorLogService } from 'src/app/containers/error-log/error-log.service';
import { SetPasswordService } from 'src/app/containers/staff/change-password-staff/set-password-staff.service';
import { SetPasswordStaffComponent } from 'src/app/containers/staff/change-password-staff/set-password-staff.component';
import { ChangePasswordComponent } from './shared/components/change-password/change-password.component';
import { ChangePasswordService } from 'src/app/shared/components/change-password/change-password.service';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { InfoDialogComponent } from './shared/components/info-dialog/info-dialog.component';
import { UpdateSettingValueComponent } from './containers/settings/update-setting-value/update-setting-value.component';
import { SettingsComponent } from './containers/settings/settings.component';
import { EditOutletComponent } from 'src/app/containers/outlet/edit-outlet/edit-outlet.component';
import { ActivityService } from './containers/activity/activity.service';
import { ActivityComponent } from './containers/activity/activity.component';
import { EditCustomerInfoComponent } from './containers/customer/edit-customer-info/edit-customer-info.component';
import { ValidationService } from 'src/app/shared/services/validation.service';
import { ReactiveCustomerComponent } from './containers/customer/reactive-customer/reactive-customer.component';
import { NgxEditorModule } from 'ngx-editor';
import { DeleteSettingValueComponent } from './containers/settings/delete-setting-value/delete-setting-value.component';
import { MemberSummaryComponent } from './containers/homepage/member-summary/member-summary.component';
import { PointSummaryComponent } from './containers/homepage/point-summary/point-summary.component';
import { WalletSummaryComponent } from './containers/homepage/wallet-summary/wallet-summary.component';
import { AddSettingValueComponent } from './containers/settings/add-setting-value/add-setting-value.component';
import { ChartsModule } from 'ng2-charts';
import { StatisticsChartComponent } from './containers/homepage/statistics-chart/statistics-chart.component';
import { PointChartComponent } from './containers/homepage/statistics-chart/point-chart/point-chart.component';
import { WalletChartComponent } from './containers/homepage/statistics-chart/wallet-chart/wallet-chart.component';
import { VisitorChartComponent } from './containers/homepage/statistics-chart/visitor-chart/visitor-chart.component';
// tslint:disable-next-line:max-line-length
import { UpdateSettingColorComponent } from './containers/settings/update-setting-value/update-setting-color/update-setting-color.component';
import { ColorPickerModule } from 'ngx-color-picker';


export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    CustomerComponent,
    HomepageComponent,
    OutletComponent,
    StaffComponent,
    SmsReportComponent,
    LoginComponent,
    SidebarComponent,
    HeaderComponent,
    CustomerDetailComponent,
    UploadFileComponent,
    LoadingIndicatorComponent,
    ErrorLogComponent,
    SetPasswordStaffComponent,
    ChangePasswordComponent,
    ConfirmDialogComponent,
    InfoDialogComponent,
    UpdateSettingValueComponent,
    SettingsComponent,
    EditOutletComponent,
    ActivityComponent,
    EditCustomerInfoComponent,
    ReactiveCustomerComponent,
    DeleteSettingValueComponent,
    MemberSummaryComponent,
    PointSummaryComponent,
    WalletSummaryComponent,
    AddSettingValueComponent,
    StatisticsChartComponent,
    PointChartComponent,
    WalletChartComponent,
    VisitorChartComponent,
    UpdateSettingColorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    NgbDropdownModule.forRoot(),
    NgxDatatableModule,
    FormsModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    ReactiveFormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      closeButton: true,
      positionClass: 'toast-top-center',
      preventDuplicates: true,
      maxOpened: 3,
    }),
    NgSelectModule,
    NgxEditorModule,
    ChartsModule,
    ColorPickerModule
  ],
  providers: [
    AuthService,
    AuthGuardService,
    HttpService,
    AppTranslateService,
    { provide: ErrorHandler, useClass: GlobalExceptionHandlerService },
    LoggingService,
    StaffService,
    UserService,
    CustomerService,
    PagerService,
    OutletService,
    CustomerDetailService,
    { provide: OAuthStorage, useValue: localStorage },
    UploadFileService,
    ExportCSVService,
    SMSReportService,
    HomepageService,
    ErrorLogService,
    SetPasswordService,
    ChangePasswordService,
    ActivityService,
    ValidationService],

  bootstrap: [AppComponent],
  entryComponents: [
    UploadFileComponent,
    SetPasswordStaffComponent,
    ChangePasswordComponent,
    ConfirmDialogComponent,
    InfoDialogComponent,
    UpdateSettingValueComponent,
    EditOutletComponent,
    ReactiveCustomerComponent,
    EditCustomerInfoComponent,
    DeleteSettingValueComponent,
    AddSettingValueComponent]
})
export class AppModule {
}
