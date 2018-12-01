using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePINCommandHandler
{
    internal class ChangePINCommandHandler : IChangePINCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePINCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ExecuteAsync(ChangePINCommand changePINCommand)
        {
            var user = await _userManager.FindByIdAsync(changePINCommand.UserId);
            if(user.Pin != changePINCommand.OldPIN)
            {
                throw new Exception("Old PIN is incorrect");
            }
            if (!user.IsActive)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            user.Pin = changePINCommand.NewPIN;
            await _userManager.UpdateAsync(user);
        }
    }
}
