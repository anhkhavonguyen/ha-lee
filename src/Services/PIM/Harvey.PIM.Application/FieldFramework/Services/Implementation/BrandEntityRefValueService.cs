using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.FieldFramework.Services.Implementation
{
    public class BrandEntityRefValueService : IEntityRefValueService<Brand>
    {
        private readonly PimDbContext _pimDbContext;
        public BrandEntityRefValueService(PimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }
        public async Task<IEnumerable<EntityRefValueModel>> GetAll()
        {
            return await _pimDbContext.Brands.Select(x => new EntityRefValueModel(x.Id, x.Name)).ToListAsync();
        }
    }
}
