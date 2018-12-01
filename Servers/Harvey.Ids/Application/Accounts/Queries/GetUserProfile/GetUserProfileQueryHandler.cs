using AutoMapper;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.GetUserProfile
{
    internal class GetUserProfileQueryHandler : IGetUserProfileQueryHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserProfileModel> ExecuteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.IsActive)
                return Mapper.Map<GetUserProfileModel>(user);
            else
                return null;
        }
    }
}
