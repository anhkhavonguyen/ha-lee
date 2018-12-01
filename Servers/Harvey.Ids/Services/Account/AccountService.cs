using Harvey.Ids.Application.User.Command.CreateUserProfile;
using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.Account.Commands;
using Harvey.Ids.Utils;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Account
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly IEmailSender _emailSender;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IBusControl bus,
            IConfiguration configuration,
            HarveyIdsDbContext harveyIdsDbContext,
            IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _bus = bus;
            _configuration = configuration;
            _harveyIdsDbContext = harveyIdsDbContext;
            _emailSender = emailSender;
        }

        public async Task SendEmailForgotPasswordAsync(ForgotPasswordViaEmailCommand forgotPasswordCommand, string currentPageUrl)
        {
            var user = forgotPasswordCommand.User;
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = user.Id });
            var shortLinkId = StringExtension.RandomString();
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _harveyIdsDbContext.SaveChangesAsync();
            string shortUrl = $"{currentPageUrl}/Account/ResetPassword?shortLinkId={shortLinkId}";

            var template = TemplateConfig.GetTemplates().Where(x => x.TemplateKey == TemplateConfig.MVC_MANAGEMENT_APP_EMAIL_RESET_PWD).FirstOrDefault();
            string content = string.Format(template.Content, user.FirstName, user.LastName, shortUrl);

            var emails = new List<string>()
            {
                user.Email
            };
            await _emailSender.SendEmailAsync(emails, template.Title, content);
        }


        public async Task SendMailConfirmUserAsync(ApplicationUser user, string currentPageUrl)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = user.Id });
            var shortLinkId = StringExtension.RandomString();
            _harveyIdsDbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _harveyIdsDbContext.SaveChangesAsync();
            string shortUrl = $"{currentPageUrl}/Account/ResetPassword?shortLinkId={shortLinkId}";
            var template = TemplateConfig.GetTemplates().Where(x => x.TemplateKey == TemplateConfig.MVC_MANAGEMENT_APP_CONFIRM_NEW_USER).FirstOrDefault();
            string content = string.Format(template.Content, user.FirstName, user.LastName, shortUrl);
            var emails = new List<string>()
            {
                user.Email
            };
            await _emailSender.SendEmailAsync(emails, template.Title, content);
        }
    }
}