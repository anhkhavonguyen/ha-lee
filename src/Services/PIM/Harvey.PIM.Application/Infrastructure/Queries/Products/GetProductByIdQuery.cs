using System;
using System.Linq;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.Infrastructure.Queries.Products
{
    public sealed class GetProductByIdQuery : IQuery<ProductModel>
    {
        public readonly Guid Id;
        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductModel>
    {
        private readonly PimDbContext _pimDbContext;
        private readonly IEventStoreRepository<Product> _repository;
        private readonly IEntityRefService _entityRefService;
        public GetProductByIdQueryHandler(
            IEventStoreRepository<Product> repository,
            IEntityRefService entityRefService,
            PimDbContext pimDbContext)
        {
            _repository = repository;
            _pimDbContext = pimDbContext;
            _entityRefService = entityRefService;
        }

        public async Task<ProductModel> Handle(GetProductByIdQuery query)
        {
            var product = await _repository.GetByIdAsync(query.Id);
            var fieldTemplate = _pimDbContext.FieldTemplates.FirstOrDefault(x => x.Id == product.FieldTemplateId);

            var result = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                FieldTemplateId = product.FieldTemplateId,
                FieldTemplateName = fieldTemplate?.Name,
                CategoryId = product.CategoryId
            };


            var fields = await _pimDbContext
               .Field_FieldTemplates
               .Include(x => x.Field)
               .Where(x => x.FieldTemplateId == product.FieldTemplateId).ToListAsync();

            foreach (var item in product.Sections.Keys)
            {
                if (!item.IsVariantField)
                {
                    var section = new ProductModel.Section
                    {
                        Name = item.Name,
                        OrderSection = item.OrderSection,
                        IsVariantField = item.IsVariantField
                    };
                    foreach (var value in product.Sections[item])
                    {
                        var field = fields.FirstOrDefault(x => x.FieldId == value.Field.Id);
                        if (field != null)
                        {
                            value.Field.DefaultValue = field.Field?.DefaultValue;
                            section.FieldValues.Add(FieldValueFactory.GetFromFieldValue(value, _entityRefService));
                        }
                    }
                    result.Sections.Add(section);
                }
            }

            foreach (var item in product.VariantFieldValues)
            {
                var variant = new ProductModel.VariantModel()
                {
                    Id = item.Key,

                };

                if (product.VariantPrices.ContainsKey(item.Key))
                {
                    variant.Price = product.VariantPrices.FirstOrDefault(x => x.Key == item.Key).Value;
                }

                foreach (var field in item.Value)
                {
                    var dbField = fields.FirstOrDefault(x => x.FieldId == field.Field.Id);
                    if (dbField != null)
                    {
                        field.Field.DefaultValue = dbField.Field?.DefaultValue;
                        variant.FieldValues.Add(FieldValueFactory.GetFromFieldValue(field, _entityRefService));
                    }
                }

                result.Variants.Add(variant);
            }

            return result;
        }
    }
}
