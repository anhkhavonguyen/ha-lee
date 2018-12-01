using Harvey.Notification.Application.Domains.Accounts.Commands.ReSendSignUpLink;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendEmailNotificationForgotPasswordAccount;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendNotificationInitAccount;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendPINToNumberPhone;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendSmsChangePhoneNumber;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendSMSNotificationForgotPasswordAccount;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Notification.Application.Domains.Accounts
{
    public class AccountApplicationModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<ISendNotificationInitAccountCommandHandler, SendNotificationInitAccountCommandHandler>();
            services.AddScoped<ISendSMSForgotPasswordAccountCommandHandler, SendSMSForgotPasswordAccountCommandHandler>();
            services.AddScoped<ISendEmailForgotPasswordAccountCommandHandler, SendEmailForgotPasswordAccountCommandHandler>();
            services.AddScoped<ISendPINToNumberPhoneCommandHandler, SendPINToNumberPhoneCommandHandler>();
            services.AddScoped<IReSendSignUpLinkCommandHandler, ReSendSignUpLinkCommandHandler>();
            services.AddScoped<ISendSmsChangePhoneNumberCommandHandler, SendSmsChangePhoneNumberCommandHandler>();
        }
    }
}
