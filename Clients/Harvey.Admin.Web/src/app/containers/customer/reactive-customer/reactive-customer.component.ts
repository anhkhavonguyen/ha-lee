import { Component, OnInit, Output, EventEmitter, Input, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { ValidationService } from 'src/app/shared/services/validation.service';
import { FormGroup } from '@angular/forms';
import { blankSpaceValidator, GeneralValidation } from 'src/app/shared/utils/validation.util';
import { ReactiveCustomerRequest } from 'src/app/containers/customer/reactive-customer/reactive-customer.model';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/shared/models/user.model';
import { ValidatePhoneModel } from '../edit-customer-info/validate-phone.model';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { AppSettingLabelByContentTypeConstants } from 'src/app/shared/constants/common.constant';

@Component({
  selector: 'app-reactive-customer',
  templateUrl: './reactive-customer.component.html',
  styleUrls: ['./reactive-customer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReactiveCustomerComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private validationService: ValidationService,
    private appSettingService: AppSettingsService
  ) { }

  countryCode = '65';
  phoneNumber = '';
  migrationOutletId = '10ecc00f-6e24-49bc-b735-c18127433f8b';
  migrationOutletName = 'Migration Outlet';
  @Input() validatePhones: Array<ValidatePhoneModel> = [];
  @Input() currentUser = new User();
  reactiveCustomerForm: FormGroup = new FormGroup({});
  @Input() isLoading = false;
  @Input() customerId = '';
  @Output() clickSaveEvent = new EventEmitter<ReactiveCustomerRequest>();

  ngOnInit() {
    this.reactiveCustomerForm = this.formBuilder.group({
      phoneCountryCode: [''],
      phoneNumber: ['', [Validators.required, blankSpaceValidator, Validators.pattern(GeneralValidation.NUMBER_REGEX)]]
    }, { validator: this.validationService.validatePhoneNumber('phoneCountryCode', 'phoneNumber', this.validatePhones) });
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onClickSaveBtn(e: any) {
    this.isLoading = true;
    const request: ReactiveCustomerRequest = {
      phoneCountryCode: this.countryCode,
      phoneNumber: this.phoneNumber,
      originalUrl: environment.memberPageUrl,
      userId: this.currentUser.id,
      outletId: this.migrationOutletId,
      outletName: this.migrationOutletName,
      customerId: this.customerId,
      memberOriginalUrl: environment.memberPageUrl,
      createdByName: this.currentUser ? `${this.currentUser.firstName} ${this.currentUser.lastName}` : '',
      acronymBrandName: this.appSettingService.getTitleFromAppSettingsData(
        this.appSettingService.appSettingsDataByContentType, AppSettingLabelByContentTypeConstants.AcronymBrandTitleValue),
    };
    this.clickSaveEvent.emit(request);
  }

}
