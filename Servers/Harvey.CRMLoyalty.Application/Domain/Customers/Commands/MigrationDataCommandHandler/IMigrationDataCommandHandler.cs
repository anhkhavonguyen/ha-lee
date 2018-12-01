using Harvey.Message.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.MigrationDataCommandHandler
{
    public interface IMigrationDataCommandHandler
    {
        string Excute(IList<MigrationDataCommand> models);

        Task ExcuteAsync(IList<UpdatedApplicationUser> updatedApplicationUsers);
    }
}
