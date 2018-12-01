using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler
{
    internal class ChangePasswordCommandHandler : IChangePasswordCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ExecuteAsync(string userId, ChangePasswordCommand changePasswordCommand)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id== userId && f.IsActive);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordCommand.CurrentPassword, changePasswordCommand.NewPassword);

            return result;
        }
    }
}
