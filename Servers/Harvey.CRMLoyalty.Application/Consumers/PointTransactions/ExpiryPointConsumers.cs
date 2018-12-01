using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ExpiryPointCommandHandler;
using Harvey.Message.PointTransactions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.PointTransactions
{
    public class ExpiryPointConsumers : IConsumer<ExpiryPointCommandMessage>
    {
        private readonly IExpiryPointCommandHandler _expiryPointCommandHandler;
        public ExpiryPointConsumers(IExpiryPointCommandHandler expiryPointCommandHandler)
        {
            _expiryPointCommandHandler = expiryPointCommandHandler;
        }
        public async Task Consume(ConsumeContext<ExpiryPointCommandMessage> context)
        {
            var command = new ExpiryPointCommand();
            command.Date = context.Message.Date;
            await _expiryPointCommandHandler.ExecuteAsync(command);
        }
    }
}
