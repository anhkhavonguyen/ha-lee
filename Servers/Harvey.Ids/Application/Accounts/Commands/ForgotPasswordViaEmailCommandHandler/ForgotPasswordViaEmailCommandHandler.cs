using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaEmailCommandHandler
{
    internal class ForgotPasswordViaEmailCommandHandler : IForgotPasswordViaEmailCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;


        public ForgotPasswordViaEmailCommandHandler(UserManager<ApplicationUser> userManager,
            IBusControl bus,
            IConfiguration configuration,
            HarveyIdsDbContext harveyIdsDbContext)
        {
            _userManager = userManager;
            _bus = bus;
            _configuration = configuration;
            _harveyIdsDbContext = harveyIdsDbContext;
        }

        public async Task ExecuteAsync(ForgotPasswordViaEmailCommand forgotPasswordCommand)
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

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_email_forgot_password_queue")));
            await sendEndpointTask.Send<SendForgotPasswordEmailMessage>(new
            {
                Email = user.Email,
                Title = "Forgot password",
                ShortLink = shortUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BrandName = forgotPasswordCommand.BrandName,
                AcronymBrandName = forgotPasswordCommand.AcronymBrandName
            });
        }
    }
}
