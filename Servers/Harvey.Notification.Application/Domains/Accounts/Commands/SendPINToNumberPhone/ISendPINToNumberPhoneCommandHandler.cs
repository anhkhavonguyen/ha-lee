using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendPINToNumberPhone
{
    public interface ISendPINToNumberPhoneCommandHandler
    {
        Task ExecuteAsync(SendPINToNumberPhoneCommand sendPINToNumberPhoneCommand);
    }
}
