using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Interface
{
    public interface IEntityRefValueService<T>
        where T : EntityBase
    {
        Task<IEnumerable<EntityRefValueModel>> GetAll();
    }
}
