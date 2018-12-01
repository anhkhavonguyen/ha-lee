using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.AddAppSettingsCommandHandler
{
    class AddAppSettingsCommandHandler : IAddAppSettingsCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private IOptions<ConfigurationRabbitMq> _config;
        private ILoggingActivityService _loggingActivityService;

        public AddAppSettingsCommandHandler(HarveyCRMLoyaltyDbContext dbContext,
                                            IOptions<ConfigurationRabbitMq> config,
                                            ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<string> ExecuteAsync(AddAppSettingsCommand addAppSettingsCommand)
        {
            var checkExistAppSettings = _dbContext.AppSettings.Any(x => x.Name.ToLower().Trim() == addAppSettingsCommand.Name.ToLower().Trim());
            var latestAppSettingId = int.Parse(_dbContext.AppSettings.OrderByDescending(x => int.Parse(x.Id)).First().Id) + 1;
            var AppSettingTypeId = _dbContext.AppSettingTypes.FirstOrDefault(x => x.TypeName == addAppSettingsCommand.AppSettingTypeName).Id;
            var user = _dbContext.Staffs.Where(x => x.Id == addAppSettingsCommand.UserId).FirstOrDefault();
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : (addAppSettingsCommand.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : "");

            if (!checkExistAppSettings)
            {
                var newAppSetting = new Entities.AppSetting();

                newAppSetting.Id = latestAppSettingId.ToString();
                newAppSetting.CreatedDate = DateTime.UtcNow;
                newAppSetting.UpdatedDate = DateTime.UtcNow;
                newAppSetting.CreatedBy = addAppSettingsCommand.UserId;
                newAppSetting.UpdatedBy = addAppSettingsCommand.UserId;
                newAppSetting.Name = addAppSettingsCommand.Name;
                newAppSetting.Value = addAppSettingsCommand.Value;
                newAppSetting.GroupName = addAppSettingsCommand.GroupName;
                newAppSetting.AppSettingTypeId = AppSettingTypeId;
                
                _dbContext.AppSettings.Add(newAppSetting);
                await LogAction(addAppSettingsCommand.UserId, userName, addAppSettingsCommand.GroupName, addAppSettingsCommand.Name, addAppSettingsCommand.Value, _config.Value.RabbitMqUrl);
                await _dbContext.SaveChangesAsync();
                return newAppSetting.Id;
            }
            return null;
        }
        private async Task LogAction(string userId, string userName, string appSettingGroupName,string appSettingName,string appSettingValue, string rabbitMqUrl)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = appSettingGroupName;
            request.Comment = $"{appSettingName}-{appSettingValue}";
            request.ActionType = ActionType.AddAppSetting;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
