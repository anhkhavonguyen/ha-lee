using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.UpdateAppSettingCommandHandler
{
    public class UpdateAppSettingCommandHandler : IUpdateAppSettingCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private IOptions<ConfigurationRabbitMq> _config;
        private ILoggingActivityService _loggingActivityService;
        private const string UpdateAppSetting = "Update App Setting";
        public UpdateAppSettingCommandHandler(HarveyCRMLoyaltyDbContext dbContext, IOptions<ConfigurationRabbitMq> config, ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }
        public async Task<string> ExecuteAsync(UpdateAppSettingCommand command)
        {
            var appSetting = await _dbContext.AppSettings.FirstOrDefaultAsync(a => a.Id == command.Id);
            var user = _dbContext.Staffs.Where(x => x.Id == command.UserId).FirstOrDefault();
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : (command.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : "");

            appSetting.Value = command.Value;
            appSetting.UpdatedBy = command.UserId;
            appSetting.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            await LogAction(command.UserId, userName, command.Comment, _config.Value.RabbitMqUrl);

            return appSetting.Id;
        }

        private async Task LogAction(string userId, string userName, string comment, string rabbitMqUrl)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = UpdateAppSetting;
            request.Comment = comment;
            request.ActionType = ActionType.UpdateAppSetting;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
