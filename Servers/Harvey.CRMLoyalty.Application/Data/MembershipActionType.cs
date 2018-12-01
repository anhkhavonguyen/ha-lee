using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Data
{
    public enum MembershipActionType
    {
        Migration,
        New,
        Upgrade,
        Renew,
        Extend,
        Downgrade,
        Void,
        ChangeExpiredDate,
        Comment
    }
}
