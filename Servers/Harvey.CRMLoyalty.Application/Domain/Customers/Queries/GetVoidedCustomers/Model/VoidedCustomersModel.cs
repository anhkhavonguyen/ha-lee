﻿using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class VoidedCustomersModel : CustomerModel
    {
        public DateTime? VoidedDate { get; set; }
    }
}
  