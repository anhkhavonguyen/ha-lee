using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings.Model
{
    public class GetAppSettingsResponse : BaseResponse
    {
        public List<AppSettingModel> AppSettingModels { get; set; }
    }
}
