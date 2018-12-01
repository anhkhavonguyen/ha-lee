using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{ 
    public class ExpiredCustomersModel : CustomerModel
    {
        public DateTime? ExpiredDate { get; set; }
    }
}
