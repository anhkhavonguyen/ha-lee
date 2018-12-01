using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Extensions;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.FieldFramework.Services.Implementation
{
    public class FieldValueService : IFieldValueService
    {
        private readonly PimDbContext _pimDbContext;
        public FieldValueService(PimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }

        public async Task<Guid> SaveAsync(string entityId, string fieldId, FieldValue fieldValue)
        {
            fieldValue.Id = Guid.NewGuid();
            fieldValue.EntityId = new Guid(entityId);
            fieldValue.FieldId = new Guid(entityId);
            await _pimDbContext.FieldValues.AddAsync(fieldValue);
            await _pimDbContext.SaveChangesAsync();
            return fieldValue.Id;
        }

        public async Task SaveAsync(string enityId, IList<FieldData> fields)
        {
            var temp = new List<FieldValue>();
            foreach (var item in fields)
            {
                var fieldValue = new FieldValue();

                fieldValue.EntityId = new Guid(enityId);
                fieldValue.FieldId = item.Field.Id;

                FieldValueHandler.SetData(item.Field.Type, fieldValue, item.Value);

                temp.Add(fieldValue);
            }

            await _pimDbContext.FieldValues.AddRangeAsync(temp);
            await _pimDbContext.SaveChangesAsync();
        }
    }
}
