

using AutoMapper;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.UpdateUserProfile
{
    public class UpdateUserProfileHandler: IUpdateUserProfileHandler
    {
        private readonly HarveyIdsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateUserProfileHandler(HarveyIdsDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<string> ExecuteAsync(UpdateUserProfile User, string currentUserId)
        {
            var usersEmailExists = _userManager.Users.Where(x => x.Email == User.Email && x.Id != User.Id).ToList();
            if (usersEmailExists.Any())
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(User.Id);
            user.FirstName = User.FirstName;
            user.LastName = User.LastName;
            user.DateOfBirth = User.DateOfBirth;
            user.PhoneNumber = User.PhoneNumber;
            user.Email = User.Email;
            user.Gender = User.Gender;
            user.IsActive = User.IsActive;
            user.UpdatedBy = currentUserId;
            user.UpdatedDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();
            return User.Id;
        }
    }

}
