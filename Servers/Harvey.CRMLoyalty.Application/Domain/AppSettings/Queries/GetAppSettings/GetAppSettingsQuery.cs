using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings
{
    public class GetAppSettingsQuery : IGetAppSettingsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetAppSettingsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetAppSettingsResponse GetAppSettings()
        {
            var response = new GetAppSettingsResponse();

            var query = _dbContext.AppSettings.AsNoTracking()
                .Include(x => x.AppSettingType)
                .GroupBy(x => x.GroupName)
                .Select(group => new
                {
                    Id = group.Key,
                    AppSettings = group.AsQueryable(),
                }).SelectMany(x => x.AppSettings).ToList()
                .OrderBy(x => x.GroupName).ThenBy(x => x.Id);

            var result = query.Select(a => new AppSettingModel
            {
                Id = a.Id,
                GroupName = a.GroupName,
                Name = a.Name,
                Value = a.Value,
                AppSettingTypeId = a.AppSettingTypeId,
                AppSettingType = a.AppSettingType?.TypeName
            });

            response.AppSettingModels = result.ToList();

            return response;
        }

        public GetAppSettingsResponse GetAppSettings(GetAppSettingsRequest request)
        {

            var query = _dbContext.AppSettings.AsNoTracking()
                .Include(x => x.AppSettingType)
                .OrderBy(x => x.AppSettingType)
                .ThenBy(x => x.GroupName)
                .ThenBy(x => x.Id)
                .Select(x => new AppSettingModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AppSettingType = x.AppSettingType.TypeName,
                    AppSettingTypeId = x.AppSettingTypeId,
                    GroupName = x.GroupName,
                    Value = x.Value
                });

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var searchString = request.SearchText.ToLower().Trim();
                    query = query.Where(x => (x.Name != null && x.Name.ToLower().Trim().Contains(searchString))
                                          || (x.AppSettingType != null && x.AppSettingType.ToLower().Trim().Contains(searchString))
                                          || (x.GroupName != null && x.GroupName.ToLower().Trim().Contains(searchString)));
            }

            var result = PagingExtensions.GetPaged<AppSettingModel>(query, request.PageNumber, request.PageSize);
            var response = new GetAppSettingsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.AppSettingModels = result.Results;
            return response;
        }

        public GetAppSettingsResponse GetAppSettings(int type)
        {
            var response = new GetAppSettingsResponse();

            var result = _dbContext.AppSettings.AsNoTracking().Select(a => new AppSettingModel
            {
                Id = a.Id,
                GroupName = a.GroupName,
                Name = a.Name,
                Value = a.Value,
                AppSettingTypeId = a.AppSettingTypeId
            }).Where(a=>a.AppSettingTypeId == type);

            response.AppSettingModels = result.ToList();

            return response;
        }

        public GetAppSettingsResponse GetAppSettingsByListName(List<string> appSettingNames)
        {
            var response = new GetAppSettingsResponse();

            if(appSettingNames == null && !appSettingNames.Any())
            {
                response.AppSettingModels = new List<AppSettingModel>();
                return response;
            }

            var query = _dbContext.AppSettings.AsNoTracking().Where(x => appSettingNames.Contains(x.Name)).Select(a => new AppSettingModel
            {
                Id = a.Id,
                GroupName = a.GroupName,
                Name = a.Name,
                Value = a.Value,
                AppSettingTypeId = a.AppSettingTypeId
            });

            response.AppSettingModels = query.ToList();

            return response;
        }
    }
}
