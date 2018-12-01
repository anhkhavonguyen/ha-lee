using Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Ids.Consumers.Customer
{
    public class InitCustomerProfileConsumer : IConsumer<InitMemberProfileCommand>
    {
        private readonly IInitMemberAccountCommandHandler _initMemberAccountCommandHandler;
        public InitCustomerProfileConsumer(IInitMemberAccountCommandHandler initMemberAccountCommandHandler)
        {
            _initMemberAccountCommandHandler = initMemberAccountCommandHandler;
        }

        public async Task Consume(ConsumeContext<InitMemberProfileCommand> context)
        {
            var message = context.Message;

            var command = new InitMemberAccountCommand {
                PhoneCountryCode = message.PhoneCountryCode,
                PhoneNumber = message.PhoneNumber,
                UserId = message.UserId,
                CurrentUserId = message.CreatedBy,
                OriginalUrl = message.OriginalUrl,
                OutletName = message.OutletName
            };
            

            await _initMemberAccountCommandHandler.ExecuteAsync(command);
        }
    }
}
