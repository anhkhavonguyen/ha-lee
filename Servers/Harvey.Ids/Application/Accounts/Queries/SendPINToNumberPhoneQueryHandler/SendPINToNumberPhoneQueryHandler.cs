using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.SendPINToNumberPhoneQueryHandler
{
    internal class SendPINToNumberPhoneQueryHandler : ISendPINToNumberPhoneQueryHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBusControl _bus;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public SendPINToNumberPhoneQueryHandler(HarveyIdsDbContext harveyIdsDbContext,
            IBusControl bus, 
            UserManager<ApplicationUser> userManager,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _bus = bus;
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(string countryCode, string phoneNumber, string acronymBrandName, string outletName, string userId = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!user.IsActive)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            if (user.UserType == UserType.Member)
            {
                ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_pin_to_phone_queue")));
                await sendEndpointTask.Send<SendPINToNumberPhoneMessage>(new
                {
                    PhoneNumber = user.PhoneNumber,
                    PhoneCountryCode = user.PhoneCountryCode,
                    Pin = user.Pin,
                    AcronymBrandName = acronymBrandName,
                });
            }
            else
            {
                var customer = _harveyIdsDbContext.Users.FirstOrDefault(f => f.PhoneCountryCode == countryCode && f.PhoneNumber == phoneNumber);
                if(customer == null)
                {
                    throw new EntityNotFoundException("Entity Not Found");
                }
                ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_pin_to_phone_queue")));
                await sendEndpointTask.Send<SendPINToNumberPhoneMessage>(new
                {
                    PhoneNumber = customer.PhoneNumber,
                    PhoneCountryCode = customer.PhoneCountryCode,
                    Pin = customer.Pin,
                    AcronymBrandName = acronymBrandName,
                    OutletName = outletName
                });
            }
        }
    }
}
