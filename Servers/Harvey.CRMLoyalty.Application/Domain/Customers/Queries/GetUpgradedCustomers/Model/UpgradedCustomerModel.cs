using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers.Model
{
    public class UpgradedCustomerModel : CustomerModel
    {
        public DateTime? UpgradedDate { get; set; }
    }
}
