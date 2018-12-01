import { Component, OnInit, Input } from '@angular/core';
import { AppSettingModel } from '../../../shared/models/app-settings.model';
import { AppSettingsService } from '../../../shared/services/app-settings.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddAppSettingValueRequest } from './add-setting-value.model';
import { User } from '../../../shared/models/user.model';

@Component({
  selector: 'app-add-setting-value',
  templateUrl: './add-setting-value.component.html',
  styleUrls: ['./add-setting-value.component.scss']
})
export class AddSettingValueComponent implements OnInit {
  user: User = new User();
  addValueForm: FormGroup = new FormGroup({});

  constructor(private appSettingService: AppSettingsService,
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private toast: ToastrService,
    private translate: TranslateService) { }
  ngOnInit() {
    this.userService.getUserProfile().subscribe(res => {
      this.user = res;
    });
    this.addValueForm = this.formBuilder.group({
      name: ['', Validators.required],
      value: ['', Validators.required],
      groupName: ['', Validators.required],
      appSettingTypeName: ['', Validators.required],
    });
  }
  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  updateRequetsValue() {
    const getName = this.addValueForm.get('name');
    const getValue = this.addValueForm.get('value');
    const getGroupName = this.addValueForm.get('groupName');
    const getAppSettingPageTypeName = this.addValueForm.get('appSettingTypeName');
    const request: AddAppSettingValueRequest = {
      userId: this.user.id,
      name: getName ? getName.value : '',
      value: getValue ? getValue.value : '',
      groupName: getGroupName ? getGroupName.value : '',
      appSettingTypeName: getAppSettingPageTypeName ? getAppSettingPageTypeName.value : '',
    };
    this.addNewAppSetting(request);
  }

  addNewAppSetting(request: AddAppSettingValueRequest) {
    this.appSettingService.addAppSettingValue(request).subscribe((e: any) => {
      if (e) {
        this.translate.get('APP.SETTINGS.ADD-SUCCESS').subscribe(message => {
          this.toast.success(message);
          this.activeModal.close(request);
        });
      } else {
        this.translate.get('APP.ERROR.NAME_EXIST').subscribe(message => {
          this.toast.error(message);
        });
      }
    }, error1 => {
      this.translate.get('APP.SETTINGS.ADD-FAILED').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  public onChangeGroupName(value: any) {
    if (value === 'OptionRedeem') {
      this.addValueForm.patchValue({
        appSettingTypeName: 'StoreApp'
      });
    } else if ( value === 'ValidatePhone' ) {
      this.addValueForm.patchValue({
        appSettingTypeName: 'ValidatePhone'
      });
    }
  }
}
