using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings
{
    public interface IGetAppSettingsQuery
    {
        GetAppSettingsResponse GetAppSettings();
        GetAppSettingsResponse GetAppSettings(int type);
        GetAppSettingsResponse GetAppSettingsByListName(List<string> appSettingNames);
        GetAppSettingsResponse GetAppSettings(GetAppSettingsRequest request);
    }
}
