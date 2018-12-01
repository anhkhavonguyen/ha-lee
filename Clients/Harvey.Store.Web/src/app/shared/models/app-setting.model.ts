export class AppSettingModel {
  public id: string;
  public name: string;
  public value: string;
  public groupName: string;
  public appSettingTypeId: number;
}

export enum AppSettingType {
  adminApp = 'AdminApp',
  storeApp = 'StoreApp',
  memberApp = 'MemberApp',
}

export class GetAppsettingResponse {
  public appSettingModels: Array<AppSettingModel> = [];
}
