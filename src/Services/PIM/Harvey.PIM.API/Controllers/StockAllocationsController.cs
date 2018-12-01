using Harvey.PIM.API.Extensions;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/stockallocations")]
    [Authorize]
    public class StockAllocationsController : ControllerBase
    {
        private readonly TransactionDbContext _transactionDbContext;
        public StockAllocationsController(TransactionDbContext transactionDbContext)
        {
            _transactionDbContext = transactionDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]StockAllocationModel stockAllocationModel)
        {
            var userId = User.GetUserId();

            var transactionType = _transactionDbContext.TransactionTypes.AsNoTracking().Where(x => x.Code == "GIW").FirstOrDefault();
            Guid fromLocationId = Guid.Parse("508c1b19-5e5d-440b-9414-42f7de8879d4");
            if (transactionType != null && fromLocationId != null)
            {
                var gIWDocumentId = Guid.NewGuid();
                var gIWDocument = new GIWDocument
                {
                    Id = gIWDocumentId,
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString()
                };

                await _transactionDbContext.GIWDocuments.AddAsync(gIWDocument);

                var inventoryTransaction = new InventoryTransaction
                {
                    Id = Guid.NewGuid(),
                    GIWDocumentId = gIWDocumentId,
                    FromLocationId = fromLocationId,
                    ToLocationId = stockAllocationModel.ToLocationId,
                    TransactionTypeId = transactionType.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = userId
                };

                await _transactionDbContext.InventoryTransactions.AddAsync(inventoryTransaction);

                if (stockAllocationModel.AllocationProducts != null && stockAllocationModel.AllocationProducts.Count > 0)
                {
                    foreach (var item in stockAllocationModel.AllocationProducts)
                    {
                        await _transactionDbContext.GIWDocumentItems.AddAsync(new GIWDocumentItem
                        {
                            Id = Guid.NewGuid(),
                            GIWDocumentId = gIWDocumentId,
                            VariantId = item.VariantId,
                            StockTypeId = item.StockTypeId,
                            Quantity = item.Quantity
                        });

                        var lastStockTransaction = _transactionDbContext.StockTransactions.AsNoTracking()
                            .OrderByDescending(x => x.CreatedDate)
                            .Where(x => x.VariantId == item.VariantId
                            && x.StockTypeId == item.StockTypeId
                            && x.ToLocationId == stockAllocationModel.ToLocationId).FirstOrDefault();

                        await _transactionDbContext.StockTransactions.AddAsync(new StockTransaction
                        {
                            Id = Guid.NewGuid(),
                            FromLocationId = fromLocationId,
                            ToLocationId = stockAllocationModel.ToLocationId,
                            VariantId = item.VariantId,
                            StockTypeId = item.StockTypeId,
                            TransactionTypeId = transactionType.Id,
                            Quantity = item.Quantity,
                            Balance = lastStockTransaction != null ? (lastStockTransaction.Balance + item.Quantity) : item.Quantity,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = userId
                        });
                    }
                }

                await _transactionDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Can't find Transaction Type GIW");
            }
        }
    }
}
