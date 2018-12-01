using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Interface
{
    public interface IEntityRefService
    {
        Task<IList<EntityRefModel>> GetAsync();

        Task<IEnumerable<EntityRefValueModel>> GetValuesAsync(Guid id);
    }
}
