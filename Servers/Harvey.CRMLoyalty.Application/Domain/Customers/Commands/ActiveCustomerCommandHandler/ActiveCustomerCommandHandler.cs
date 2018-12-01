using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ActiveCustomerCommandHandler
{
    public class ActiveCustomerCommandHandler : IActiveCustomerCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private readonly IBusControl _bus;
        private IOptions<ConfigurationRabbitMq> _config;
        private readonly ILoggingActivityService _loggingActivityService;
        public ActiveCustomerCommandHandler(HarveyCRMLoyaltyDbContext dbContext, IBusControl bus, IOptions<ConfigurationRabbitMq> config, ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _bus = bus;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<bool> ExecuteAsync(ActiveCustomerCommand command)
        {
            var customers = _dbContext.Customers.Where(c => c.PhoneCountryCode == command.PhoneCountryCode && c.Phone == command.PhoneNumber);
            var customer = customers.FirstOrDefault(c => c.Id == command.CustomerId);
            var user = _dbContext.Staffs.Where(a => a.Id == command.UserId).FirstOrDefault() ;
            if (customer == null)
                throw new Extensions.EntityNotFoundException("Customer not found.");
            if ((Entities.Status)command.IsActive == Entities.Status.Active)
            {
                if (customers != null && customers.Count() > 1 && customers.Any(c => c.Status == Entities.Status.Active))
                    return false;

                customer.Status = Entities.Status.Active;
                command.FirstName = customer.FirstName;
                command.LastName = customer.LastName;
                command.Email = customer.Email;
                command.DateOfBirth = customer.DateOfBirth;
                command.Gender = customer.Gender;
            }
            else
            {
                customer.Status = Entities.Status.InActive;
            }
            customer.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            await SendMessageActiveCustomer(command, _config.Value.RabbitMqUrl);
            var customerPhone = command.PhoneCountryCode + command.PhoneNumber;
            var userName = command.CreatedByName;
            await LogAction(command.UserId, _config.Value.RabbitMqUrl, customerPhone, customer.CustomerCode, (Entities.Status)command.IsActive, userName);
            return true;
        }

        private async Task SendMessageActiveCustomer(ActiveCustomerCommand command, string RabbitMqUrl)
        {
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(RabbitMqUrl, "/", "active_customer_queue")));

            await sendEndpointTask.Send<ActiveCustomerCommandMessage>(new
            {
                PhoneCountryCode = command.PhoneCountryCode,
                PhoneNumber = command.PhoneNumber,
                CustomerId = command.CustomerId,
                IsActive = command.IsActive,
                command.FirstName,
                command.LastName,
                command.Email,
                command.DateOfBirth,
                command.Gender
            });
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string phoneNumber, string customerCode, Entities.Status IsActive, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = IsActive.HasFlag(Entities.Status.InActive) ? ActionType.DeActiveCustomer : ActionType.ActiveCustomer;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
