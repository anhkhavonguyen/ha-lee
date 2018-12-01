using Harvey.Ids.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Account.Commands
{
    public class ForgotPasswordViaEmailCommand
    {
        public ApplicationUser User { get; set; }
        public string AcronymBrandName { get; set; }
        public string BrandName { get; set; }
    }
}
