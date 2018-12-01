using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Harvey.Exception
{
    public class ProblemDetails
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
    }
}
