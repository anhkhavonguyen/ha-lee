using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerProfileAfterInitCommandHandler;
using MassTransit;
using System.Threading.Tasks;
using Harvey.Message.Customers;

namespace Harvey.CRMLoyalty.Application.Consumers.Customers
{
    public class UpdateCustomerProfileAfterInitConsumer : IConsumer<UpdateCustomerProfileAfterInitCommandMessage>
    {
        private readonly IUpdateCustomerProfileAfterInitCommandHandler _updateCustomerProfileAfterInitCommandHandler;

        public UpdateCustomerProfileAfterInitConsumer (IUpdateCustomerProfileAfterInitCommandHandler updateCustomerProfileAfterInitCommandHandler)
        {
            _updateCustomerProfileAfterInitCommandHandler = updateCustomerProfileAfterInitCommandHandler;
        }

        public async Task Consume(ConsumeContext<UpdateCustomerProfileAfterInitCommandMessage> context)
        {
            var message = context.Message;

            await _updateCustomerProfileAfterInitCommandHandler.ExecuteAsync(new UpdateCustomerProfileAfterInitCommand
            {
                CustomerId = message.CustomerId,
                PhoneCountryCode = message.PhoneCountryCode,
                PhoneNumber = message.PhoneNumber,
                UpdatedBy = message.UpdatedBy,
                DateOfBirth = message.DateOfBirth,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                Gender = message.Gender
            });
        }
    }
}
