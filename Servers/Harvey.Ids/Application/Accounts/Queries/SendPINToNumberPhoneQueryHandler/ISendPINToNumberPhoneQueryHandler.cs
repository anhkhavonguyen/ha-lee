using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.SendPINToNumberPhoneQueryHandler
{
    public interface ISendPINToNumberPhoneQueryHandler
    {
        Task ExecuteAsync(string countryCode, string phoneNumber, string acronymBrandName, string outletName, string userId = null);
    }
}
