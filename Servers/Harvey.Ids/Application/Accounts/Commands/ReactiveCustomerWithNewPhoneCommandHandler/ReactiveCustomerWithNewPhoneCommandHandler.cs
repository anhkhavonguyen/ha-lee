using Harvey.Ids.Configs;
using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ReactiveCustomerWithNewPhoneCommandHandler
{
    public class ReactiveCustomerWithNewPhoneCommandHandler : IReactiveCustomerWithNewPhoneCommandHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;
        public ReactiveCustomerWithNewPhoneCommandHandler(
            HarveyIdsDbContext harveyIdsDbContext,
            UserManager<ApplicationUser> userManager,
            IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService,
            IOptions<ConfigurationRabbitMq> configurationRabbitMq,
            IBusControl bus
            )
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
            _configurationRabbitMq = configurationRabbitMq;
            _bus = bus;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
        }

        public async Task ExecuteAsync(ReactiveCustomerWithNewPhoneCommand command)
        {
            var checkExistsUser = _userManager.Users.Any(x => x.Id == command.CustomerId || (x.PhoneCountryCode == command.PhoneCountryCode && x.PhoneNumber == command.PhoneNumber));
            if (!checkExistsUser)
            {
                var applicationUser = new ApplicationUser
                {
                    Id = command.CustomerId,
                    UserName = $"{command.PhoneCountryCode}{command.PhoneNumber}",
                    PhoneNumber = command.PhoneNumber,
                    CreatedDate = DateTime.UtcNow,
                    IsMigrateData = false,
                    PhoneCountryCode = command.PhoneCountryCode,
                    Pin = StringExtension.GeneratePIN(),
                    UserType = Data.UserType.Member,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    DateOfBirth = command.DateOfBirth,
                    IsActive = true,
                    EmailConfirmed = string.IsNullOrEmpty(command.Email) ? false : true,
                    PhoneNumberConfirmed = string.IsNullOrEmpty(command.Email) ? false : true
                };

                if (command.Gender.HasValue)
                    applicationUser.Gender = (Gender)command.Gender;
                else
                    applicationUser.Gender = null;

                IdentityResult result = await _userManager.CreateAsync(applicationUser);

                if(result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Member" });
                    await SendNotifySmsToCustomer(command);
                }
            }           
        }

        private async Task<string> GetResetPasswordShortLink(string originalUrl, ApplicationUser customer)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(customer);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = customer.Id });
            var shortLinkId = StringExtension.RandomString();
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _harveyIdsDbContext.SaveChangesAsync();

            string shortUrl = $"{originalUrl}/auth/reset-pwd/{shortLinkId}";

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(shortUrl);
        }

        private async Task SendNotifySmsToCustomer(ReactiveCustomerWithNewPhoneCommand command)
        {
            var customer = await _userManager.FindByIdAsync(command.CustomerId);
            if (customer != null)
            {
                var contentResetPasswordShortLink = string.Empty;
                var contentProfileShortLink = string.Empty;
                var contentLoginShortLink = string.Empty;
                var resetPasswordShortLink = await GetResetPasswordShortLink(command.MemberOriginalUrl, customer);
                contentResetPasswordShortLink = string.Format("Please click {0} to reset your member account password.", resetPasswordShortLink);

                ISendEndpoint sendSmsChangePhoneEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "send_sms_change_phone_number_queue")));
                await sendSmsChangePhoneEndpointTask.Send<SendSmsChangePhoneNumberCommand>(new
                {
                    CustomerId = customer.Id,
                    NewPhoneCountryCode = command.PhoneCountryCode,
                    NewPhoneNumber = command.PhoneNumber,
                    UpdatedBy = command.UserId,
                    ResetPasswordShortLink = contentResetPasswordShortLink,
                    UpdateProfileShortLink = contentProfileShortLink,
                    LoginShortLink = contentLoginShortLink,
                    AcronymBrandName = command.AcronymBrandName
                });
            }
            
        }
    }
}
