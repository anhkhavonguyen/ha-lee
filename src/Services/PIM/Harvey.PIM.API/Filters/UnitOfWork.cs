using Harvey.PIM.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Harvey.PIM.API.Filters
{
    public class EfUnitOfWork : IActionFilter
    {
        private readonly PimDbContext _pimDbContext;
        private readonly TransactionDbContext _transactionDbContext;
        public EfUnitOfWork(PimDbContext pimDbContext, TransactionDbContext transactionDbContext)
        {
            _pimDbContext = pimDbContext;
            _transactionDbContext = transactionDbContext;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _pimDbContext.SaveChangesAsync().Wait();
                _transactionDbContext.SaveChangesAsync().Wait();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
