using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.Setting
{
    public interface IAppSettingService
    {
        Task<IEnumerable<AppSettingModel>> GetAsync();
        Task<IEnumerable<AppSettingModel>> GetByKeyAsync(string key);
        Task<Guid> AddAsync(AppSettingModel appSetting);
        Task<Guid> UpdateAsync(AppSettingModel appSetting);
        Task Delete(string id);
    }
}
