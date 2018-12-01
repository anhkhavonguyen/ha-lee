using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Requests
{
    public class BaseResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
    }
}
