using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Commands.UpdateOutletCommandHandler
{
    public class UpdateOutletCommandHandler : IUpdateOutletCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private IOptions<ConfigurationRabbitMq> _config;
        private ILoggingActivityService _loggingActivityService;
        private const string UpdateOutlet = "Update Outlet";

        public UpdateOutletCommandHandler(HarveyCRMLoyaltyDbContext dbContext,
                                          IOptions<ConfigurationRabbitMq> config,
                                          ILoggingActivityService loggingActivityService)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<string> ExecuteAsync(UpdateOutletCommand command)
        {
            var user = _dbContext.Staffs.Where(x => x.Id == command.UserId).FirstOrDefault();
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : (command.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : "");


            if (command == null)
                return null;
            var outlet = await _dbContext.Outlets.FirstOrDefaultAsync(x => !string.IsNullOrEmpty(command.Id) && x.Id == command.Id);
            if (outlet == null)
                return null; 

            var oldName = outlet.Name != command.Name ? "Name: " + outlet.Name : "";
            var oldPhoneCountryCode = outlet.PhoneCountryCode != command.PhoneCountryCode ? "Phone Country Code: " + outlet.PhoneCountryCode : "";
            var oldPhone = outlet.Phone != command.Phone ? "Phone: " + outlet.Phone : "";
            var oldAddress = outlet.Address != command.Address ? "Address: " + outlet.Address : "";
            var oldData = oldName != "" || 
                          oldPhoneCountryCode != "" ||
                          oldPhone != "" ||
                          oldAddress != "" ? $"Old Data: {oldName} {oldPhoneCountryCode} {oldPhone} {oldAddress}" : "";

            var updatedName = outlet.Name != command.Name ? "Name: " + command.Name : "";
            var updatedPhoneCountryCode = outlet.PhoneCountryCode != command.PhoneCountryCode ? "Phone Country Code: " + command.PhoneCountryCode : "";
            var updatedPhone = outlet.Phone != command.Phone ? "Phone: " + command.Phone : "";
            var updatedAddress = outlet.Address != command.Address ? "Address: " + command.Address : "";
            var updateIcon = outlet.OutletImage != command.OutletImage ? "Outlet Logo has changed." : "";
            var updatedData = updatedName != "" ||
                              updatedPhoneCountryCode != "" ||
                              updatedPhone != "" ||
                              updatedAddress != "" ? $"Updated Data: {updatedName} {updatedPhoneCountryCode} {updatedPhone} {updatedAddress}" : "";
            
            var logDesciption = $"{oldData} {updatedData} {updateIcon}";

            outlet.Name = command.Name != null ? command.Name.Trim() : outlet.Name;
            outlet.Address = command.Address != null ? command.Address.Trim() : outlet.Address;
            outlet.PhoneCountryCode = command.PhoneCountryCode != null ? command.PhoneCountryCode.Trim() : outlet.PhoneCountryCode;
            outlet.Phone = command.Phone != null ? command.Phone.Trim() : outlet.Phone;
            outlet.OutletImage = command.OutletImage;

            _dbContext.Outlets.Update(outlet);
            await LogAction(command.UserId, userName, logDesciption, _config.Value.RabbitMqUrl);
            await _dbContext.SaveChangesAsync();
            return outlet.Id;
        }

        private async Task LogAction(string userId, string userName, string logDesciption, string rabbitMqUrl)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = UpdateOutlet;
            request.Comment = logDesciption;
            request.ActionType = ActionType.UpdateOutlet;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
