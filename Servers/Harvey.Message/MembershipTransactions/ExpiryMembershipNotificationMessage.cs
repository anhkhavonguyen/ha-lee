using System;

namespace Harvey.Message.MembershipTransactions
{
    public interface ExpiryMembershipNotificationMessage
    {
        DateTime Date { get; set; }
    }
}
