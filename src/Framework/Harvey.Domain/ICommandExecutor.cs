using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface ICommandExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
    }
}
