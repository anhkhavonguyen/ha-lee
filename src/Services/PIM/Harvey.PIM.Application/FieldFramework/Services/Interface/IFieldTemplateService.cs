using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.FieldFramework.Services.Interface
{
    public interface IFieldTemplateService
    {
        Task<FieldTemplateModel> SaveAsync(FieldTemplateModel fieldTemplateModel);
        Task<FieldTemplateModel> GetAsync(Guid id);
        Task<PagedResult<FieldTemplate>> GetAllAsync(PagingFilterCriteria pagingFilterCriteria);
        Task Delete(Guid id);
    }
}
