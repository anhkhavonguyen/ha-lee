using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers.Model
{
    public class RenewedCustomersModel : CustomerModel
    {
        public DateTime? RenewedDate { get; set; }
    }
}
