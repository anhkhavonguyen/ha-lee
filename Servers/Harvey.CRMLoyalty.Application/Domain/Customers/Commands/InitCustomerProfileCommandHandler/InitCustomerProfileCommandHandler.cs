using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerCodeCommandHandler;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.InitCustomerProfileCommandHandler
{
    public class InitCustomerProfileCommandHandler : IInitCustomerProfileCommandHandler
    {
        private readonly IBusControl _bus;
        private readonly HarveyCRMLoyaltyDbContext _harveyCRMLoyaltyDbContext;
        private readonly IUpdateCustomerCodeCommandHandler _updateCustomerCodeCommandHandler;
        private IOptions<ConfigurationRabbitMq> _config;
        private string InitCustomer = "InitCustomer";
        private readonly ILoggingActivityService _loggingActivityService;

        public InitCustomerProfileCommandHandler(HarveyCRMLoyaltyDbContext harveyCRMLoyaltyDbContext, IBusControl bus, IOptions<ConfigurationRabbitMq> config, IUpdateCustomerCodeCommandHandler updateCustomerCodeCommandHandler, ILoggingActivityService loggingActivityService)
        {
            _harveyCRMLoyaltyDbContext = harveyCRMLoyaltyDbContext;
            _updateCustomerCodeCommandHandler = updateCustomerCodeCommandHandler;
            _bus = bus;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }


        public async Task<string> ExecuteAsync(InitCustomerProfileCommand command)
        {
            var isExistCustomer = await _harveyCRMLoyaltyDbContext.Customers.AnyAsync(c => c.PhoneCountryCode == command.PhoneCountryCode 
                                                                                                        && c.Phone == command.PhoneNumber
                                                                                                        && c.Status == Status.Active);

            var user = _harveyCRMLoyaltyDbContext.Staffs.Where(a => a.Id == command.CreatedBy).FirstOrDefault();

            if (isExistCustomer)
                return null;

            var customer = new Customer();
            customer.Id = command.UserId;
            customer.PhoneCountryCode = command.PhoneCountryCode;
            customer.Phone = command.PhoneNumber;
            customer.JoinedDate = DateTime.UtcNow;
            customer.Status = Status.Active;
            customer.CreatedBy = command.CurrentUserId;
            customer.CreatedDate = DateTime.UtcNow;
            customer.FirstOutlet = _harveyCRMLoyaltyDbContext.Outlets.Find(command.OutletId)?.Name;

            _harveyCRMLoyaltyDbContext.Customers.Add(customer);
            await _harveyCRMLoyaltyDbContext.SaveChangesAsync();

            var updateCodeCommand = new UpdateCustomerCodeCommand();
            updateCodeCommand.CreatedDate = customer.CreatedDate;
            updateCodeCommand.CustomerId = customer.Id;
            updateCodeCommand.OutletId = command.OutletId;
            await _updateCustomerCodeCommandHandler.ExcuteAsync(updateCodeCommand);
            await SendMessageInitCustomerProfile(command, _config.Value.RabbitMqUrl);
            var customerPhone = command.PhoneCountryCode + command.PhoneNumber;
            var userName = command.CreatedBy == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
            await LogAction(command.CurrentUserId, _config.Value.RabbitMqUrl, customerPhone, userName, command.OutletId);
            return customer.Id;
        }

        public async Task SendMessageInitCustomerProfile(InitCustomerProfileCommand command, string RabbitMqUrl)
        {
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(RabbitMqUrl, "/", "init_member_account_profile")));

            await sendEndpointTask.Send<InitMemberProfileCommand>(new
            {
                UserId = command.UserId,
                PhoneNumber = command.PhoneNumber,
                PhoneCountryCode = command.PhoneCountryCode,
                CreatedBy = command.CreatedBy,
                OriginalUrl = command.OriginalUrl,
                OutletName = command.OutletName
            });
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string phoneNumber, string userName, string outletId)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = InitCustomer;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.InitCustomer;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            request.Value = outletId;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
