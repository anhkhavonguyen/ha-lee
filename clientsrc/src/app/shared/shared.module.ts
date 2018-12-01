import {NgModule} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { ListViewManagementComponent } from 'src/app/shared/components/list-view-management/list-view-management.component';
import { CommonModule } from '@angular/common';
import { BootstrapModule } from 'src/app/shared/bootstrap.module';
import { ListViewComponent } from 'src/app/shared/components/list-view/list-view.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ClientStorageService } from 'src/app/shared/services/client-storage.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NavbarComponent } from 'src/app/shared/components/navbar/navbar.component';
import { SidebarComponent } from 'src/app/shared/components/sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { NgbModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrModule } from 'ngx-toastr';
import { NotificationService } from './services/notification.service';
import { EntityAssignmentComponent } from './components/entity-assignment/entity-assignment.component';
import { NgDragDropModule } from 'ng-drag-drop';

@NgModule({
  imports: [
    NgSelectModule,
    RouterModule,
    FormsModule,
    FormsModule,
    CommonModule,
    BootstrapModule,
    NgxDatatableModule,
    CommonModule,
    NgbModule.forRoot(),
    NgbDropdownModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      disableTimeOut: false,
      tapToDismiss: false,
      toastClass: 'toast',
      closeButton: true,
    }),
    NgDragDropModule.forRoot(),
  ],
  exports: [
    ListViewManagementComponent,
    ListViewComponent,
    NavbarComponent,
    SidebarComponent,
    EntityAssignmentComponent
  ],
  declarations: [
    ListViewManagementComponent,
    ListViewComponent,
    NavbarComponent,
    SidebarComponent,
    EntityAssignmentComponent
  ],
  providers: [
    ClientStorageService,
    AuthService,
    NotificationService
  ],
  entryComponents: [
  ]
})
export class SharedModule {

}
