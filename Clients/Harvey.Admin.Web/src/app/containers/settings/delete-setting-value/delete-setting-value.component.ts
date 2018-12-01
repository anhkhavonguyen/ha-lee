import { Component, OnInit, Input } from '@angular/core';
import { AppSettingModel, AppSettingUpdateValue, AppSettingDeleteRequest } from '../../../shared/models/app-settings.model';
import { AppSettingsService } from '../../../shared/services/app-settings.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from '../../../shared/models/user.model';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-delete-setting-value',
  templateUrl: './delete-setting-value.component.html',
  styleUrls: ['./delete-setting-value.component.scss']
})
export class DeleteSettingValueComponent implements OnInit {

  @Input() appSettingValue!: AppSettingModel;
  user: User = new User();
  constructor(private appSettingService: AppSettingsService,
    private activeModal: NgbActiveModal,
    private userService: UserService,
    private toast: ToastrService,
    private translate: TranslateService) { }

  ngOnInit() {
    this.userService.getUserProfile().subscribe(res => {
      this.user = res;
    });
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  deleteValue() {
    const request: AppSettingDeleteRequest = {
      userId: this.user.id,
      userName: `${this.user.firstName} ${this.user.lastName}`,
      appSettingId: this.appSettingValue.id.toString()
    };
    this.appSettingService.deleteAppSetting(request).subscribe(() => {
      this.translate.get('APP.SETTINGS.DELETE-SUCCESS').subscribe(message => {
        this.toast.success(message);
        this.activeModal.close();
      });
    },
      error => {
        this.translate.get('APP.SETTINGS.DELETE-FAILED').subscribe(message => {
          this.toast.success(message);
          this.activeModal.close();
        });
      });
  }
}
