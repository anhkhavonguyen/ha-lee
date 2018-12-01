using System.Linq;
using Harvey.Activity.Api;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities.Model;
using Harvey.Activity.Application.Extensions.PagingExtensions;
using Harvey.Activity.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities
{
    public class GetHistoryChangeNumberCustomerQuery : IGetHistoryChangeNumberCustomerQuery
    {
        private readonly HarveyActivityDbContext _dbContext;
        public GetHistoryChangeNumberCustomerQuery(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public GetHistoryChangeNumberCustomerRespone Execute(GetHistoryChangeNumberCustomerRequest request)
        {
            var query = _dbContext.Activities
                .Where(a=>a.ActionTypeId == request.ActionType && a.Description == request.CustomerCode)
                .Include(x => x.ActionType)
                .Include(x => x.AreaActivity)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new ActionActivityModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Comment = x.Comment,
                    ActionType = x.ActionTypeId,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate.Value,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.Value,
                    ActionArea = x.ActionAreaId,
                    CreatedByName = x.CreatedByName
                })
               .OrderByDescending(x => x.CreatedDate.Value)
               .AsQueryable();

            var result = PagingExtensions.GetPaged<ActionActivityModel>(query, request.PageNumber, request.PageSize);

            var response = new GetHistoryChangeNumberCustomerRespone();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ActionModels = result.Results.ToList();
            return response;
        }
    }
}
