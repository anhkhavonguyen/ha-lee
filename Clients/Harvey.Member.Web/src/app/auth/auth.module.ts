import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from './component/login/login.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TranslateModule} from '@ngx-translate/core';
import {SignUpComponent} from './component/sign-up/sign-up.component';
import {AuthRoutingModule} from './auth-routing.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {ResetPasswordComponent} from './component/reset-password/reset-password.component';
import {ForgotPasswordComponent} from './component/forgot-password/forgot-password.component';
import {PageMessageComponent} from '../shared/components/page-message/page-message.component';

@NgModule({
  imports: [
    CommonModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    AuthRoutingModule
  ],
  declarations: [
    LoginComponent,
    SignUpComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    PageMessageComponent]
})
export class AuthModule {
}
