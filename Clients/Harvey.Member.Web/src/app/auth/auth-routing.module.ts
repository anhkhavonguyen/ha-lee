import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './component/login/login.component';
import {SignUpComponent} from './component/sign-up/sign-up.component';
import {ResetPasswordComponent} from './component/reset-password/reset-password.component';
import {ForgotPasswordComponent} from './component/forgot-password/forgot-password.component';
import {PageMessageComponent} from '../shared/components/page-message/page-message.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'login/:term',
    component: LoginComponent
  },
  {
    path: 'signup/:code',
    component: SignUpComponent
  },
  {
    path: 'reset-pwd/:code',
    component: ResetPasswordComponent
  },
  {
    path: 'forgot-pwd',
    component: ForgotPasswordComponent
  },
  {
    path: 'messages/:code',
    component: PageMessageComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {
}
