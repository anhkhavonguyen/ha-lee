using Harvey.Ids.Domains;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaSMSCommandHandler
{
    internal class ForgotPasswordViaSMSCommandHandler : IForgotPasswordViaSMSCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;


        public ForgotPasswordViaSMSCommandHandler(UserManager<ApplicationUser> userManager,
            IBusControl bus,
            IConfiguration configuration,
            HarveyIdsDbContext harveyIdsDbContext,
            IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService)
        {
            _userManager = userManager;
            _bus = bus;
            _configuration = configuration;
            _harveyIdsDbContext = harveyIdsDbContext;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
        }

        public async Task ExecuteAsync(ForgotPasswordViaSMSCommand forgotPasswordCommand)
        {
            var user = forgotPasswordCommand.User;
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = user.Id });
            var shortLinkId = StringExtension.RandomString();
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _harveyIdsDbContext.SaveChangesAsync();

            string shortUrl = $"{forgotPasswordCommand.OriginalUrl}/auth/reset-pwd/{shortLinkId}";

            string shortLink = await _generateShortLinkFromTinyUrlService.ExecuteAsync(shortUrl);

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_sms_forgot_password_queue")));
            await sendEndpointTask.Send<SendForgotPasswordSMSMessage>(new
            {
                PhoneNumber = user.PhoneNumber,
                PhoneCountryCode = user.PhoneCountryCode,
                Title = "Forgot password",
                Link = shortLink,
                AcronymBrandName = forgotPasswordCommand.AcronymBrandName,
                OutletName = forgotPasswordCommand.OutletName
            });
        }
    }
}
