using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Setting
{
    public abstract class SettingServiceBase<C, T>
        where C : DbContext
        where T : class 
    {
        public C _dbContext;
        public C Context
        {
            get { return _dbContext; }
            set { _dbContext = value; }
        }

        public SettingServiceBase(DbContext dbContext)
        {
            _dbContext = (C)dbContext;
        }

        public async virtual Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async virtual Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
            return query;
        }
    }
}
