import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSettingModel, AppSettingUpdateValue } from '../../../shared/models/app-settings.model';
import { UserService } from '../../../shared/services/user.service';
import { User } from '../../../shared/models/user.model';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { CommonConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-update-setting-value',
  templateUrl: './update-setting-value.component.html',
  styleUrls: ['./update-setting-value.component.scss']
})
export class UpdateSettingValueComponent implements OnInit {

  updateValueForm: FormGroup = new FormGroup({});
  @Input() appSettingValue!: AppSettingModel;
  @Input() isAppSettingIcon!: boolean;
  @Input() isAppSettingBooleanValue!: boolean;
  @Input() isAppSettingHtmlContent!: boolean;
  @Input() isAppSettingMemberContactInfo!: boolean;
  @Input() isAppSettingColor!: boolean;
  @Input() memberHomeContentHowGetBenefit!: string;
  @Input() listAppSettings!: any;

  user: User = new User();
  isUpdate = false;
  public icon = '';
  public displayIcon = '';
  public booleanValue = false;
  public memberHomeContent = '';
  public memberHomeHtmlContent = '';
  public memberHomeTitle = '';
  public memberHomeContentContactInfo = '';

  public appSettingColorValue = {
    backgroundColor: '',
    textColor: ''
  };

  editorConfig = {
    editable: true,
    spellcheck: false,
    height: '10rem',
    minHeight: '5rem',
    placeholder: 'Please input the content',
    translate: 'no'
  };

  constructor(private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private appSettingService: AppSettingsService,
    private userService: UserService,
    private toast: ToastrService,
    private translate: TranslateService) {
  }

