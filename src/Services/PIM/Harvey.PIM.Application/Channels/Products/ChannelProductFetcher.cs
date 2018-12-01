using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductFetcher : IFeedFetcher<ProductFeed>
    {
        private readonly TransientPimDbContext _pimDbContext;
        public ChannelProductFetcher(TransientPimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }
        public async Task<IEnumerable<ProductFeed>> FetchAsync()
        {
            var result = new List<ProductFeed>();
            var products = await _pimDbContext.Products.AsNoTracking().ToListAsync();
            var productIds = products.Select(x => x.Id);
            var productFieldTemplateIds = products.Select(x => x.FieldTemplateId);

            var variants = _pimDbContext
                                  .Variants
                                  .AsNoTracking()
                                  .Where(x => productIds.Contains(x.ProductId))
                                  .ToList();

            var allVariantIds = variants.Select(x => x.Id);

            var fieldValues = _pimDbContext
                                .FieldValues
                                .Include(x => x.Field)
                                .AsNoTracking()
                                .Where(x => productIds.Contains(x.EntityId) || allVariantIds.Contains(x.EntityId)).ToList();
            var fieldTempates = _pimDbContext
                                    .Field_FieldTemplates
                                    .Include(x => x.Field)
                                    .AsNoTracking()
                                    .Where(x => productFieldTemplateIds.Contains(x.FieldTemplateId));

            foreach (var item in products)
            {
                var feed = new ProductFeed()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                };

                var variantIds = variants.Where(x => x.ProductId == item.Id).Select(x => x.Id).ToList();
                foreach (var fv in fieldValues.Where(x => variantIds.Contains(x.EntityId) || x.EntityId == item.Id))
                {
                    var ft = fieldTempates.Single(x => x.FieldTemplateId == item.FieldTemplateId && x.FieldId == fv.FieldId);
                    var fvf = new CatalogFieldValue()
                    {
                        EntityId = fv.EntityId,
                        FieldId = fv.FieldId,
                        FieldName = fv.Field.Name,
                        FieldType = fv.Field.Type,
                        FieldValueId = fv.Id,
                        IsVariantField = ft.IsVariantField,
                        Id = fv.Id,
                        Section = ft.Section,
                        OrderSection = ft.OrderSection
                    };
                    fvf.FieldValue = FieldValueFactory.GetFieldValueFromFieldType(fvf.FieldType, fv);
                    feed.FieldValues.Add(fvf);
                }
                result.Add(feed);
            }
            return result;
        }
    }
}
