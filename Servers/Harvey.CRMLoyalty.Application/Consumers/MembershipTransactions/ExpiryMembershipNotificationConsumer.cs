using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler.Model;
using Harvey.Message.MembershipTransactions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.MembershipTransactions
{
    public class ExpiryMembershipNotificationConsumer : IConsumer<ExpiryMembershipNotificationMessage>
    {
        private readonly IExpiryMembershipNotificationCommand _expiryMembershipNotificationCommand;

        public ExpiryMembershipNotificationConsumer(IExpiryMembershipNotificationCommand expiryMembershipNotificationCommand)
        {
            _expiryMembershipNotificationCommand = expiryMembershipNotificationCommand;
        }

        public async Task Consume(ConsumeContext<ExpiryMembershipNotificationMessage> context)
        {
            var command = new ExpiryMembershipNotificationModel();
            command.Date = context.Message.Date;
            await _expiryMembershipNotificationCommand.ExecuteAsync(command);
        }
    }
}
