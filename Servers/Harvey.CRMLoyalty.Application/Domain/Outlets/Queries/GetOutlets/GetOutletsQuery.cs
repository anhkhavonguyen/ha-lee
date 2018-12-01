using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Queries
{
    public class GetOutletsQuery : IGetOutletsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetOutletsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetOutletsResponse GetOutlets(GetOutletsRequest request)
        {
            var query = _dbContext.Outlets.AsNoTracking().Where(a=>a.Status == Status.Active).AsQueryable();
            var result = PagingExtensions.GetPaged<Outlet, OutletModel>(query, request.PageNumber, request.PageSize);
            var response = new GetOutletsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.OutletModels = result.Results;
            return response;
        }

        public GetOutletsResponse GetOutlets()
        {
            var query = _dbContext.Outlets.AsNoTracking().Where(a => a.Status == Status.Active).Select(o => new OutletModel
            {
                Address = o.Address,
                City = o.City,
                Email = o.Email,
                Id = o.Id,
                Name = o.Name,
                OutletImage = o.OutletImage,
                Phone = o.Phone,
                PhoneCountryCode = o.PhoneCountryCode,
                PostalCode = o.PostalCode,
                Code = o.Code
            });
            var response = new GetOutletsResponse();
            response.OutletModels = query.ToList();
            return response;
        }

        public GetOutletsResponse GetOutletsByStaff(GetOutletsRequest request)
        {
            var query = _dbContext.Outlets.AsNoTracking().Where(a => a.Status == Status.Active && a.Staff_Outlets.Select(b => b.StaffId).Contains(request.UserId)).AsQueryable();
            var result = PagingExtensions.GetPaged<Outlet, OutletModel>(query, request.PageNumber, request.PageSize);
            var response = new GetOutletsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.OutletModels = result.Results.OrderBy(x=>x.Name).ToList();
            return response;
        }

        public GetOutletsResponse GetOutletsWithStoreAccount(GetOutletsRequest request)
        {
            var query = _dbContext.Outlets.AsNoTracking().Where(a => a.Status == Status.Active).Select(o => new OutletModel
            {
                Id = o.Id,
                FirstNameAccount = o.Staff_Outlets.FirstOrDefault(so => so.OutletId == o.Id && so.Staff.TypeOfStaff == TypeOfStaff.StoreAccount).Staff.FirstName,
                LastNameAccount = o.Staff_Outlets.FirstOrDefault(so => so.OutletId == o.Id && so.Staff.TypeOfStaff == TypeOfStaff.StoreAccount).Staff.LastName,
                Name = o.Name,
                Phone = o.Phone,
                PhoneCountryCode = o.PhoneCountryCode,
                OutletImage = o.OutletImage,
                Address = o.Address,
                Code = o.Code
            }).AsQueryable();

            var result = PagingExtensions.GetPaged<OutletModel>(query, request.PageNumber, request.PageSize);
            var response = new GetOutletsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.OutletModels = result.Results;
            return response;
        }
    }
}
