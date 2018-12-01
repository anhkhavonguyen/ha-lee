using Harvey.CRMLoyalty.Api.Utils;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.AddAppSettingsCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.DeleteAppSettingCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.UpdateAppSettingCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings.Model;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler.Model;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AppSettings")]
    public class AppSettingsController : Controller
    {
        private readonly IGetAppSettingsQuery _getAppSettingsQuery;
        private readonly IUpdateAppSettingCommandHandler _updateAppSettingCommandHandler;
        private readonly IDeleteAppSettingCommandHandler _deleteAppSettingCommandHandler;
        private readonly IAddAppSettingsCommandHandler _addAppSettingCommandHandler;
        private readonly IExpiryRewardPointNotificationCommand _expiryRewardPointNotificationCommand;
        private readonly IExpiryMembershipNotificationCommand _expiryMembershipNotificationCommand;

        public AppSettingsController(IGetAppSettingsQuery getAppSettingsQuery,
                                     IUpdateAppSettingCommandHandler updateAppSettingCommandHandler,
                                     IDeleteAppSettingCommandHandler deleteAppSettingCommandHandler,
                                     IAddAppSettingsCommandHandler addAppSettingCommandHandler,
                                     IExpiryRewardPointNotificationCommand expiryRewardPointNotificationCommand,
                                     IExpiryMembershipNotificationCommand expiryMembershipNotificationCommand)
        {
            _getAppSettingsQuery = getAppSettingsQuery;
            _updateAppSettingCommandHandler = updateAppSettingCommandHandler;
            _deleteAppSettingCommandHandler = deleteAppSettingCommandHandler;
            _addAppSettingCommandHandler = addAppSettingCommandHandler;
            _expiryRewardPointNotificationCommand = expiryRewardPointNotificationCommand;
            _expiryMembershipNotificationCommand = expiryMembershipNotificationCommand;
        }


        [HttpGet("gets")]
        [AllowAnonymous]
        public IActionResult GetAppSettings()
        {
            var result = _getAppSettingsQuery.GetAppSettings();
            return Ok(result);
        }

        [HttpGet("getsData")]
        [AllowAnonymous]
        public IActionResult GetAppSettings(GetAppSettingsRequest request)
        {
            var result = _getAppSettingsQuery.GetAppSettings(request);
            return Ok(result);
        }

        [HttpGet("getsbytype")]
        [AllowAnonymous]
        public IActionResult GetAppSettings(int type)
        {
            var result = _getAppSettingsQuery.GetAppSettings(type);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> AddAppSetting([FromBody] AddAppSettingsCommand command)
        {
            var result = await _addAppSettingCommandHandler.ExecuteAsync(command);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult>  UpdateAppSetting([FromBody] UpdateAppSettingCommand command)
        {
            var result = await _updateAppSettingCommandHandler.ExecuteAsync(command);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> DeleteAppSetting(DeleteAppSettingCommand command)
        {
            await _deleteAppSettingCommandHandler.ExcuteAsync(command);
            return Ok();
        }

        [HttpPost("testSMSrepiryPoint")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> SendMessenger([FromQuery] string date)
        {
            var dateString = date.Split("-");
            var datetime = new System.DateTime(int.Parse(dateString[0]), int.Parse(dateString[1]), int.Parse(dateString[2]));

            var command = new ExpiryRewardPointNotificationModel();
            command.Date = datetime;

            var commandtest = new ExpiryMembershipNotificationModel();
            commandtest.Date = datetime;

            await _expiryRewardPointNotificationCommand.ExecuteAsync(command);
            await _expiryMembershipNotificationCommand.ExecuteAsync(commandtest);
            return Ok();
        }
    }
}