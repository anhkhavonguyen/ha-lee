using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler
{
    public interface IChangePasswordCommandHandler
    {
        Task<IdentityResult> ExecuteAsync(string userId, ChangePasswordCommand changePasswordCommand);
    }
}
