using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.DeleteUserProfile
{
    public class DeleteUserHandler: IDeleteUserHandler
    {
        private readonly HarveyIdsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteUserHandler(HarveyIdsDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
