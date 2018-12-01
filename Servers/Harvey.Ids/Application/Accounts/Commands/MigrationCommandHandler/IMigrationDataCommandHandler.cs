using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.MigrationDataCommandHandler
{
    public interface IMigrationDataCommandHandler
    {
        string Execute(List<MigrationDataCommand> initMemberAccountCommand);

        Task<int> ExecuteAsync();
    }
}
