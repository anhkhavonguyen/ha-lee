using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.FieldFramework.Services.Implementation
{
    public class FieldTemplateService : IFieldTemplateService
    {
        private readonly PimDbContext _pimDbContext;
        private readonly IMapper _mapper;
        private readonly IEfRepository<PimDbContext, FieldTemplate> _repository;
        public FieldTemplateService(PimDbContext pimDbContext, IMapper mapper, IEfRepository<PimDbContext, FieldTemplate> repository)
        {
            _pimDbContext = pimDbContext;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<PagedResult<FieldTemplate>> GetAllAsync(PagingFilterCriteria query)
        {
            var result = await _repository.GetAsync(query.Page, query.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<FieldTemplate>()
            {
                CurrentPage = query.Page,
                NumberItemsPerPage = query.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }

        public async Task<FieldTemplateModel> GetAsync(Guid id)
        {
            var fieldTemplate = await _pimDbContext
                .FieldTemplates
                .Include(x => x.Field_FieldTemplates)
                .ThenInclude(x => x.Field)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (fieldTemplate == null)
            {
                return null;
            }
            return _mapper.Map<FieldTemplate, FieldTemplateModel>(fieldTemplate);
        }

        public async Task<FieldTemplateModel> SaveAsync(FieldTemplateModel fieldTemplateModel)
        {
            var fieldTemplate = _mapper.Map<FieldTemplateModel, FieldTemplate>(fieldTemplateModel);
            if (fieldTemplate.Id == Guid.Empty)
            {
                _pimDbContext.Add(fieldTemplate);
                fieldTemplateModel.Id = fieldTemplate.Id;
            }
            else
            {
                _pimDbContext.Update(fieldTemplate);
            }

            var fields = fieldTemplateModel.Sections.SelectMany(x => x.Fields);

            var fieldFieldTemplates = _mapper.Map<FieldTemplateModel, List<Field_FieldTemplate>>(fieldTemplateModel);
            fieldFieldTemplates.ForEach(x =>
            {
                if (x.Id == Guid.Empty)
                {
                    _pimDbContext.Add(x);
                }
                else
                {
                    var field = fields.SingleOrDefault(f => f.Id == x.Id);
                    if (field != null)
                    {
                        switch (field.Action)
                        {
                            case ItemActionType.Update:
                                _pimDbContext.Update(x);
                                break;
                            case ItemActionType.Delete:
                                _pimDbContext.Remove(x);
                                break;
                        }
                    }
                }
            });
            await _pimDbContext.SaveChangesAsync();
            return await GetAsync(fieldTemplate.Id);
        }

        public async Task Delete(Guid id)
        {
            var fieldTemplate = _pimDbContext.FieldTemplates.Where(x => x.Id == id).SingleOrDefault();
            if (fieldTemplate == null)
                return;
            var field_fieldTemplate = _pimDbContext.Field_FieldTemplates.Where(x => x.FieldTemplateId == id).ToList();
            _pimDbContext.Field_FieldTemplates.RemoveRange(field_fieldTemplate);
            _pimDbContext.FieldTemplates.Remove(fieldTemplate);
            await _pimDbContext.SaveChangesAsync();
        }
    }
}
