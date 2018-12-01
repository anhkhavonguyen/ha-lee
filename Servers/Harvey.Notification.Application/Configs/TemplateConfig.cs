using Harvey.Notification.Application.Entities;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Configs
{
    public static class TemplateConfig
    {
        public const string SMS_INIT_ACCOUNT= "SMS_INIT_ACCOUNT";
        public const string EMAIL_RESET_PWD = "EMAIL_RESET_PWD";
        public const string SMS_RESET_PWD = "SMS_RESET_PWD";
        public const string SMS_SEND_PIN = "SMS_SEND_PIN";
        public const string SMS_SEND_CHANGE_MOBILE_PHONE = "SMS_SEND_CHANGE_MOBILE_PHONE";
        public const string SMS_SEND_EXPIRY_MEMBERSHIP_NOTIFICATION = "SMS_SEND_EXPIRY_MEMBERSHIP_NOTIFICATION";
        public const string SMS_SEND_EXPIRY_REWARD_POINT_NOTIFICATION = "SMS_SEND_EXPIRY_REWARD_POINT_NOTIFICATION";

        public static List<Template> GetTemplates()
        {
            return new List<Template> {
                new Template
                {
                    Title = "Send SMS Init Account",
                    DisplayName = "Send SMS Init Account",
                    TemplateKey = SMS_INIT_ACCOUNT,
                    Content = "Welcome to {0}! Your PIN is {1} . To earn loyalty points and rewards, click {2} and register."
                },
                new Template
                {
                    Title = "Send Email Reset Password",
                    DisplayName = "Send Email Reset Password",
                    TemplateKey = EMAIL_RESET_PWD,
                    Content = @"<!DOCTYPE html><html lang=""en""><head> <meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <style> * {{ margin: 0; padding: 0; box-sizing: border-box }} body {{ font-size: 16px; font-weight: 400; line-height: 1.7; }} .email-container {{ display: flex; flex-direction: column; width: fit-content; height: auto; }} .mb-20 {{ margin-bottom: 20px; }} .btn {{ display: inline-block; padding: 6px 12px; margin-bottom: 0; font-size: 14px; font-weight: 400; line-height: 1.42857143; text-align: center; white-space: nowrap; vertical-align: middle; -ms-touch-action: manipulation; touch-action: manipulation; cursor: pointer; -webkit-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none; background-image: none; border: 1px solid transparent; border-radius: 4px; text-decoration: none; }} .button-wrapper {{ display: flex; flex-direction: row; justify-content: center; }} .btn--confirm {{ background-color: #e42c00; color: white; margin-bottom: 20px; height: 50px; width: 250px; font-size: 25px; }} </style></head><body> <section class=""email-container""> <p class=""mb-20"">Hi {0} {1}</p> <p class=""mb-20"">Forgot your password? Don't worry, we're here to help! Simply tap the button below to reset your password.</p> <div class=""button-wrapper""> <a type=""button"" href=""{2}"" class=""btn btn--confirm"">Reset Password</a> </div> <p class=""mb-20"">If you did not make this request, simply ignore this email. If you have any questions, please contact us at https://toyorgame.com.sg/pages/contact-us</p> <p>Thank you for shopping with us! <br/> {3} - {4} Support</p> </section></body></html>"
                },
                new Template
                {
                    Title = "Welcome to {0}, This is your PIN",
                    DisplayName = "Send PIN to user",
                    TemplateKey = SMS_SEND_PIN,
                    Content = "Welcome to {0}! Your PIN is {1}."
                },
                new Template
                {
                    Title = "Send SMS Reset Password",
                    DisplayName = "Send SMS Reset Password",
                    TemplateKey = SMS_RESET_PWD,
                    Content = "Welcome to {0}! Please click {1} to reset your member account password."
                },
                new Template
                {
                    Title = "Send SMS Change Mobile Phone",
                    DisplayName = "Send Change Mobile Phone",
                    TemplateKey = SMS_SEND_CHANGE_MOBILE_PHONE,
                    Content = "Welcome to {0}! Your phone number has been updated successfully. {1} {2} {3}"
                },
                new Template
                {
                    Title = "Reminder expiry membership",
                    DisplayName = "Reminder expiry membership",
                    TemplateKey = SMS_SEND_EXPIRY_MEMBERSHIP_NOTIFICATION,
                    Content = "Your {0} Membership is expiring on {1}! Visit {0} outlets and redeem rewards now! \n {2}"
                },
                new Template
                {
                    Title = "Reminder expiry reward points",
                    DisplayName = "Reminder expiry reward points",
                    TemplateKey = SMS_SEND_EXPIRY_REWARD_POINT_NOTIFICATION,
                    Content = "{0} Member Rewards Points are expiring on {1}! Visit {2} outlets and redeem rewards now \n {3}"
                }
            };
        }
    }
}
