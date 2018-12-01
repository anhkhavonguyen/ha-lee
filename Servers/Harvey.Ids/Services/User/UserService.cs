using Harvey.Ids.Domains;
using Harvey.Ids.Services.User.Commands;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Account.User
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            HarveyIdsDbContext harveyIdsDbContext
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _harveyIdsDbContext = harveyIdsDbContext;
        }

        public async Task<ApplicationUser> AddToRolesAsync(AddToRolesCommand command)
        {
            var user = await _userManager.FindByIdAsync(command.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException("Entity Not Found");
            }

            var newRoleIds = command.RoleIds;
            if (newRoleIds == null && !newRoleIds.Any())
            {
                throw new EntityNotFoundException("Roles Not Found");
            }

            var newRoleNames = _roleManager.Roles.Where(x => newRoleIds.Contains(x.Id)).Select(x => x.Name).ToList();
            if (newRoleNames == null && !newRoleNames.Any())
            {
                throw new EntityNotFoundException("Roles Not Found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRolesAsync(user, newRoleNames);
            if (result.Succeeded)
            {
                return user;
            }

            return null;
        }

        public async Task<List<ApplicationRole>> GetRolesAsync(string UserId = null)
        {
            if (UserId == null)
            {
                return _roleManager.Roles.ToList();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new List<ApplicationRole>();
            }

            var roleNames = await _userManager.GetRolesAsync(user);
            return _roleManager.Roles.Where(x => roleNames.Contains(x.Name)).Distinct().ToList();
        }
    }
}
