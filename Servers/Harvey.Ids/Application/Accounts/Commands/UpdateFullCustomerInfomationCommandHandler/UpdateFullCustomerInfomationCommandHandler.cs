using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.Activity;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Harvey.Ids.Utils;
using Harvey.Message.Customers;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler
{
    internal class UpdateFullCustomerInfomationCommandHandler : IUpdateFullCustomerInfomationCommandHandler
    {
        private readonly HarveyIdsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggingActivityService _activityService;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;
        private readonly IGenerateShortLinkFromTinyUrlService _generateShortLinkFromTinyUrlService;
        private const string DuplicatePhoneNumber = "duplicate_phone_number";

        public UpdateFullCustomerInfomationCommandHandler(UserManager<ApplicationUser> userManager, ILoggingActivityService activityService, IOptions<ConfigurationRabbitMq> configurationRabbitMq, IBusControl bus,
            HarveyIdsDbContext dbContext, IGenerateShortLinkFromTinyUrlService generateShortLinkFromTinyUrlService)
        {
            _userManager = userManager;
            _activityService = activityService;
            _configurationRabbitMq = configurationRabbitMq;
            _bus = bus;
            _dbContext = dbContext;
            _generateShortLinkFromTinyUrlService = generateShortLinkFromTinyUrlService;
        }

        public async Task<string> ExecuteAsync(UpdateFullCustomerInfomationCommand command)
        {
            if (command == null)
                return null;

            var customer = await _userManager.FindByIdAsync(command.CustomerId);
            if (customer == null)
                return null;

            //TODO: if user change phone country code or phone number
            if ((command.NewPhoneCountryCode != customer.PhoneCountryCode) || (command.NewPhoneNumber != customer.PhoneNumber))
            {
                var newPhoneNumber = $"{command.NewPhoneCountryCode}{command.NewPhoneNumber}";
                if (await IsUserExist(newPhoneNumber, customer.Id))
                    return DuplicatePhoneNumber;

                await NotifySmsToCustomer(command, customer);
            }

            var trackData = await UpdateData(command, customer);
            if (trackData == DuplicatePhoneNumber)
                return DuplicatePhoneNumber;

            //Fire message to CRM update customer phone
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "update_full_customer_infomation_queue")));
            await sendEndpointTask.Send<UpdateFullCustomerInfomationCommandMessage>(new
            {
                CustomerId = customer.Id,
                NewPhoneCountryCode = command.NewPhoneCountryCode,
                NewPhoneNumber = command.NewPhoneNumber,
                UpdatedBy = command.UserId,
                DateOfBirth = command.DateOfBirth,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Gender = command.Gender,
                PostalCode = command.PostalCode,
                AcronymBrandName = command.AcronymBrandName
            });

            var user = await _userManager.FindByIdAsync(command.UserId);
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : "";

            await LogAction(command.UserId, _configurationRabbitMq.Value.RabbitMqUrl, trackData, command.CustomerCode, userName);

            return customer.Id;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string trackData, string customerCode, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = trackData;
            request.ActionType = ActionType.UpdateCustomerInfomation;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _activityService.ExecuteAsync(request, rabbitMqUrl);
        }
        private async Task<string> GetResetPasswordShortLink(string originalUrl, ApplicationUser customer)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(customer);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(new { Code = code, UserId = customer.Id });
            var shortLinkId = StringExtension.RandomString();
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = value
            });
            await _dbContext.SaveChangesAsync();

            string shortUrl = $"{originalUrl}/auth/reset-pwd/{shortLinkId}";

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(shortUrl);
        }
        private async Task<string> GetUpdateProfileShortLink(string originalUrl, ApplicationUser customer)
        {
            string shortLinkId = StringExtension.RandomString();
            string signUpShortLink = $"{originalUrl}/su/{shortLinkId}";
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = customer.Id })
            });
            await _dbContext.SaveChangesAsync();

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(signUpShortLink);
        }
        private async Task<string> GetLoginShortLink(string originalUrl, ApplicationUser customer)
        {
            string shortLinkId = StringExtension.RandomString();
            string logInShortLink = $"{originalUrl}/auth/login/{shortLinkId}";
            _dbContext.ShortLinks.Add(new ShortLink
            {
                Id = shortLinkId,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = customer.Id })
            });
            await _dbContext.SaveChangesAsync();

            return await _generateShortLinkFromTinyUrlService.ExecuteAsync(logInShortLink);
        }
        private async Task<Boolean> IsUserExist(string newPhoneNumber, string customerId)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserName == newPhoneNumber && u.IsActive == true && u.Id != customerId);
        }
        private async Task NotifySmsToCustomer(UpdateFullCustomerInfomationCommand command, ApplicationUser customer)
        {
            if (!string.IsNullOrEmpty(command.NewPhoneCountryCode) || !string.IsNullOrEmpty(command.NewPhoneNumber))
            {
                var contentResetPasswordShortLink = string.Empty;
                var contentProfileShortLink = string.Empty;
                var contentLoginShortLink = string.Empty;

                if (customer.PasswordHash == null)
                {
                    var resetPasswordShortLink = await GetResetPasswordShortLink(command.MemberOriginalUrl, customer);
                    contentResetPasswordShortLink = string.Format("Please click {0} to reset your member account password.", resetPasswordShortLink);
                }
                else if (!customer.EmailConfirmed && !customer.PhoneNumberConfirmed)
                {
                    var updateProfileShortLink = await GetUpdateProfileShortLink(command.MemberOriginalUrl, customer);
                    contentProfileShortLink = string.Format("To earn loyalty points and rewards, click {0} to complete your profile.", updateProfileShortLink);
                }
                else
                {
                    var loginShortLink = await GetLoginShortLink(command.MemberOriginalUrl, customer);
                    contentLoginShortLink = string.Format("Please click {0} to login your member account.", loginShortLink);
                }

                ISendEndpoint sendSmsChangePhoneEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "send_sms_change_phone_number_queue")));
                await sendSmsChangePhoneEndpointTask.Send<SendSmsChangePhoneNumberCommand>(new
                {
                    CustomerId = customer.Id,
                    NewPhoneCountryCode = command.NewPhoneCountryCode,
                    NewPhoneNumber = command.NewPhoneNumber,
                    UpdatedBy = command.UserId,
                    ResetPasswordShortLink = contentResetPasswordShortLink,
                    UpdateProfileShortLink = contentProfileShortLink,
                    LoginShortLink = contentLoginShortLink,
                    AcronymBrandName = command.AcronymBrandName
                });
            }
        }
        private async Task<string> UpdateData(UpdateFullCustomerInfomationCommand command, ApplicationUser customer)
        {
            var oldData = "OldData: ";
            var updatedData = "UpdatedData: ";
            List<string> oldDataArray = new List<string>();
            List<string> newDataArray = new List<string>();

            if (!string.IsNullOrEmpty(command.NewPhoneCountryCode) || !string.IsNullOrEmpty(command.NewPhoneNumber))
            {
                var countryCode = customer.PhoneCountryCode;
                var phone = customer.PhoneNumber;

                if (!string.IsNullOrEmpty(command.NewPhoneCountryCode) && command.NewPhoneCountryCode != customer.PhoneCountryCode)
                {
                    oldDataArray.Add("CountryCode: " + customer.PhoneCountryCode);
                    countryCode = command.NewPhoneCountryCode;
                    newDataArray.Add("CountryCode: " + command.NewPhoneCountryCode);
                }
                if (!string.IsNullOrEmpty(command.NewPhoneNumber) && command.NewPhoneNumber != customer.PhoneNumber)
                {
                    oldDataArray.Add("PhoneNumber: " + customer.PhoneNumber);
                    phone = command.NewPhoneNumber;
                    newDataArray.Add("PhoneNumber: " + command.NewPhoneNumber);
                }
                var newPhoneNumber = $"{countryCode}{phone}";
                if (await IsUserExist(newPhoneNumber, customer.Id))
                    return DuplicatePhoneNumber;

                customer.PhoneNumber = phone;
                customer.PhoneCountryCode = countryCode;
                customer.UserName = newPhoneNumber;
                customer.NormalizedUserName = newPhoneNumber;
            }
            if (command.FirstName != customer.FirstName)
            {
                oldDataArray.Add("FirstName: " + customer.FirstName);
                customer.FirstName = command.FirstName;
                newDataArray.Add("FirstName: " + command.FirstName);
            }
            if (command.LastName != customer.LastName)
            {
                oldDataArray.Add("LastName: " + customer.LastName);
                customer.LastName = command.LastName;
                newDataArray.Add("LastName: " + command.LastName);
            }
            if (command.DateOfBirth.HasValue)
            {
                if (customer.DateOfBirth.HasValue)
                {
                    if (command.DateOfBirth.Value.Date != customer.DateOfBirth.Value.Date)
                    {
                        oldDataArray.Add("DateOfBirth: " + customer.DateOfBirth?.ToShortDateString());
                        customer.DateOfBirth = command.DateOfBirth;
                        newDataArray.Add("DateOfBirth: " + command.DateOfBirth.Value.ToShortDateString());
                    }
                } else
                {
                    oldDataArray.Add("DateOfBirth: " + customer.DateOfBirth?.ToShortDateString());
                    customer.DateOfBirth = command.DateOfBirth;
                    newDataArray.Add("DateOfBirth: " + command.DateOfBirth.Value.ToShortDateString());
                }
            }
            if (command.Email != customer.Email)
            {
                oldDataArray.Add("Email: " + customer.Email);
                customer.Email = command.Email;
                newDataArray.Add("Email: " + command.Email);
            }
            if (command.Gender != customer.Gender)
            {
                oldDataArray.Add("Gender: " + customer.Gender);
                customer.Gender = command.Gender;
                newDataArray.Add("Gender: " + command.Gender);
            }

            oldData = oldData + string.Join("-", oldDataArray);
            updatedData = updatedData + string.Join("-", newDataArray);

            customer.UpdatedDate = DateTime.UtcNow;
            customer.UpdatedBy = command.UserId;

            await _userManager.UpdateAsync(customer);

            return oldData + "\r\n" + updatedData;
        }
    }
}
