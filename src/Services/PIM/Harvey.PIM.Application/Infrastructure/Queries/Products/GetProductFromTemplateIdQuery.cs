using System;
using System.Linq;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.Infrastructure.Queries.Products
{
    public sealed class GetProductFromTemplateIdQuery : IQuery<ProductModel>
    {
        public readonly Guid FieldTemplateId;
        public GetProductFromTemplateIdQuery(Guid fieldTemplateId)
        {
            FieldTemplateId = fieldTemplateId;
        }
    }

    public sealed class GetProductFromTemplateIdQueryHandler : IQueryHandler<GetProductFromTemplateIdQuery, ProductModel>
    {
        private readonly PimDbContext _pimDbContext;
        private readonly IEntityRefService _entityRefService;
        public GetProductFromTemplateIdQueryHandler(
            PimDbContext pimDbContext,
            IEntityRefService entityRefService)
        {
            _pimDbContext = pimDbContext;
            _entityRefService = entityRefService;
        }
        public async Task<ProductModel> Handle(GetProductFromTemplateIdQuery query)
        {
            var fields = await _pimDbContext
                .Field_FieldTemplates
                .Include(x => x.Field)
                .Where(x => x.FieldTemplateId == query.FieldTemplateId).ToListAsync();

            var result = new ProductModel
            {
                Sections = fields
                .Where(x => !x.IsVariantField)
                .GroupBy(x => new
                {
                    x.Section,
                    x.IsVariantField,
                    x.OrderSection
                }, (key, g) => new ProductModel.Section()
                {
                    IsVariantField = key.IsVariantField,
                    Name = key.Section,
                    OrderSection = key.OrderSection,
                    FieldValues = g.Select(x => FieldValueFactory.GetFromFieldValue(FieldValue.Default(x.Field), _entityRefService)).ToList()
                }).ToList()
            };

            result.Variants.Add(fields
               .Where(x => x.IsVariantField)
                .GroupBy(x => new
                {
                    x.Section,
                    x.IsVariantField,
                    x.OrderSection
                }, (key, g) => new ProductModel.VariantModel()
                {
                    IsVariantField = key.IsVariantField,
                    Name = key.Section,
                    OrderSection = key.OrderSection,
                    FieldValues = g.Select(x => FieldValueFactory.GetFromFieldValue(FieldValue.Default(x.Field), _entityRefService)).ToList()
                }).FirstOrDefault());

            return result;


        }
    }
}
