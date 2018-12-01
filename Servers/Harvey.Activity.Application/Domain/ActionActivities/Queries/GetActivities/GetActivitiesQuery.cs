using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvey.Activity.Api;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities.Model;
using Harvey.Activity.Application.Entities;
using Harvey.Activity.Application.Extensions.PagingExtensions;
using Harvey.Activity.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities
{
    public class GetActivitiesQuery : IGetActivitiesQuery
    {
        private readonly HarveyActivityDbContext _dbContext;
        public GetActivitiesQuery(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetActivitiesResponse Execute(GetActivitiesRequest request)
        {
            var query = _dbContext.Activities
                .Include(x => x.ActionType)
                .Include(x => x.AreaActivity)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new ActionActivityModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate.Value,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.Value,
                    Comment = x.Comment,
                    ActionArea = x.ActionAreaId,
                    ActionType = x.ActionTypeId,
                    CreatedByName = x.CreatedByName
                })
               .OrderByDescending(x => x.CreatedDate.Value)
               .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var searchString = request.SearchText.ToLower().Trim();
                query = query.Where(x => (x.Description != null && x.Description.ToLower().Trim().Contains(searchString))
                            || (x.Comment != null && x.Comment.ToLower().Trim().Contains(searchString)));
            }

            var result = PagingExtensions.GetPaged<ActionActivityModel>(query, request.PageNumber, request.PageSize);

            var response = new GetActivitiesResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ActionActivityModels = result.Results.ToList();
            return response;
        }
    }
}
