using Harvey.Ids.Domains;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ReSendSignUpLinkCommandHandler
{
    public class ReSendSignUpLinkCommandHandler : IReSendSignUpLinkCommandHandler
    {
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReSendSignUpLinkCommandHandler(IBusControl bus, IConfiguration configuration, IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService, HarveyIdsDbContext harveyIdsDbContext, UserManager<ApplicationUser> userManager)
        {
            _bus = bus;
            _configuration = configuration;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(ReSendSignUpLinkCommandRequest command)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId && x.IsActive);
            if (!user.IsActive)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            string shortLinkId = StringExtension.RandomString();
            string signUpShortLink = $"{command.OriginalUrl}/su/{shortLinkId}";
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = command.UserId })
            });
            await _harveyIdsDbContext.SaveChangesAsync();

            string shortLink = await _generateShortLinkFromTinyUrlService.ExecuteAsync(signUpShortLink);
            string PIN = await GetPINAsync(command.UserId);

            ISendEndpoint reSendSignUpLinkEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "resend_sign_up_link")));
            await reSendSignUpLinkEndpointTask.Send<ReSendSignUpLinkCommand>(new
            {
                PhoneNumber = command.PhoneNumber,
                CountryCode = command.CountryCode,
                SignUpShortLink = shortLink,
                OutletName = command.OutletName,
                PIN = PIN
            });
        }

        private async Task<string> GetPINAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? user.Pin : null;
        }
    }
}
