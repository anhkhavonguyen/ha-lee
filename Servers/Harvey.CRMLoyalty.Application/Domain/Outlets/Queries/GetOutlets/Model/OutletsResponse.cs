using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Queries
{
    public class GetOutletsResponse : BaseResponse
    {
        public List<OutletModel> OutletModels { get; set; }
    }
}
