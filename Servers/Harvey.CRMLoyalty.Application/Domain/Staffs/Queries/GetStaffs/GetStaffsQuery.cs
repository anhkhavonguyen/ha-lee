using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Staffs.Queries
{
    public class GetStaffsQuery : IGetStaffsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetStaffsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetStaffsResponse GetStaffs(GetStaffsRequest request)
        {
            var query = _dbContext.Staffs.AsNoTracking().Where(w=>w.Id != "76af5bea-6af1-4a18-b38e-1396875518e5").Select(s => new StaffModel
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                Gender = s.Gender,
                Id = s.Id,
                Phone = s.Phone,
                PhoneCountryCode = s.PhoneCountryCode,
                ProfileImage = s.ProfileImage,
                FullName = s.FirstName + ' ' + s.LastName,
                TypeOfStaff = s.TypeOfStaff.ToString()
            });

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                var searchString = request.SearchString.ToLower().Trim();

                query = query.Where(s => s.FirstName != null && s.FirstName.ToLower().Trim().Contains(searchString)
                                  || s.LastName != null && s.LastName.ToLower().Trim().Contains(searchString)
                                  || s.FullName != null && s.FullName.ToLower().Trim().Contains(searchString)
                                  || s.Email != null && s.Email.ToLower().Contains(searchString));
            }
            var result = PagingExtensions.GetPaged<StaffModel>(query, request.PageNumber, request.PageSize);
            var response = new GetStaffsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.StaffModels = result.Results;
            return response;
        }

        public GetStaffsByOutletResponse GetStaffsByOutlet(GetStaffsByOutletRequest request)
        {
            var query = _dbContext.Staffs.AsNoTracking().Where(s => s.Staff_Outlets.Select(a=>a.OutletId).Contains(request.OutletId));
            var result = PagingExtensions.GetPaged<Staff, StaffModel>(query, request.PageNumber, request.PageSize);
            var response = new GetStaffsByOutletResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.StaffModels = result.Results;
            return response;
        }
    }
}
