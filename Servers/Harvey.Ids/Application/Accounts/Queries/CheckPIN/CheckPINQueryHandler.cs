using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.CheckPIN
{
    internal class CheckPINQueryHandler : ICheckPINQueryHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckPINQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CheckPIN> ExecuteAsync(string userId, string PIN)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new CheckPIN
            {
                IsValidPIN = user.Pin == PIN
            };
        }
    }
}
