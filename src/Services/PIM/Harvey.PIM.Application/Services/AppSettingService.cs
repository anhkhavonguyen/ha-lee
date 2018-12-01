using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.Setting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Services
{
    public class AppSettingService : SettingServiceBase<PimDbContext, AppSetting>, IAppSettingService
    {
        private readonly PimDbContext _pimDbContext;
        public AppSettingService(PimDbContext pimDbContext) : base(pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }

        public async Task<Guid> AddAsync(AppSettingModel appSetting)
        {
            Expression<Func<AppSetting, bool>> predicate = a => a.Key == appSetting.Key;
            var item = await FindBy(predicate).FirstOrDefaultAsync();
            if (item != null)
            {
                return Guid.Empty;
            }
            var entity = new AppSetting
            {
                Id = Guid.NewGuid(),
                Key = appSetting.Key,
                Value = appSetting.Value
            };

            await base.AddAsync(entity);
            await base.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(string id)
        {
            Expression<Func<AppSetting, bool>> predicate = a => a.Id == new Guid(id);
            var item = await FindBy(predicate).FirstOrDefaultAsync();
            base.Delete(item);
            await base.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppSettingModel>> GetAsync()
        {
            var result = await base.GetAll().Select(a => new AppSettingModel
            {
                Id = a.Id.ToString(),
                Key = a.Key,
                Value = a.Value
            }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AppSettingModel>> GetByKeyAsync(string key)
        {
            Expression<Func<AppSetting, bool>> predicate = a => a.Key == key;
            var items = await base.FindBy(predicate).Select(a => new AppSettingModel
            {
                Id = a.Id.ToString(),
                Key = a.Key,
                Value = a.Value
            }).ToListAsync();
            return items;
        }

        public async Task<Guid> UpdateAsync(AppSettingModel appSetting)
        {
            Expression<Func<AppSetting, bool>> predicate = a => a.Key == appSetting.Key;
            var item = await base.FindBy(predicate).FirstOrDefaultAsync();
            if (item == null)
                return Guid.Empty;
            item.Value = appSetting.Value;
            await base.SaveChangesAsync();
            return item.Id;
        }
    }
}
