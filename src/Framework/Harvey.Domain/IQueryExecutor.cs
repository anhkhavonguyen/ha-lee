using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}
