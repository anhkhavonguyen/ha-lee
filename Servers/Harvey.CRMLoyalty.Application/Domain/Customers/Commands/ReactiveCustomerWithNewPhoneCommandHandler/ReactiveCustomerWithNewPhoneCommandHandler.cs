using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ReactiveCustomerWithNewPhoneCommandHandler
{
    public class ReactiveCustomerWithNewPhoneCommandHandler : IReactiveCustomerWithNewPhoneCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private readonly IBusControl _bus;
        private IOptions<ConfigurationRabbitMq> _config;
        private readonly ILoggingActivityService _loggingActivityService;

        public ReactiveCustomerWithNewPhoneCommandHandler(HarveyCRMLoyaltyDbContext dbContext, IBusControl bus, IOptions<ConfigurationRabbitMq> config, ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _bus = bus;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<bool> ExecuteAsync(ReactiveCustomerWithNewPhoneCommand command)
        {
            var oldData = "Old phone number: ";
            var updatedData = "New Phone Number: ";
            var customers = _dbContext.Customers.Where(c => c.PhoneCountryCode == command.PhoneCountryCode && c.Phone == command.PhoneNumber);

            var checkDuplicatePhone = customers.Any(x => x.Status == Entities.Status.Active);
            if (checkDuplicatePhone)
                return false;

            var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == command.CustomerId);
            if (customer == null)
                throw new Extensions.EntityNotFoundException("Customer not found.");

            var user = _dbContext.Staffs.Where(a => a.Id == command.UserId).FirstOrDefault();
            var trackData = $"{oldData}{customer.PhoneCountryCode}{customer.Phone} {updatedData}{command.PhoneCountryCode}{command.PhoneNumber}";
            customer.PhoneCountryCode = command.PhoneCountryCode;
            customer.Phone = command.PhoneNumber;
            customer.Status = Entities.Status.Active;
            await _dbContext.SaveChangesAsync();

            command.FirstName = customer.FirstName;
            command.LastName = customer.LastName;
            command.DateOfBirth = customer.DateOfBirth;
            command.Gender = customer.Gender;
            command.Email = customer.Email;
            await SendMessageActiveCustomer(command, _config.Value.RabbitMqUrl);
            var userName = command.CreatedByName;
            await LogAction(command.UserId, _config.Value.RabbitMqUrl, trackData, customer.CustomerCode, (Entities.Status)command.IsActive, userName);
            return true;
        }

        private async Task SendMessageActiveCustomer(ReactiveCustomerWithNewPhoneCommand command, string RabbitMqUrl)
        {
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(RabbitMqUrl, "/", "reactive_customer_queue")));

            await sendEndpointTask.Send<ReactiveCustomerWithNewPhoneMessageCommand>(new
            {
                command.PhoneCountryCode,
                command.PhoneNumber,
                command.CustomerId,
                command.IsActive,
                command.FirstName,
                command.LastName,
                command.Email,
                command.DateOfBirth,
                command.Gender,
                command.MemberOriginalUrl,
                command.AcronymBrandName
            });
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string phoneNumber, string trackData, Entities.Status IsActive, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = trackData;
            request.Comment = phoneNumber;
            request.ActionType = IsActive.HasFlag(Entities.Status.InActive) ? ActionType.DeActiveCustomer : ActionType.ActiveCustomer;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
