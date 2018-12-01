using Harvey.Ids.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.CreateUserProfile
{
    public interface ICreateUserProfileHandler
    {
        Task<Tuple<IdentityResult, ApplicationUser>> Execute(CreateUserProfile command, string currentUserId);
    }
}
