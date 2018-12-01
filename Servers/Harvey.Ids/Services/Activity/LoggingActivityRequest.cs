using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Activity
{
    public class LoggingActivityRequest
    {
        public string UserId { get; set; }
        public ActionArea ActionAreaPath { get; set; }
        public ActionType ActionType { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string CreatedByName { get; set; }
    }

    public enum ActionArea
    {
        StoreApp,
        MemberApp,
        AdminApp,
        Job
    }

    public enum ActionType
    {
        AddPoint,
        RedeemPoint,
        TopUp,
        Spending,
        Void,
        ExpiryPoint,
        UpdateAppSetting,
        InitCustomer,
        LogInStoreApp,
        LogInAdminApp,
        LogInMemberApp,
        ActiveCustomer,
        DeActiveCustomer,
        LoginServingCustomer,
        UpdateCustomerInfomation,
        UpdateCustomerProfile,
        ChangeMobilePhoneNumber
    }
}
