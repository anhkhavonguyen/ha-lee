using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Configs
{
    public static class TemplateConfig
    {
        public const string MVC_MANAGEMENT_APP_EMAIL_RESET_PWD = "MVC_MANAGEMENT_APP_EMAIL_RESET_PWD";
        public const string MVC_MANAGEMENT_APP_CONFIRM_NEW_USER = "MVC_MANAGEMENT_APP_CONFIRM_NEW_USER";

        public static List<Template> GetTemplates()
        {
            return new List<Template> {
                new Template
                {
                    Title = "Send Email Reset Password",
                    DisplayName = "Send Email Reset Password",
                    TemplateKey = MVC_MANAGEMENT_APP_EMAIL_RESET_PWD,
                    Content = @"<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head><body><section class=""email-container""><p class=""mb-20"">Hi {0} {1}</p><p class=""mb-20"">Forgot your password? Don't worry, we're here to help! Simply tap the button below to reset your password. </p><p class=""mb-20"">Click the link below to reset your password</p><div class=""button-wrapper""><a type=""button"" href=""{2}"" class=""btn btn--confirm"">Reset Password</a></div><br><p class=""mb-20"">If you need help, please contact the site administrator</p></section></body></html>"
                },

                new Template
                {
                    Title = "Send Email Confirm New User",
                    DisplayName = "Send Email Confirm New User",
                    TemplateKey = MVC_MANAGEMENT_APP_CONFIRM_NEW_USER,
                    Content= @"<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head><body><section class=""email-container""><p class=""mb-20"">Hi {0} {1}</p><p class=""mb-20"">A new account has been created using your email address </p><p class=""mb-20"">Click the link below to reset your password</p><div class=""button-wrapper""><a type=""button"" href=""{2}"" class=""btn btn--confirm"">Reset Password</a></div><br><p class=""mb-20"">If you need help, please contact the site administrator</p></section></body></html>"

                }
            };
        }
    }

    public class Template
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string DisplayName { get; set; }
        public string TemplateKey { get; set; }
    }
}