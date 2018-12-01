using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateFullCustomerInfomationCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.Customers
{
    public class UpdateFullCustomerInfomationConsumer : IConsumer<UpdateFullCustomerInfomationCommandMessage>
    {
        private readonly IUpdateFullCustomerInfomationCommandHandler _updateFullCustomerInfomationCommandHandler;
        public UpdateFullCustomerInfomationConsumer(IUpdateFullCustomerInfomationCommandHandler changePhoneNumberCommandHandler)
        {
            _updateFullCustomerInfomationCommandHandler = changePhoneNumberCommandHandler;
        }

        public async Task Consume(ConsumeContext<UpdateFullCustomerInfomationCommandMessage> context)
        {
            var message = context.Message;
            await _updateFullCustomerInfomationCommandHandler.ExecuteAsync(new UpdateFullCustomerInfomationCommand
            {
                CustomerId = message.CustomerId,
                NewPhoneCountryCode = message.NewPhoneCountryCode,
                NewPhoneNumber = message.NewPhoneNumber,
                UpdatedBy = message.UpdatedBy,
                DateOfBirth = message.DateOfBirth,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                Gender = message.Gender,
                PostalCode = message.PostalCode,
                AcronymBrandName = message.AcronymBrandName
            });
        }
    }
}
