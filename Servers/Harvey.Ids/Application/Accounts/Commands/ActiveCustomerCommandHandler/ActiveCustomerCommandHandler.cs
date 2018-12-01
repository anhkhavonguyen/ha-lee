using Harvey.Ids.Data;
using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ActiveCustomerCommandHandler
{
    public class ActiveCustomerCommandHandler : IActiveCustomerCommandHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public ActiveCustomerCommandHandler(HarveyIdsDbContext harveyIdsDbContext,
            UserManager<ApplicationUser> userManager)
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(ActiveCustomerCommand command)
        {
            if ((StatusUser)command.IsActive == StatusUser.Active)
            {
                var applicationUser = new ApplicationUser
                {
                    Id = command.CustomerId,
                    UserName = $"{command.PhoneCountryCode}{command.PhoneNumber}",
                    PhoneNumber = command.PhoneNumber,
                    CreatedDate = DateTime.UtcNow,
                    IsMigrateData = false,
                    PhoneCountryCode = command.PhoneCountryCode,
                    Pin = StringExtension.GeneratePIN(),
                    UserType = Data.UserType.Member,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    DateOfBirth = command.DateOfBirth,
                    IsActive = true,
                    EmailConfirmed = string.IsNullOrEmpty(command.Email) ? false : true,
                    PhoneNumberConfirmed = string.IsNullOrEmpty(command.Email) ? false : true
                };

                if (command.Gender.HasValue)
                    applicationUser.Gender = (Gender)command.Gender;
                else
                    applicationUser.Gender = null;

                IdentityResult result = await _userManager.CreateAsync(applicationUser);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Member" });
                }
            }
            else
            {
                var user = _harveyIdsDbContext.Users.FirstOrDefault(u => u.Id == command.CustomerId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    await _harveyIdsDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
