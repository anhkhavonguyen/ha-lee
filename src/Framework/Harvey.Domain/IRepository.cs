using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Harvey.Domain
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAsync(int page = 0, int numberItemsPerPage = 0);
        Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> whereCondition, int page = 0, int numberItemsPerPage = 0);
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(List<TEntity> entities);
        Task<int> Count(Expression<Func<TEntity, bool>> whereCondition = null);
    }

    public interface IRepository<TEntity, TModel>
        where TEntity : EntityBase
        where TModel : class
    {
        Task<TModel> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAsync(int page = 0, int numberItemsPerPage = 0);
        Task<IEnumerable<TModel>> ListAsync(Expression<Func<TModel, bool>> whereCondition, int page = 0, int numberItemsPerPage = 0);
        Task<TModel> AddAsync(TModel model);
        Task<IEnumerable<TModel>> AddAsync(List<TModel> models);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TModel model);
        Task DeleteAsync(List<TModel> models);
        Task<int> Count(Expression<Func<TEntity, bool>> whereCondition = null);
    }
}
