using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
}
