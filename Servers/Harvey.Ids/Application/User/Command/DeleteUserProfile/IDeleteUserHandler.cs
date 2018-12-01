using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Command.DeleteUserProfile
{
    public interface IDeleteUserHandler
    {
        Task ExecuteAsync(string userId);
    }
}
