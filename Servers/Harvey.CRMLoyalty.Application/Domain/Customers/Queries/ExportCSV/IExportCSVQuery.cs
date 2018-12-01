using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public interface IExportCSVQuery
    {
        byte[] Excute();
    }
}
