using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.SetPasswordForAccountCommandHandler
{
    internal class SetPasswordForAccountCommandHandler : ISetPasswordForAccountCommandHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetPasswordForAccountCommandHandler(HarveyIdsDbContext harveyIdsDbContext,
            UserManager<ApplicationUser> userManager)
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(SetPasswordForAccountCommand setPasswordForStoreAccountCommand)
        {
            var account = await _harveyIdsDbContext.Users.FirstOrDefaultAsync(f => f.UserName == setPasswordForStoreAccountCommand.Email);
            if (account == null)
            {
                throw new EntityNotFoundException("USER_NOT_FOUND");
            }
            var roles = await _userManager.GetRolesAsync(account);
            if(roles.Contains(UserType.Staff.ToString()) || roles.Contains(UserType.Admin.ToString()))
            {
                account.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(account, setPasswordForStoreAccountCommand.NewPassword);
                await _harveyIdsDbContext.SaveChangesAsync();
            } else
            {
                throw new DataInvalidException("CAN_NOT_SET_PASSWORD_FOR_THIS_USER");
            }
            
        }
    }
}
