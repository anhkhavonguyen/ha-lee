using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.PointTransactions
{
    public interface ExpiryRewardPointNotificationMessage
    {
        DateTime Date { get; set; }
    }
}
