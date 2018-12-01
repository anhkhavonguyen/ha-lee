using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.DeleteAppSettingCommandHandler
{
    public class DeleteAppSettingCommandHandler: IDeleteAppSettingCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private IOptions<ConfigurationRabbitMq> _config;
        private ILoggingActivityService _loggingActivityService;
        private const string DeleteAppSetting = "Delete App Setting";
        public DeleteAppSettingCommandHandler(HarveyCRMLoyaltyDbContext dbContext,
                                              IOptions<ConfigurationRabbitMq> config,
                                              ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task ExcuteAsync(DeleteAppSettingCommand command)
        {
            var appSetting = _dbContext.AppSettings.Where(x => x.Id == command.AppSettingId).FirstOrDefault();
            if (appSetting != null)
            {
                if (appSetting.GroupName == DeleteAppSettingAllowed.OptionRedeem || appSetting.GroupName == DeleteAppSettingAllowed.ValidatePhone)
                {
                    _dbContext.AppSettings.Remove(appSetting);
                    await _dbContext.SaveChangesAsync();
                    await LogAction(command.UserId, command.UserName, _config.Value.RabbitMqUrl, appSetting.Name);
                }
            }    
        }
        private async Task LogAction(string userId, string userName ,string rabbitMqUrl, string appSettingName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = DeleteAppSetting;
            request.Comment = appSettingName;
            request.ActionType = ActionType.DeleteAppSetting;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
