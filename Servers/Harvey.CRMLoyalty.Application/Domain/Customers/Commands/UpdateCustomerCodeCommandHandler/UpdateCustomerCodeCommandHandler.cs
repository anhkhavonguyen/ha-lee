using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerCodeCommandHandler
{
    public class UpdateCustomerCodeCommandHandler : IUpdateCustomerCodeCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _harveyCRMLoyaltyDbContext;
        private readonly SemaphoreSlim _semaphoreSlim;

        public UpdateCustomerCodeCommandHandler(HarveyCRMLoyaltyDbContext harveyCRMLoyaltyDbContext)
        {
            _harveyCRMLoyaltyDbContext = harveyCRMLoyaltyDbContext;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async Task ExcuteAsync(UpdateCustomerCodeCommand models)
        {
            try
            {
                const string MigrationOutletCode = "MO";
                await _semaphoreSlim.WaitAsync();
                var outlet = _harveyCRMLoyaltyDbContext.Outlets.Find(models.OutletId);
                if(outlet == null)
                {
                    outlet = _harveyCRMLoyaltyDbContext.Outlets.FirstOrDefault(x => x.Code == MigrationOutletCode);
                }
                var index = _harveyCRMLoyaltyDbContext.Customers.Count(a => a.FirstOutlet == outlet.Name &&  a.CreatedDate <= models.CreatedDate && a.CreatedDate.Value.ToString("MMyy") == DateTime.UtcNow.ToString("MMyy")).ToString().PadLeft(5,'0');
                var dateSection = DateTime.UtcNow.ToString("MMyy");
                var customerCode = $"{outlet.Code}{dateSection}{index}";
                var entity = _harveyCRMLoyaltyDbContext.Customers.Find(models.CustomerId);
                entity.CustomerCode = customerCode;
                _harveyCRMLoyaltyDbContext.Customers.Update(entity);
                _harveyCRMLoyaltyDbContext.SaveChanges();
            }
            finally
            {

                _semaphoreSlim.Release();
            }
        }
    }
}
