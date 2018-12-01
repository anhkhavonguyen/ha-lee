import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyAccountComponent } from './my-account/my-account.component';
import { EditUserProfileComponent } from './my-account/edit-user-profile/edit-user-profile.component';
import { ChangePhoneComponent } from './my-account/change-phone/change-phone.component';
import { HomeComponent } from './my-account/home/home.component';
import { QrCodeComponent } from './my-account/qr-code/qr-code.component';

const routes: Routes = [
  {
    path: 'my-account',
    component: MyAccountComponent
  },
  {
    path: 'edit-profile',
    component: EditUserProfileComponent
  },
  {
    path: 'change-phone',
    component: ChangePhoneComponent
  },
  {
    path: 'home',
    children: [
      { path: '', component: HomeComponent },
      { path: 'term-of-use', component: HomeComponent, data: { openPanel: 'term-of-use' } },
      { path: 'privacy-policy', component: HomeComponent, data: { openPanel: 'privacy-policy' } }
    ],
  },
  {
    path: 'my-qr',
    component: QrCodeComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContainersRoutingModule {
}
