using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Interface
{
    public interface IFieldService
    {
        Task<Guid> SaveAsync(Field field);
        Task SaveAsync(IList<Field> fields);
        Task<FieldType> GetFieldTypeAsync(Guid id);
        Task<string> ParseData();
    }
}
