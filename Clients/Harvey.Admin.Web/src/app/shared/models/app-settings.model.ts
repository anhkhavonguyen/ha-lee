export class AppSettingModel {
    constructor(appSettingModel?: any) {
      if (!appSettingModel) { return; }
      this.id = appSettingModel.id;
      this.name = appSettingModel.name;
      this.value = appSettingModel.value;
      this.groupName = appSettingModel.groupName;
      this.appSettingTypeId = appSettingModel.appSettingTypeId;
      this.appSettingType = appSettingModel.appSettingType;
    }

    public id = 0;
    public name = '';
    public value = '';
    public groupName = '';
    public appSettingTypeId = 0;
    public appSettingType = '';

    static buildAppSetting(item: AppSettingModel): AppSettingModel {
      const appSetting = new AppSettingModel();
      appSetting.id = item.id;
      appSetting.name = item.name;
      appSetting.value = item.value;
      appSetting.groupName = item.groupName;
      appSetting.appSettingTypeId = item.appSettingTypeId;
      appSetting.appSettingType = item.appSettingType;
      return appSetting;
  }
}

export class AppSettingUpdateValue {
  userId = '';
  id = '';
  value = '';
  comment = '';
}

export enum AppSettingType {
  adminApp = 'AdminApp',
  storeApp = 'StoreApp',
  memberApp = 'MemberApp',
}

export class AppSettingDeleteRequest {
  userId = '';
  appSettingId = '';
  userName = '';
}

export class GetAppSettingsRequest {
  public pageNumber = 0;
  public pageSize = 0;
  public searchText = '';
}

export class GetAppSettingsReponse {
  public pageNumber = 0;
  public pageSize = 0;
  public totalItem = 0;
  public appSettingModels = [];
}
