using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.CheckSignUpLink
{
    public class CheckSignUpLinkHandler : ICheckSignUpLink
    {
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckSignUpLinkHandler(HarveyIdsDbContext harveyIdsDbContext, UserManager<ApplicationUser> userManager)
        {
            _harveyIdsDbContext = harveyIdsDbContext;
            _userManager = userManager;
        }

        public async Task<CheckSignUpLink> ExecuteAsync(string code)
        {
            var shortLink = _harveyIdsDbContext.ShortLinks.FirstOrDefault(f => f.Id == code);
            if (shortLink == null)
            {
                return new CheckSignUpLink()
                {
                    IsValidLink = false
                };
            }

            CheckSignUpLinkDeserializeModel checkSignUpLinkDeserializeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckSignUpLinkDeserializeModel>(shortLink.Value);

            var user = await _userManager.FindByIdAsync(checkSignUpLinkDeserializeModel.UserId);

            if (user.EmailConfirmed || user.PhoneNumberConfirmed)
            {
                return new CheckSignUpLink()
                {
                    IsValidLink = false
                };
            }

            return new CheckSignUpLink()
            {
                IsValidLink = true
            };
        }
    }
}
