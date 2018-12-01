using AutoMapper;
using Harvey.Ids.Application.User.Queries.GetAllUser;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Queries.GetUser
{
    public class GetUserQueries : IGetUserQueries
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserQueries(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetAllUserModel> ExecuteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return Mapper.Map<GetAllUserModel>(user); 
        }
    }
}
