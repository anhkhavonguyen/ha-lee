using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ChangePhoneNumberCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.Customers
{
    public class ChangePhoneNumberConsumer : IConsumer<ChangePhoneNumberCommandMessage>
    {
        private readonly IChangePhoneNumberCommandHandler _changePhoneNumberCommandHandler;

        public ChangePhoneNumberConsumer(IChangePhoneNumberCommandHandler changePhoneNumberCommandHandler)
        {
            _changePhoneNumberCommandHandler = changePhoneNumberCommandHandler;
        }

        public async Task Consume(ConsumeContext<ChangePhoneNumberCommandMessage> context)
        {
            var message = context.Message;
            await _changePhoneNumberCommandHandler.ExecuteAsync(new ChangePhoneNumberCommand
            {
                CustomerId = message.CustomerId,
                NewPhoneCountryCode = message.NewPhoneCountryCode,
                NewPhoneNumber = message.NewPhoneNumber,
                UpdatedBy = message.UpdatedBy,
            });
        }
    }
}
