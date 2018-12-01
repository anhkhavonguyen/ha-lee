using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
