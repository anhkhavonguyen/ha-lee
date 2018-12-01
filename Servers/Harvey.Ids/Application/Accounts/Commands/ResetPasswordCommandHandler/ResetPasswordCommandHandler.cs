using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ResetPasswordCommandHandler
{
    internal class ResetPasswordCommandHandler : IResetPasswordCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, HarveyIdsDbContext harveyIdsDbContext)
        {
            _userManager = userManager;
            _harveyIdsDbContext = harveyIdsDbContext;
        }

        public async Task ExecuteAsync(string code, string password)
        {
            var resetPasswordCode = await _harveyIdsDbContext.ShortLinks.FirstOrDefaultAsync(f => f.Id == code);
            if(resetPasswordCode == null)
            {
                throw new EntityNotFoundException("Entity Not Found");
            }
            ForgotPasswordDeserializeModel forgotPasswordDeserializeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ForgotPasswordDeserializeModel>(resetPasswordCode.Value);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == forgotPasswordDeserializeModel.UserId && x.IsActive);
            if(user == null)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            var result = await _userManager.ResetPasswordAsync(user, forgotPasswordDeserializeModel.Code, password);
            if(!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors.Select(s => s.Description)));
            }
            if (user.IsMigrateData)
            {
                user.IsMigrateData = false;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
