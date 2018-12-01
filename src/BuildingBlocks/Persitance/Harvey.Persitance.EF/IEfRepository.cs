using Harvey.Domain;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Persitance.EF
{
    public interface IEfRepository<TContext, TEntity> : IRepository<TEntity>, IUnitOfWork
        where TContext : DbContext
        where TEntity : EntityBase
    {
    }

    public interface IEfRepository<TContext, TEntity, TModel> : IRepository<TEntity, TModel>, IUnitOfWork
        where TContext : DbContext
        where TEntity : EntityBase
        where TModel : class
    {
    }
}
