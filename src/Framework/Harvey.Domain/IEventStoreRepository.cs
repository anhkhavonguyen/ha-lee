using System;
using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface IEventStoreRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        Task<TAggregateRoot> GetByIdAsync(Guid id);
        Task SaveAsync(TAggregateRoot aggregate);
    }
}
