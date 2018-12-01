using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.Staffs.Queries
{
    public class GetStaffsResponse : BaseResponse
    {
        public List<StaffModel> StaffModels { get; set; }
    }

    public class GetStaffsByOutletResponse : BaseResponse
    {
        public List<StaffModel> StaffModels { get; set; }
    }
}
