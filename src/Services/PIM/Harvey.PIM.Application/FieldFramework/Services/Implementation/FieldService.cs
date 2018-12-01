using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Implementation
{
    public class FieldService : IFieldService
    {
        private readonly PimDbContext _pimDbContext;

        public FieldService(PimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }

        public async Task<FieldType> GetFieldTypeAsync(Guid id)
        {
            var result = await _pimDbContext.Fields.FirstOrDefaultAsync(f => f.Id == id);
            return result.Type;
        }

        public Task<string> ParseData()
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> SaveAsync(Field field)
        {
            field.Id = Guid.NewGuid();
            await _pimDbContext.Fields.AddAsync(field);
            await _pimDbContext.SaveChangesAsync();
            return field.Id;
        }

        public async Task SaveAsync(IList<Field> fields)
        {
            foreach (var item in fields)
            {
                item.Id = Guid.NewGuid();
            }

            await _pimDbContext.Fields.AddRangeAsync(fields);
            await _pimDbContext.SaveChangesAsync();
        }
    }
}
