using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.CheckSignUpLink
{
    public interface ICheckSignUpLink
    {
        Task<CheckSignUpLink> ExecuteAsync(string code);
    }
}
