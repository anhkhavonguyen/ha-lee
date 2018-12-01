using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.PointTransactions
{
    public interface ExpiryPointCommandMessage
    {
        System.DateTime Date { get; set; }
    }
}
