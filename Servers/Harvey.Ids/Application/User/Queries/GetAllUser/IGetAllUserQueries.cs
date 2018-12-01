using Harvey.Ids.Domains;
using System;
using System.Collections.Generic;

namespace Harvey.Ids.Application.User.Queries.GetAllUser
{
    public interface IGetAllUserQueries
    {
        List<GetAllUserModel> Excecute();
    }
}
