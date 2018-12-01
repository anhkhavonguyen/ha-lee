﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePhoneNumberCommandHandler
{
    public class ChangePhoneNumberCommand
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string UpdatedBy { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewPhoneCountryCode { get; set; }
        public string MemberOriginalUrl { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
