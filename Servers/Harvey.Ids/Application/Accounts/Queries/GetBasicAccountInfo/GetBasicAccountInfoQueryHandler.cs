using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.GetBasicAccountInfo
{
    internal class GetBasicAccountInfoQueryHandler : IGetBasicAccountInfoQueryHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetBasicAccountInfoQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetBasicAccountInfo> ExecuteAsync(string countryCode, string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(f=>f.PhoneNumber == phoneNumber && f.PhoneCountryCode == countryCode && f.IsActive);
            if(user == null)
            {
                return null;
            }
            return new GetBasicAccountInfo
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                IsMigrateData = user.IsMigrateData,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PasswordHash = user.PasswordHash
            }; 
        }
    }
}
