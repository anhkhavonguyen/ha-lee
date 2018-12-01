import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { AppSettingModel, GetAppSettingsRequest } from '../../shared/models/app-settings.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UpdateSettingValueComponent } from './update-setting-value/update-setting-value.component';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DeleteSettingValueComponent } from './delete-setting-value/delete-setting-value.component';
import { AddSettingValueComponent } from './add-setting-value/add-setting-value.component';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SettingsComponent implements OnInit {

  listAppSettings: Array<AppSettingModel> = [];
  AppSettingsListTest: Array<AppSettingModel> = [];
  pageNumber: number;
  pageSize: number;
  totalItem: number;
  searchText = '';
  loadingIndicator = true;
  private appSettingMemberApp = 3;
  private appSettings: any;
  @ViewChild('searchInput')
  searchInput!: ElementRef;

  constructor(private appSettingService: AppSettingsService,
    private modalService: NgbModal,
    private translate: TranslateService,
    private toast: ToastrService) {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
  }

  ngOnInit() {
    this.onSearch();
    this.addKeyUpEventToSearchText();
  }


  setPage(pageInfo: { offset: number; }) {
    const request: GetAppSettingsRequest = {
      pageNumber: pageInfo.offset,
      pageSize: 10,
      searchText: this.searchText
    };

    this.appSettingService.getAppSettingsData(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;
      this.listAppSettings = temp.appSettingModels.map(result => {
        const appSettingModel = AppSettingModel.buildAppSetting(result);
        this.loadingIndicator = false;
        return appSettingModel;
      });
    });
  }

  onSearch() {
    this.setPage({ offset: 0 });
  }
  addKeyUpEventToSearchText() {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .subscribe(() => {
        this.onSearch();
      });
  }

  checkIsAppSettingIcon(appSettingValue: AppSettingModel) {
    const groupNameIcon = 'Icon';
    return appSettingValue.groupName === groupNameIcon ? true : false;
  }

  checkIsAppSettingHtmlContent(appSettingValue: AppSettingModel) {
    const groupNameHmlContent = 'HtmlContent';
    return appSettingValue.groupName === groupNameHmlContent ? true : false;
  }

  checkIsAppSettingMemberContactInfo(appSettingValue: AppSettingModel) {
    const groupNameHmlMemberContactInfo = 'MemberContactInfo';
    return appSettingValue.groupName === groupNameHmlMemberContactInfo ? true : false;
  }

  checkIsAppSettingButtonColor(appSettingValue: AppSettingModel) {
    const groupNameColor = 'Color';
    return appSettingValue.groupName.includes(groupNameColor) ? true : false;
  }

  getTitle(appSettingValue: AppSettingModel) {
    const value = JSON.parse(appSettingValue.value);
    return value.title;
  }

  getButtonColor(appSettingValue: AppSettingModel) {
    const appSettingButtonColorValue = JSON.parse(appSettingValue.value);
    const value = {
      backgroundColor: appSettingButtonColorValue['background-color'],
      color: appSettingButtonColorValue['color'],
    };
    return value;
  }

  checkIsAppSettingBooleanValue(appSettingValue: AppSettingModel) {
    return appSettingValue.value === 'true' || appSettingValue.value === 'false' ? true : false;
  }

  showBooleanValue(appSettingValue: AppSettingModel) {
    return appSettingValue.value === 'true' ? true : false;
  }

  checkIsAppSettingStringValue(appSettingValue: AppSettingModel) {
    const isIcon = this.checkIsAppSettingIcon(appSettingValue);
    const isBoolean = this.checkIsAppSettingBooleanValue(appSettingValue);
    const isHtmlContent = this.checkIsAppSettingHtmlContent(appSettingValue);
    const isMemberContactInfo = this.checkIsAppSettingMemberContactInfo(appSettingValue);
    return !isMemberContactInfo && !isHtmlContent && !isIcon && !isBoolean ? true : false;
  }

  onClickEditValue(appSettingValue: AppSettingModel) {
    if (appSettingValue) {
      const isAppSettingIcon = this.checkIsAppSettingIcon(appSettingValue);
      const isAppSettingBooleanValue = this.checkIsAppSettingBooleanValue(appSettingValue);
      const isAppSettingHtmlContent = this.checkIsAppSettingHtmlContent(appSettingValue);
      const isAppSettingMemberContactInfo = this.checkIsAppSettingMemberContactInfo(appSettingValue);
      const isAppSettingColor = this.checkIsAppSettingButtonColor(appSettingValue);
      const dialogRef = this.modalService.open(UpdateSettingValueComponent, { size: 'lg', centered: true, backdrop: 'static' });
      const instance = dialogRef.componentInstance;
      instance.appSettingValue = appSettingValue;
      instance.isAppSettingIcon = isAppSettingIcon;
      instance.isAppSettingBooleanValue = isAppSettingBooleanValue;
      instance.isAppSettingHtmlContent = isAppSettingHtmlContent;
      instance.isAppSettingMemberContactInfo = isAppSettingMemberContactInfo;
      instance.isAppSettingColor = isAppSettingColor;
      instance.listAppSettings = this.listAppSettings;
      return dialogRef.result.then((result) => {
        this.setPage({ offset: this.pageNumber });
      }, (reason) => {
      });
    }
  }

  onClickDeleteValue(appSettingValue: AppSettingModel) {
    if (appSettingValue) {
      const isAppSettingIcon = this.checkIsAppSettingIcon(appSettingValue);
      const isAppSettingBooleanValue = this.checkIsAppSettingBooleanValue(appSettingValue);
      const dialogRef = this.modalService.open(DeleteSettingValueComponent, { size: 'lg', centered: true, backdrop: 'static' });
      const instance = dialogRef.componentInstance;
      instance.appSettingValue = appSettingValue;
      instance.isAppSettingIcon = isAppSettingIcon;
      instance.isAppSettingBooleanValue = isAppSettingBooleanValue;
      return dialogRef.result.then((result) => {
        this.setPage({ offset: this.pageNumber });
      }, (reason) => {
      });
    }
  }
  onClickAddNewAppSetting() {
    const dialogRef = this.modalService.open(AddSettingValueComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    return dialogRef.result.then((result) => {
      this.setPage({ offset: this.pageNumber });
    }, (reason) => {
    });
  }

  isShowDelete(appSetting: AppSettingModel) {
    if (appSetting.groupName === 'OptionRedeem' || appSetting.groupName === 'ValidatePhone') {
      return true;
    }
    return false;
  }
}
