using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointBalance
{
    public interface IGetPointBalance
    {
        Task<decimal> ExecuteAsync(string customerId);
    }
}
