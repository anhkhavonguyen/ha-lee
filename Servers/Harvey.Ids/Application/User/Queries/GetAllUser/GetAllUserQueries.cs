using AutoMapper;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Queries.GetAllUser
{
    public class GetAllUserQueries: IGetAllUserQueries
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetAllUserQueries(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public List<GetAllUserModel> Excecute(){
            var Users = _userManager.Users.OrderByDescending(x => x.CreatedDate).ToList();
            
            return Mapper.Map<List<GetAllUserModel>>(Users);
        }
    }
}
