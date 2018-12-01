using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Services.SMSService
{
    public interface ISMSService
    {
        Task<PublishResponse> SendAsync(string phoneNumber, string title, string content);
        Task SendToMultiplePhoneNumbers();
    }
}
