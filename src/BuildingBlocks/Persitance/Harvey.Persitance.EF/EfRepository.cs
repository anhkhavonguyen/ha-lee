using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Harvey.Domain;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Persitance.EF
{
    public class EfRepository<TContext, TEntity> : IEfRepository<TContext, TEntity>, IUnitOfWork
        where TContext : DbContext
        where TEntity : EntityBase
    {
        protected readonly TContext DbContext;
        public EfRepository(TContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity is IAuditable)
            {
                ((IAuditable)entity).CreatedDate = DateTime.Now;
                ((IAuditable)entity).UpdatedDate = DateTime.Now;
            }
            await DbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddAsync(List<TEntity> entities)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public Task<int> Count(Expression<Func<TEntity, bool>> whereCondition = null)
        {
            if (whereCondition == null)
            {
                return DbContext.Set<TEntity>().CountAsync();
            }
            else
            {
                return DbContext.Set<TEntity>().CountAsync(whereCondition);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Yield();
            DbContext.Set<TEntity>().Remove(entity);

        }

        public async Task DeleteAsync(List<TEntity> entities)
        {
            await Task.Yield();
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(int page = 0, int numberItemsPerPage = 0)
        {
            var result = DbContext.Set<TEntity>();
            if (page == 0 && numberItemsPerPage == 0)
            {
                return await result.ToListAsync();
            }
            return await result
                .Skip((page - 1) * numberItemsPerPage)
                .Take(numberItemsPerPage)
                .ToListAsync();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> whereCondition, int page = 0, int numberItemsPerPage = 0)
        {
            var result = DbContext.Set<TEntity>().AsQueryable().Where(whereCondition);
            if (page == 0 && numberItemsPerPage == 0)
            {
                return await result.ToListAsync();
            }
            return await result
                .Skip((page - 1) * numberItemsPerPage)
                .Take(numberItemsPerPage)
                .ToListAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            if (entity is IAuditable)
            {
                ((IAuditable)entity).UpdatedDate = DateTime.Now;
            }
            return DbContext.SaveChangesAsync();
        }
    }

    public class EfRepository<TContext, TEntity, TModel> : IEfRepository<TContext, TEntity, TModel>, IUnitOfWork
        where TContext : DbContext
        where TEntity : EntityBase
        where TModel : class
    {
        protected readonly TContext DbContext;
        private readonly IMapper _mapper;
        public EfRepository(TContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TModel> AddAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            if (entity is IAuditable)
            {
                ((IAuditable)entity).CreatedDate = DateTime.Now;
                ((IAuditable)entity).UpdatedDate = DateTime.Now;
            }
            await DbContext.Set<TEntity>().AddAsync(entity);
            return _mapper.Map<TModel>(entity);

        }

        public async Task<IEnumerable<TModel>> AddAsync(List<TModel> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
            return _mapper.Map<List<TModel>>(entities);
        }

        public Task<int> Count(Expression<Func<TEntity, bool>> whereCondition = null)
        {
            if (whereCondition == null)
            {
                return DbContext.Set<TEntity>().CountAsync();
            }
            else
            {
                return DbContext.Set<TEntity>().CountAsync(whereCondition);
            }
        }

        public async Task DeleteAsync(TModel model)
        {
            await Task.Yield();
            var entity = _mapper.Map<TEntity>(model);
            DbContext.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteAsync(List<TModel> models)
        {
            await Task.Yield();
            var entities = _mapper.Map<List<TEntity>>(models);
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TModel>> GetAsync(int page = 0, int numberItemsPerPage = 0)
        {
            var result = DbContext.Set<TEntity>();
            List<TEntity> entities = null;
            if (page == 0 && numberItemsPerPage == 0)
            {
                entities = await result.ToListAsync();
            }
            else
            {
                entities = await result
                    .Skip((page - 1) * numberItemsPerPage)
                    .Take(numberItemsPerPage)
                    .ToListAsync();
            }
            return _mapper.Map<IEnumerable<TModel>>(entities);
        }

        public async Task<TModel> GetByIdAsync(Guid id)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            return _mapper.Map<TModel>(entity);
        }

        public async Task<IEnumerable<TModel>> ListAsync(Expression<Func<TModel, bool>> whereCondition, int page = 0, int numberItemsPerPage = 0)
        {
            Expression<Func<TEntity, bool>> condition = ConvertExpressionType<TEntity, TModel, bool>(whereCondition);
            List<TEntity> entities = null;
            var result = DbContext.Set<TEntity>().AsQueryable().Where(condition);
            if (page == 0 && numberItemsPerPage == 0)
            {
                entities = await result.ToListAsync();
            }
            else
            {
                entities = await result
                .Skip((page - 1) * numberItemsPerPage)
                .Take(numberItemsPerPage)
                .ToListAsync();
            }

            return _mapper.Map<IEnumerable<TModel>>(entities);
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            if (entity is IAuditable)
            {
                ((IAuditable)entity).UpdatedDate = DateTime.Now;
            }
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        private Expression<Func<NewParam, TResult>> ConvertExpressionType<NewParam, OldParam, TResult>(Expression<Func<OldParam, TResult>> expression)
        {
            var param = Expression.Parameter(typeof(NewParam));
            return Expression.Lambda<Func<NewParam, TResult>>(expression.Body.Replace(expression.Parameters[0], param), param);
        }
    }
}
