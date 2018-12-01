using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers.Model
{
    public class GetExtendedCutomersResponse : BaseResponse
    {
        public List<ExtendedCustomersModel> CustomerListResponse { get; set; }
    }
}
