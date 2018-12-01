using AutoMapper;
using Harvey.Ids.Domains;
using Harvey.Ids.Services.Account;
using Harvey.Ids.Services.Account.User;
using Harvey.Ids.Services.User.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.CreateUserProfile
{
    public class CreateUserProfileHandler: ICreateUserProfileHandler
    {
        private readonly HarveyIdsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CreateUserProfileHandler(HarveyIdsDbContext dbContext, UserManager<ApplicationUser> userManager, IAccountService accountService, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _accountService = accountService;
            _roleManager = roleManager;
        }

        public async Task<Tuple<IdentityResult, ApplicationUser>> Execute(CreateUserProfile command, string currentUserId)
        {
            ApplicationUser user = new ApplicationUser();
            user = Mapper.Map<ApplicationUser>(command);                  
            var userByEmail = await _userManager.FindByEmailAsync(command.Email);
            if (userByEmail == null)
            {
                user.CreatedDate = DateTime.UtcNow;
                user.UserName = command.Email;
                user.NormalizedUserName = command.Email.ToUpper();
                user.NormalizedEmail = command.Email.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString("D");
                user.CreatedDate = DateTime.UtcNow;
                user.CreatedBy = currentUserId;
                user.IsActive =  true;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;

                var result = await _userManager.CreateAsync(user, command.Password);
                if (result.Succeeded)
                {
                    var addToRolesCommand = new AddToRolesCommand
                    {
                        UserId = user.Id,
                        RoleIds = command.RoleIds
                    };
                    var newRoleNames = _roleManager.Roles.Where(x => command.RoleIds.Contains(x.Id)).Select(x => x.Name).ToList();
                    await _userManager.AddToRolesAsync(user, newRoleNames);
                }

                return new Tuple<IdentityResult, ApplicationUser>(result, user);
            }
            else
            {
                return null;
            }
        }
    }
}
