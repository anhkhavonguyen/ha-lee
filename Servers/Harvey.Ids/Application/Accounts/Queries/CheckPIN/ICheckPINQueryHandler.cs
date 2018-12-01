using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Queries.CheckPIN
{
    public interface ICheckPINQueryHandler
    {
        Task<CheckPIN> ExecuteAsync(string userId, string PIN);
    }
}
