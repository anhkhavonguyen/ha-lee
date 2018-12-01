using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvey.Activity.Api;
using Harvey.Activity.Application.Data;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities.Model;
using Harvey.Activity.Application.Extensions.PagingExtensions;
using Harvey.Activity.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities
{
    public class GetActivatedCustomerActivitiesQuery : IGetActivatedCustomerActivitiesQuery
    {
        private readonly HarveyActivityDbContext _dbContext;

        public GetActivatedCustomerActivitiesQuery(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetActivatedCustomerActivitiesResponse Execute(GetActivatedCustomerActivitiesRequest request)
        {
            var response = new GetActivatedCustomerActivitiesResponse();

            if (!request.FromDateFilter.HasValue || !request.ToDateFilter.HasValue)
            {
                response.ActionModels = new List<ActionActivityModel>();
                return response;
            }

            var activeCustomerActionType = ((int)ActionType.ActiveCustomer).ToString();
            var query = _dbContext.Activities.AsNoTracking().Where(x => x.ActionTypeId == activeCustomerActionType
                                    && x.CreatedDate >= request.FromDateFilter
                                    && x.CreatedDate <= request.ToDateFilter)
                            .GroupBy(x => x.Description)
                            .Select(group => group.OrderByDescending(x => x.CreatedDate).FirstOrDefault())
                            .Select(entity => new ActionActivityModel
                            {
                                Id = entity.Id,
                                Description = entity.Description,
                                UpdatedBy = entity.UpdatedBy,
                                UpdatedDate = entity.UpdatedDate.Value,
                                CreatedBy = entity.CreatedBy,
                                CreatedDate = entity.CreatedDate.Value,
                                Comment = entity.Comment,
                                ActionArea = entity.ActionAreaId,
                                ActionType = entity.ActionTypeId,
                                CreatedByName = entity.CreatedByName
                            }).OrderByDescending(x => x.CreatedDate).AsQueryable();
            
            var result = PagingExtensions.GetPaged<ActionActivityModel>(query, request.PageNumber, request.PageSize);

            response.TotalItem = query.Distinct().Count();
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ActionModels = result.Results.ToList();

            return response;
        }
    }
}
