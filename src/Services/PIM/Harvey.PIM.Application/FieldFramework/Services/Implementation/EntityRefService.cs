using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.FieldFramework.Services.Implementation
{
    public class EntityRefService : IEntityRefService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PimDbContext _pimDbContext;
        public EntityRefService(
            IServiceProvider serviceProvider,
            PimDbContext pimDbContext)
        {
            _serviceProvider = serviceProvider;
            _pimDbContext = pimDbContext;
        }


        public async Task<IList<EntityRefModel>> GetAsync()
        {
            return await _pimDbContext.EntityRefs.Select(e => new EntityRefModel
            {
                Id = e.Id,
                Name = e.Name,
                Namespace = e.Namespace
            }).ToListAsync();
        }

        public async Task<IEnumerable<EntityRefValueModel>> GetValuesAsync(Guid id)
        {
            var entityRef = await _pimDbContext.EntityRefs.FindAsync(id);
            var type = Type.GetType(entityRef.Namespace);
            if (type == null)
            {
                throw new ArgumentException($"Entity Ref {entityRef.Name} is not supported.");
            }
            var htype = typeof(IEntityRefValueService<>).MakeGenericType(type);
            dynamic service = _serviceProvider.GetService(htype);
            return await service.GetAll();
        }
    }
}
