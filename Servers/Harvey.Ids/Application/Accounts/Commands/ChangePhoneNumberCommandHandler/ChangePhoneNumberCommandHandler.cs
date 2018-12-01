using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.Activity;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Customers;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePhoneNumberCommandHandler
{
    public class ChangePhoneNumberCommandHandler : IChangePhoneNumberCommandHandler
    {
        private readonly HarveyIdsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggingActivityService _activityService;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;
        private const string DuplicatePhoneNumber = "duplicate_phone_number";

        public ChangePhoneNumberCommandHandler(UserManager<ApplicationUser> userManager,
            ILoggingActivityService activityService,
            IOptions<ConfigurationRabbitMq> configurationRabbitMq,
            IBusControl bus,
            HarveyIdsDbContext dbContext,
            IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService)
        {
            _userManager = userManager;
            _activityService = activityService;
            _configurationRabbitMq = configurationRabbitMq;
            _bus = bus;
            _dbContext = dbContext;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
        }

        public async Task<string> ExecuteAsync(ChangePhoneNumberCommand command)
        {
            var customer = await _userManager.FindByIdAsync(command.CustomerId);
            if (customer == null)
                return null;

            if ((command.NewPhoneCountryCode != customer.PhoneCountryCode) || (command.NewPhoneNumber != customer.PhoneNumber))
            {
                var newMobilePhoneNumber = $"{command.NewPhoneCountryCode}{command.NewPhoneNumber}";
                if (await IsUserExist(newMobilePhoneNumber, customer.Id))
                    return DuplicatePhoneNumber;

                customer.PhoneCountryCode = command.NewPhoneCountryCode;
                customer.PhoneNumber = command.NewPhoneNumber;
                customer.UserName = newMobilePhoneNumber;
                customer.NormalizedUserName = newMobilePhoneNumber;
                customer.UpdatedDate = DateTime.UtcNow;

                await _userManager.UpdateAsync(customer);

                await NotifySmsToCustomer(command, customer);

                ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "change_phone_number_queue")));
                await sendEndpointTask.Send<ChangePhoneNumberCommandMessage>(new
                {
                    CustomerId = customer.Id,
                    NewPhoneCountryCode = command.NewPhoneCountryCode,
                    NewPhoneNumber = command.NewPhoneNumber,
                    UpdatedBy = command.UpdatedBy
                });

                var user = await _userManager.FindByIdAsync(command.UpdatedBy);
                var userName = user != null ? $"{user.FirstName} {user.LastName}" : "";
                await LogAction(command.UpdatedBy, _configurationRabbitMq.Value.RabbitMqUrl, newMobilePhoneNumber, command.CustomerCode, userName);
            }

            return customer.Id;
        }
        private async Task LogAction(string updateBy, string rabbitMqUrl, string newPhoneMobile, string customerCode, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = updateBy;
            request.Description = customerCode;
            request.Comment = newPhoneMobile;
            request.ActionType = ActionType.ChangeMobilePhoneNumber;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _activityService.ExecuteAsync(request, rabbitMqUrl);
        }

        private async Task<string> GetResetPasswordShortLink(string originalUrl, ApplicationUser customer)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(customer);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = customer.Id });
            var shortLinkId = StringExtension.RandomString();
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _dbContext.SaveChangesAsync();

            string shortUrl = $"{originalUrl}/auth/reset-pwd/{shortLinkId}";

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(shortUrl);
        }
        private async Task<string> GetUpdateProfileShortLink(string originalUrl, ApplicationUser customer)
        {
            string shortLinkId = StringExtension.RandomString();
            string signUpShortLink = $"{originalUrl}/su/{shortLinkId}";
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = customer.Id })
            });
            await _dbContext.SaveChangesAsync();

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(signUpShortLink);
        }
        private async Task<string> GetLoginShortLink(string originalUrl, ApplicationUser customer)
        {
            string shortLinkId = StringExtension.RandomString();
            string logInShortLink = $"{originalUrl}/auth/login/{shortLinkId}";
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = customer.Id })
            });
            await _dbContext.SaveChangesAsync();

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(logInShortLink);
        }
        private async Task<Boolean> IsUserExist(string newPhoneNumber, string customerId)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserName == newPhoneNumber && u.IsActive == true && u.Id != customerId);
        }

        private async Task NotifySmsToCustomer(ChangePhoneNumberCommand command, ApplicationUser customer)
        {
            if (!string.IsNullOrEmpty(command.NewPhoneCountryCode) || !string.IsNullOrEmpty(command.NewPhoneNumber))
            {
                var contentResetPasswordShortLink = string.Empty;
                var contentProfileShortLink = string.Empty;
                var contentLoginShortLink = string.Empty;

                if (customer.PasswordHash == null)
                {
                    var resetPasswordShortLink = await GetResetPasswordShortLink(command.MemberOriginalUrl, customer);
                    contentResetPasswordShortLink = string.Format("Please click {0} to reset your member account password.", resetPasswordShortLink);
                }
                else if (!customer.EmailConfirmed && !customer.PhoneNumberConfirmed)
                {
                    var updateProfileShortLink = await GetUpdateProfileShortLink(command.MemberOriginalUrl, customer);
                    contentProfileShortLink = string.Format("To earn loyalty points and rewards, click {0} to complete your profile.", updateProfileShortLink);
                }
                else
                {
                    var loginShortLink = await GetLoginShortLink(command.MemberOriginalUrl, customer);
                    contentLoginShortLink = string.Format("Please click {0} to login your member account.", loginShortLink);
                }

                ISendEndpoint sendSmsChangePhoneEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "send_sms_change_phone_number_queue")));
                await sendSmsChangePhoneEndpointTask.Send<SendSmsChangePhoneNumberCommand>(new
                {
                    CustomerId = customer.Id,
                    NewPhoneCountryCode = command.NewPhoneCountryCode,
                    NewPhoneNumber = command.NewPhoneNumber,
                    UpdatedBy = command.UpdatedBy,
                    ResetPasswordShortLink = contentResetPasswordShortLink,
                    UpdateProfileShortLink = contentProfileShortLink,
                    LoginShortLink = contentLoginShortLink,
                    AcronymBrandName = command.AcronymBrandName
                });
            }
        }
    }
}
