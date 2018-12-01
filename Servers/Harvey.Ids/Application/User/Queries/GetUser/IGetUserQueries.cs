using Harvey.Ids.Application.User.Queries.GetAllUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.User.Queries.GetUser
{
    public interface IGetUserQueries
    {
        Task<GetAllUserModel> ExecuteAsync(string userId);
    }
}
