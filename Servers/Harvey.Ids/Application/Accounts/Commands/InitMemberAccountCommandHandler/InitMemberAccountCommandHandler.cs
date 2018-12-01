using Harvey.Ids.Domains;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler
{
    internal class InitMemberAccountCommandHandler : IInitMemberAccountCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly IBusControl _bus;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;
        public InitMemberAccountCommandHandler(UserManager<ApplicationUser> userManager, 
            HarveyIdsDbContext harveyIdsDbContext,
            IBusControl bus,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService)
        {
            _userManager = userManager;
            _harveyIdsDbContext = harveyIdsDbContext;
            _bus = bus;
            _configuration = configuration;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
        }

        public async Task ExecuteAsync(InitMemberAccountCommand initMemberAccountCommand)
        {
            string phonenumber = $"{initMemberAccountCommand.PhoneCountryCode}{initMemberAccountCommand.PhoneNumber}";
            var applicationUser = new ApplicationUser
            {
                Id = initMemberAccountCommand.UserId,
                UserName = phonenumber,
                PhoneNumber = initMemberAccountCommand.PhoneNumber,
                CreatedDate = DateTime.UtcNow,
                IsMigrateData = false,
                PhoneCountryCode = initMemberAccountCommand.PhoneCountryCode,
                Pin = StringExtension.GeneratePIN(),
                UserType = Data.UserType.Member,
                IsActive = true
            };
            IdentityResult result = await _userManager.CreateAsync(applicationUser);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Member" });

                string shortLinkId = StringExtension.RandomString();
                string signUpShortLink = $"{initMemberAccountCommand.OriginalUrl}/su/{shortLinkId}";
                _harveyIdsDbContext.ShortLinks.Add(new ShortLink
                {
                    Id = shortLinkId,
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = applicationUser.Id })
                });
                await _harveyIdsDbContext.SaveChangesAsync();

                string shortLink = await _generateShortLinkFromTinyUrlService.ExecuteAsync(signUpShortLink);

                ISendEndpoint sendEndpointTaskInitAccountCompleted = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "init_member_account_completed_queue")));
                await sendEndpointTaskInitAccountCompleted.Send<InitMemberAccountCompletedMessage>(new
                {
                    PhoneNumber = applicationUser.PhoneNumber,
                    SignUpShortLink = shortLink,
                    PhoneCountryCode = applicationUser.PhoneCountryCode,
                    Pin = applicationUser.Pin,
                    CreatedBy = initMemberAccountCommand.CurrentUserId,
                    OutletName = initMemberAccountCommand.OutletName
                });
            }
            else
            {
                throw new Exception(string.Join(";", result.Errors.Select(s => s.Description)));
            }
        }
    }
}
