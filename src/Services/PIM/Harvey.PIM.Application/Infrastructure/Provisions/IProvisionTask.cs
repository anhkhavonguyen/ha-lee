using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Provisions
{
    public interface IProvisionTask<TOption>
        where TOption : ITaskOption
    {
        Task<bool> ExecuteAsync(TOption options);
        Task Rollback();
    }

    public interface ITaskOption
    {
    }
}
