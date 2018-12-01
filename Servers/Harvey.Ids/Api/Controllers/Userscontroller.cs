using Harvey.Ids.Application.User.Queries.GetAllUser;
using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IGetAllUserQueries _getAllUser;

        public UsersController(IGetAllUserQueries getAllUser)
        {
            _getAllUser = getAllUser;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<GetAllUserModel> GetAll()
        {
            var users = _getAllUser.Excecute();
            return users;
        }
    }
}
