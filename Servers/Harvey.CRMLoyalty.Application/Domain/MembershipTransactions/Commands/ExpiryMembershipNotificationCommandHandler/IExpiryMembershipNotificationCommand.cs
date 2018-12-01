using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler.Model;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler
{
    public interface IExpiryMembershipNotificationCommand
    {
        Task ExecuteAsync(ExpiryMembershipNotificationModel command);
    }
}
