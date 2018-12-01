using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers.Model
{
    public class ExtendedCustomersModel : CustomerModel
    {
        public DateTime? ExtendedDate { get; set; }
    }
}
