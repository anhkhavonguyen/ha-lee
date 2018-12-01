using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendNotificationInitAccount
{
    public interface ISendNotificationInitAccountCommandHandler
    {
        Task ExecuteAsync(string countryCode, string phoneNumber,string outletName,string signUpShortLink,string pin);
    }
}
