using Harvey.CRMLoyalty.Api;
using System;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateFullCustomerInfomationCommandHandler
{
    public class UpdateFullCustomerInfomationCommandHandler : IUpdateFullCustomerInfomationCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public UpdateFullCustomerInfomationCommandHandler(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(UpdateFullCustomerInfomationCommand command)
        {
            var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

            customer.PhoneCountryCode = command.NewPhoneCountryCode;
            customer.Phone = command.NewPhoneNumber;
            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.DateOfBirth = command.DateOfBirth;
            customer.Email = command.Email;
            if (command.Gender != null)
                customer.Gender = (Data.Gender)(System.Int32.Parse(command.Gender));
            else
                customer.Gender = null;

            customer.UpdatedDate = DateTime.UtcNow;
            customer.UpdatedBy = command.UpdatedBy;

            await _dbContext.SaveChangesAsync();
        }
    }
}
