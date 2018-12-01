using Harvey.Ids.Application.Accounts.Commands.ReactiveCustomerWithNewPhoneCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Ids.Consumers.Customers
{
    public class ReactiveCustomerWithNewPhoneConsumer : IConsumer<ReactiveCustomerWithNewPhoneMessageCommand>
    {
        private readonly IReactiveCustomerWithNewPhoneCommandHandler _commandHandler;
        public ReactiveCustomerWithNewPhoneConsumer(IReactiveCustomerWithNewPhoneCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task Consume(ConsumeContext<ReactiveCustomerWithNewPhoneMessageCommand> context)
        {
            var message = context.Message;
            var command = new ReactiveCustomerWithNewPhoneCommand();
            command.CustomerId = message.CustomerId;
            command.IsActive = message.IsActive;
            command.PhoneCountryCode = message.PhoneCountryCode;
            command.PhoneNumber = message.PhoneNumber;
            command.FirstName = message.FirstName;
            command.LastName = message.LastName;
            command.Email = message.Email;
            command.DateOfBirth = message.DateOfBirth;
            command.Gender = message.Gender;
            command.MemberOriginalUrl = message.MemberOriginalUrl;
            command.AcronymBrandName = message.AcronymBrandName;
            await _commandHandler.ExecuteAsync(command);
        }
    }
}
