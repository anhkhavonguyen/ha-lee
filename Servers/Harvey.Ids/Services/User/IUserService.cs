using Harvey.Ids.Domains;
using Harvey.Ids.Services.User.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Account.User
{
    public interface IUserService
    {
        Task<ApplicationUser> AddToRolesAsync(AddToRolesCommand command);
        Task<List<ApplicationRole>> GetRolesAsync(string UserId = null);
    }
}
