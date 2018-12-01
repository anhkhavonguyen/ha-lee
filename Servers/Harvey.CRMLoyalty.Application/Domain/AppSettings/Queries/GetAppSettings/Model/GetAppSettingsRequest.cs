using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings.Model
{
    public class GetAppSettingsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
