using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.VoidMembershipCommandHandler
{
    public interface IVoidMembershipCommandHandler
    {
        Task<string> ExecuteAsync(VoidMembershipCommand command);
    }
}
