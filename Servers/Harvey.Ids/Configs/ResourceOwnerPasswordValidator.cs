using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.Activity;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Harvey.Ids.Configs
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;
        private readonly ILoggingActivityService _activityService;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;

        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEventService events, ILoggingActivityService activityService, IOptions<ConfigurationRabbitMq> configurationRabbitMq)
        {
            _userManager = userManager;
            _events = events;
            _signInManager = signInManager;
            _activityService = activityService;
            _configurationRabbitMq = configurationRabbitMq;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);

            if (context.Request.Client.ClientId == ClientsConfig.HarveyMemberPage && user.UserType == UserType.Member)
            {
                context.Result = await SignIn(ClientsConfig.HarveyMemberPage, user, context.Password, true);
                return;
            }
            else if (context.Request.Client.ClientId == ClientsConfig.HarveyAdministratorPage && (user.UserType == UserType.Admin || user.UserType == UserType.AdminStaff))
            {
                context.Result = await SignIn(ClientsConfig.HarveyAdministratorPage, user, context.Password, true);
                return;
            }
            else if (context.Request.Client.ClientId == ClientsConfig.HarveyStaffPage && (user.UserType == UserType.Staff || user.UserType == UserType.AdminStaff))
            {
                context.Result = await SignIn(ClientsConfig.HarveyStaffPage, user, context.Password, true);
                return;
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
            }

            context.Result = new GrantValidationResult(TokenErrors.InvalidGrant, "Wrong username or password");
        }

        private async Task<GrantValidationResult> SignIn(string clientPage, ApplicationUser user, string password, bool lockoutOnFailure)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            if (result.Succeeded)
            {
                var description = "Log In";
                var comment = "Log In";
                var userName = $"{user.FirstName} {user.LastName}";
                await LogAction(user.Id, _configurationRabbitMq.Value.RabbitMqUrl, description, comment, clientPage, userName);

                var sub = await _userManager.GetUserIdAsync(user);

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, sub, user.Id, interactive: false));

                return new GrantValidationResult(sub, AuthenticationMethods.Password);
            }
            else
            {
                return new GrantValidationResult(TokenErrors.InvalidGrant, "Wrong username or password");
            }
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string description, string comment, string clientPage, string userName)
        {
            var actionType = ActionType.LogInAdminApp;
            var actionArea = ActionArea.AdminApp;

            switch (clientPage)
            {
                case "Harvey-member-page":
                    actionType = ActionType.LogInMemberApp;
                    actionArea = ActionArea.MemberApp;
                    break;
                case "Harvey-staff-page":
                    actionType = ActionType.LogInStoreApp;
                    actionArea = ActionArea.StoreApp;
                    break;
                default:
                    break;
            }

            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = description;
            request.Comment = comment;
            request.ActionType = actionType;
            request.ActionAreaPath = actionArea;
            request.CreatedByName = userName;
            await _activityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
