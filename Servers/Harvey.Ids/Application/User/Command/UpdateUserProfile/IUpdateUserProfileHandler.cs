using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.UpdateUserProfile
{
    public interface IUpdateUserProfileHandler
    {
        Task<string> ExecuteAsync(UpdateUserProfile User, string currentUserId);
    }
}
