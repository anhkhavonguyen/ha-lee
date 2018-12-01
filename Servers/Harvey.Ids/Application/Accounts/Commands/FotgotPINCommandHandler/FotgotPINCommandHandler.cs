using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.FotgotPINCommandHandler
{
    internal class FotgotPINCommandHandler : IFotgotPINCommandHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;

        public FotgotPINCommandHandler(HarveyIdsDbContext harveyIdsDbContext,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IBusControl bus)
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
            _bus = bus;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(FotgotPINCommand fotgotPINCommand)
        {
            var user = await _userManager.FindByNameAsync(fotgotPINCommand.PhoneNumber);
            if (user == null)
                throw new EntityNotFoundException("User not found");

            var code = Newtonsoft.Json.JsonConvert.SerializeObject(new { ExpireDate = DateTime.UtcNow, UserId = user.Id });
            var shortLinkId = StringExtension.RandomString();
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = code
            });
            await _harveyIdsDbContext.SaveChangesAsync();

            string shortUrl = $"{fotgotPINCommand.OriginalUrl}/rspin/{shortLinkId}";

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_sms_forgot_pin_queue")));
            await sendEndpointTask.Send<SendForgotPINSMSMessage>(new
            {
                PhoneNumber = user.PhoneNumber,
                Title = "Forgot PIN",
                Content = shortUrl
            });
        }
    }
}
