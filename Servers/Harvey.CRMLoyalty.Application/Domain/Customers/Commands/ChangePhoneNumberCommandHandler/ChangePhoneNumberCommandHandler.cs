using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Services.Activity;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ChangePhoneNumberCommandHandler
{
    public class ChangePhoneNumberCommandHandler : IChangePhoneNumberCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public ChangePhoneNumberCommandHandler(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(ChangePhoneNumberCommand command)
        {
            var customer = await _dbContext.Customers.FindAsync(command.CustomerId);

            customer.PhoneCountryCode = command.NewPhoneCountryCode;
            customer.Phone = command.NewPhoneNumber;
            customer.UpdatedBy = command.UpdatedBy;

            await _dbContext.SaveChangesAsync();
        }
    }
}
