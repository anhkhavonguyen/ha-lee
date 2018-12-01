using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerCodeCommandHandler
{
    public class UpdateCustomerCodeCommand
    {
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
