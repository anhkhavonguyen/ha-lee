using Harvey.CRMLoyalty.Api;
using System;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerProfileAfterInitCommandHandler
{
    public class UpdateCustomerProfileAfterInitCommandHandler : IUpdateCustomerProfileAfterInitCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public UpdateCustomerProfileAfterInitCommandHandler(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ExecuteAsync(UpdateCustomerProfileAfterInitCommand command)
        {
            var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

            customer.PhoneCountryCode = !string.IsNullOrEmpty(command.PhoneCountryCode) ? command.PhoneCountryCode : customer.PhoneCountryCode;
            customer.Phone = !string.IsNullOrEmpty(command.PhoneNumber) ? command.PhoneNumber : customer.Phone;
            customer.FirstName = !string.IsNullOrEmpty(command.FirstName) ? command.FirstName : customer.FirstName;
            customer.LastName = !string.IsNullOrEmpty(command.LastName) ? command.LastName : customer.LastName;
            customer.DateOfBirth = command.DateOfBirth.HasValue ? command.DateOfBirth.Value : customer.DateOfBirth;
            customer.Email = !string.IsNullOrEmpty(command.Email) ? command.Email : customer.Email;
            customer.Gender = command.Gender != null ? (Data.Gender)(System.Int32.Parse(command.Gender)) : customer.Gender;

            customer.UpdatedDate = DateTime.UtcNow;
            customer.UpdatedBy = command.UpdatedBy;

            await _dbContext.SaveChangesAsync();
        }
    }
}
