using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Interface
{
    public interface IFieldValueService
    {
        Task<Guid> SaveAsync(string entityId, string fieldId, FieldValue fieldValue);
        Task SaveAsync(string enityId, IList<FieldData> fieldDatas);
    }
}
