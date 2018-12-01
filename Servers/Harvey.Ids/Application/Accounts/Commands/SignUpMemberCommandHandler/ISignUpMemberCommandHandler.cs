using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.SignUpMemberCommandHandler
{
    public interface ISignUpMemberCommandHandler
    {
        Task ExecuteAsync(SignUpMemberCommand signUpMemberCommand);
    }
}
