using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Services.Activity
{
    public class LoggingActivityRequest : BaseRequest
    {
        public ActionArea ActionAreaPath { get; set; }
        public ActionType ActionType { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string CreatedByName { get; set; }
        public string Value { get; set; }
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
        ChangeMobilePhoneNumber,
        DeleteAppSetting,
        AddAppSetting,
        UpdateOutlet
    }
}
