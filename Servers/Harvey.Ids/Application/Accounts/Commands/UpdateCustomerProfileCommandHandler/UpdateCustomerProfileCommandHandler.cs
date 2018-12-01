using System;
using System.Threading.Tasks;
using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Harvey.Ids.Services.Activity;
using Microsoft.Extensions.Options;
using Harvey.Ids.Configs;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateCustomerProfileCommandHandler
{
    public class UpdateCustomerProfileCommandHandler : IUpdateCustomerProfileCommandHandler
    {
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggingActivityService _activityService;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public UpdateCustomerProfileCommandHandler(UserManager<ApplicationUser> userManager,
            ILoggingActivityService activityService,
            HarveyIdsDbContext harveyIdsDbContext,
            IBus bus,
            IConfiguration configuration,
            IOptions<ConfigurationRabbitMq> configurationRabbitMq)
        {
            _userManager = userManager;
            _harveyIdsDbContext = harveyIdsDbContext;
            _bus = bus;
            _configuration = configuration;
            _activityService = activityService;
            _configurationRabbitMq = configurationRabbitMq;
        }

        public async Task ExecuteAsync(UpdateCustomerProfileCommand updateCustomerProfileCommand)
        {
            var customer = await _userManager.FindByIdAsync(updateCustomerProfileCommand.CustomerId);
            if (customer.EmailConfirmed || customer.PhoneNumberConfirmed)
            {
                throw new DataInvalidException("USER_HAS_BEEN_UPDATED");
            }
            if (!customer.IsActive)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            customer.FirstName = updateCustomerProfileCommand.FirstName;
            customer.LastName = updateCustomerProfileCommand.LastName;
            customer.Email = updateCustomerProfileCommand.Email;
            customer.EmailConfirmed = true;
            customer.PhoneNumberConfirmed = true;
            customer.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(customer, updateCustomerProfileCommand.Password);
            customer.DateOfBirth = updateCustomerProfileCommand.DateOfBirth;
            customer.Gender = updateCustomerProfileCommand.Gender;
            customer.UpdatedBy = updateCustomerProfileCommand.StaffId;
            customer.UpdatedDate = updateCustomerProfileCommand.UpdatedDate;
            customer.SecurityStamp = Guid.NewGuid().ToString("D");
            await _userManager.UpdateAsync(customer);

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "update_customer_profile_after_init_queue")));
            await sendEndpointTask.Send<UpdateCustomerProfileAfterInitCommandMessage>(new
            {
                CustomerId = customer.Id,
                PhoneCountryCode = customer.PhoneCountryCode,
                PhoneNumber = customer.PhoneNumber,
                UpdatedBy = updateCustomerProfileCommand.StaffId,
                DateOfBirth = customer.DateOfBirth,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Gender = customer.Gender
            });

            var phoneNumber = $"{customer.PhoneCountryCode}{customer.PhoneNumber}";

            var user = await _userManager.FindByIdAsync(updateCustomerProfileCommand.StaffId);
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : "";
            await LogAction(updateCustomerProfileCommand.StaffId, _configurationRabbitMq.Value.RabbitMqUrl, updateCustomerProfileCommand.CustomerCode, phoneNumber, userName);

        }

        private async Task LogAction(string staffId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = staffId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.UpdateCustomerProfile;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _activityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
