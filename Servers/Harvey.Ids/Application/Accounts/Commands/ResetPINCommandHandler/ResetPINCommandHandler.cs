using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Harvey.Ids.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ResetPINCommandHandler
{
    internal class ResetPINCommandHandler : IResetPINCommandHandler
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPINCommandHandler(HarveyIdsDbContext harveyIdsDbContext, UserManager<ApplicationUser> userManager)
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(ResetPINCommand resetPINCommand)
        {
            var shortLink = _harveyIdsDbContext.ShortLinks.FirstOrDefault(f => f.Id == resetPINCommand.Code);
            if (shortLink == null)
                throw new EntityNotFoundException("Entity not found");

            var resetPINDeserializeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResetPINDeserializeModel>(shortLink.Value);
            if(resetPINDeserializeModel.ExpireDate > DateTime.UtcNow)
            {
                throw new DataInvalidException("The token is expired");
            }
            var user = await _userManager.FindByIdAsync(resetPINDeserializeModel.UserId);
            user.Pin = resetPINCommand.NewPIN.ToMd5String();
            await _userManager.UpdateAsync(user);
        }
    }
}
