using Harvey.Ids.Application.Accounts.Commands.ActiveCustomerCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Ids.Consumers.Customers
{
    public class ActiveCustomerConsumer : IConsumer<ActiveCustomerCommandMessage>
    {
        private readonly IActiveCustomerCommandHandler _commandHandler;
        public ActiveCustomerConsumer(IActiveCustomerCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task Consume(ConsumeContext<ActiveCustomerCommandMessage> context)
        {
            var message = context.Message;
            var command = new ActiveCustomerCommand();
            command.CustomerId = message.CustomerId;
            command.IsActive = message.IsActive;
            command.PhoneCountryCode = message.PhoneCountryCode;
            command.PhoneNumber = message.PhoneNumber;
            command.FirstName = message.FirstName;
            command.LastName = message.LastName;
            command.Email = message.Email;
            command.DateOfBirth = message.DateOfBirth;
            command.Gender = message.Gender;
            await _commandHandler.ExecuteAsync(command);
        }
    }
}
