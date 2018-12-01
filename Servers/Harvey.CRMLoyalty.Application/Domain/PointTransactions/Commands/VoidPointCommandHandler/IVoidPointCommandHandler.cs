using Harvey.CRMLoyalty.Application.Models;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.VoidPointCommandHandler
{
    public interface IVoidPointCommandHandler
    {
        Task<decimal> ExecuteAsync(VoidPointCommand command);
    }
}