  ngOnInit() {
    if (this.isAppSettingHtmlContent || this.isAppSettingMemberContactInfo) {
      this.memberHomeContent = this.listAppSettings ?
        this.listAppSettings.find((a: any) => a.name === this.appSettingValue.name).value : '';
    }
    if (this.isAppSettingIcon) {
      this.icon = this.appSettingValue.value;
      this.displayIcon = `data:image/png;base64,${this.icon}`;
    }
    if (this.isAppSettingBooleanValue) {
      this.booleanValue = this.appSettingValue.value === 'true' ? true : false;
    }
    if (this.isAppSettingColor) {
      const appSettingColorValue = JSON.parse(this.appSettingValue.value);
      const backgroundColor = appSettingColorValue['background-color'];
      const textColor = appSettingColorValue['color'];

      this.appSettingColorValue.backgroundColor = backgroundColor;
      this.appSettingColorValue.textColor = textColor;
    }
    this.userService.getUserProfile().subscribe(res => {
      this.user = res;
    });

    if (!this.isAppSettingHtmlContent && !this.isAppSettingMemberContactInfo) {
      this.updateValueForm = this.formBuilder.group({
        name: [{ value: '', disabled: true }, Validators.required],
        value: ['', Validators.required],
        comment: '',
        title: ''
      });

      this.updateValueForm.patchValue({
        name: this.appSettingValue.name,
        value: this.appSettingValue.value
      });
    } else if (this.isAppSettingHtmlContent) {
      this.updateValueForm = this.formBuilder.group({
        name: [{ value: '', disabled: true }, Validators.required],
        title: ['', Validators.required]
      });
      this.memberHomeTitle = this.memberHomeContent.split('|||')[0];
      this.memberHomeHtmlContent = this.memberHomeContent.split('|||')[1];
      this.updateValueForm.patchValue({
        name: this.appSettingValue.name,
        title: this.memberHomeTitle
      });
    } else if (this.isAppSettingMemberContactInfo) {
      this.updateValueForm = this.formBuilder.group({
        name: [{ value: '', disabled: true }, Validators.required],
        title: ['', Validators.required],
        phone: ['', Validators.required],
        openTime: ['', Validators.required],
        email: ['', Validators.required]
      });
      const memberHomeContentContactInfo = JSON.parse(this.memberHomeContent);
      this.updateValueForm.patchValue({
        name: this.appSettingValue.name,
        title: memberHomeContentContactInfo.title,
        phone: memberHomeContentContactInfo.phone,
        openTime: memberHomeContentContactInfo.openTime.replace(/<br\s*\/?>/mg, '\n'),
        email: memberHomeContentContactInfo.email
      });
    }
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onSwitchButtonClick() {
    this.booleanValue = !this.booleanValue;
  }

  updateValue() {
    this.isUpdate = true;
    if (this.isAppSettingIcon) {
      this.updateIconValue();
    }
    if (this.isAppSettingBooleanValue) {
      this.updateBooleanValue();
    }
    if (this.isAppSettingHtmlContent) {
      this.updateHtmlContentValue();
    }
    if (this.isAppSettingMemberContactInfo) {
      this.updateMemberContactInfo();
    }
    if (this.isAppSettingColor) {
      this.updateAppSettingButtonColor();
    }
    if (this.checkStringValue()) {
      this.updateStringValue();
    }
  }

  updateStringValue() {
    const getValue = this.updateValueForm.get('value');
    const value = getValue ? getValue.value : '';
    if (value !== this.appSettingValue.value) {
      const request: AppSettingUpdateValue = {
        userId: this.user.id,
        id: this.appSettingValue.id.toString(),
        value: getValue ? getValue.value : '',
        comment: `Old value: ${this.appSettingValue.value} - Update value: ${getValue ? getValue.value : ''}`
      };
      this.updateAppSettingValue(request);
    } else {
      this.translate.get('APP.SETTINGS.UPDATE-NOT-CHANGE').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  updateMemberContactInfo() {
    const getTitle = this.updateValueForm.get('title');
    const getOpenTime = this.updateValueForm.get('openTime');
    const getPhone = this.updateValueForm.get('phone');
    const getEmail = this.updateValueForm.get('email');
    const value = getTitle && getOpenTime && getPhone && getEmail ?
      `{"title":"${getTitle.value}",
     "openTime":"${getOpenTime.value.replace(/\n/g, '<br/>')}",
     "email":"${getEmail.value}",
     "phone":"${getPhone.value}"
    }` : '';
    if (value !== this.appSettingValue.value) {
      const request: AppSettingUpdateValue = {
        userId: this.user.id,
        id: this.appSettingValue.id.toString(),
        value: value,
        comment: `${this.appSettingValue.name} has changed`
      };
      this.updateAppSettingValue(request);
    } else {
      this.translate.get('APP.SETTINGS.UPDATE-NOT-CHANGE').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  updateAppSettingButtonColor() {
    const request: AppSettingUpdateValue = {
      userId: this.user.id,
      id: this.appSettingValue.id.toString(),
      value: JSON.stringify({
        'background-color': this.appSettingColorValue.backgroundColor,
        'color': this.appSettingColorValue.textColor
      }),
      comment: `${this.appSettingValue.name} has changed`
    };

    this.updateAppSettingValue(request);
    const appSettingModels = this.appSettingService.appSettingModelsByStoreType;
    if (!Array.isArray(appSettingModels)) {
      return '';
    }

    const appSettingButtonColor = appSettingModels.find(x => (x.id === this.appSettingValue.id.toString()));
    appSettingButtonColor.value = JSON.stringify({
      'background-color': this.appSettingColorValue.backgroundColor,
      'color': this.appSettingColorValue.textColor
    });
    this.appSettingService.triggerAppSettingsDataByStoreType(appSettingModels);
  }

  updateHtmlContentValue() {
    const getValue = this.updateValueForm.get('title');
    const value = getValue ? `${getValue.value}|||${this.memberHomeHtmlContent}` : '';
    if (value !== this.appSettingValue.value) {
      const request: AppSettingUpdateValue = {
        userId: this.user.id,
        id: this.appSettingValue.id.toString(),
        value: value,
        comment: `${this.appSettingValue.name} has changed`
      };
      this.updateAppSettingValue(request);
    } else {
      this.translate.get('APP.SETTINGS.UPDATE-NOT-CHANGE').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  updateIconValue() {
    if (this.icon !== this.appSettingValue.value) {
      const request: AppSettingUpdateValue = {
        userId: this.user.id,
        id: this.appSettingValue.id.toString(),
        value: this.icon,
        comment: `${this.appSettingValue.name} has changed`
      };
      this.updateAppSettingValue(request);
    } else {
      this.translate.get('APP.SETTINGS.UPDATE-NOT-CHANGE').subscribe(message => {
        this.toast.error(message);
      });
    }
  }

  updateBooleanValue() {
    if (this.appSettingValue.value !== this.booleanValue.toString()) {
      const value = this.booleanValue ? 'true' : 'false';
      const request: AppSettingUpdateValue = {
        userId: this.user.id,
        id: this.appSettingValue.id.toString(),
        value: value,
        comment: `Old value: ${this.appSettingValue.value} - Update value: ${value}`
      };
      this.updateAppSettingValue(request);
    } else {
      this.translate.get('APP.SETTINGS.UPDATE-NOT-CHANGE').subscribe(message => {
        this.toast.error(message);
      });
    }

  }

  updateAppSettingValue(request: AppSettingUpdateValue) {
    this.appSettingService.updateAppSettingValue(request).subscribe(() => {
      this.isUpdate = false;
      this.translate.get('APP.SETTINGS.UPDATE-SUCCESS').subscribe(message => {
        this.toast.success(message);
        this.activeModal.close(request);
      });
    }, error1 => {
      this.isUpdate = false;
      this.translate.get('APP.SETTINGS.UPDATE-FAILED').subscribe(message => {
        this.toast.error(message);
      });
    });
  }

  checkStringValue() {
    return !this.isAppSettingMemberContactInfo &&
      !this.isAppSettingHtmlContent && !this.isAppSettingBooleanValue && !this.isAppSettingIcon && !this.isAppSettingColor
      ? true : false;
  }

  uploadIcon(fileSelected: any) {
    const imgFile = fileSelected.target.files;
    const file = imgFile[0];
    const fileSize = file.size;
    const fileType = file.type;
    const allowedSize = 2097152;
    const allowedType = 'image';
    if (imgFile && file) {
      if (fileSize <= allowedSize && fileType.includes(allowedType)) {
        const reader = new FileReader();
        reader.onload = this.convertoBase64.bind(this);
        reader.readAsBinaryString(file);
      } else {
        if (fileSize > allowedSize) {
          this.translate.get('APP.ERROR.LIMIT_FILE_SIZE').subscribe(result => {
            this.toast.error(result);
          });
        }
        if (!fileType.includes(allowedType)) {
          this.translate.get('APP.ERROR.ALLOWED_FILE_TYPE').subscribe(result => {
            this.toast.error(result);
          });
        }
      }
    }
  }

  convertoBase64(file: any) {
    const binaryString = file.target.result;
    const convertImg = btoa(binaryString);
    this.icon = convertImg;
    this.displayIcon = `data:image/png;base64,${convertImg}`;
  }

  onChangeColor(colorValue: any) {
    const backgroundColor = colorValue.backgroundColor;
    const textColor = colorValue.textColor;
    if (backgroundColor && backgroundColor) {
      this.appSettingColorValue = {
        backgroundColor: backgroundColor,
        textColor: textColor
      };
    }
  }
}
