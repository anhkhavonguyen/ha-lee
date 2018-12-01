using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductSerializer : IFeedSerializer<CatalogProductFeed>
    {
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;

        public ChannelProductSerializer(IEfRepository<TransientPimDbContext, Channel> efRepository)
        {
            _efRepository = efRepository;
        }

        public async Task SerializeAsync(IEnumerable<CatalogProductFeed> feedItems)
        {
            var channel = await _efRepository.GetByIdAsync(feedItems.First().CorrelationId);
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                foreach (var item in feedItems)
                {
                    var entity = dbContext.Products.FirstOrDefault(x => x.Id == item.Id);
                    if (entity == null)
                    {
                        entity = new CatalogProduct()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        };
                        await dbContext.Products.AddAsync(item);
                    }
                    else
                    {
                        dbContext.Entry(entity).State = EntityState.Modified;
                        entity.Name = item.Name;
                        entity.Description = item.Description;
                    }

                    foreach (var fv in item.FieldValues)
                    {
                        var en = dbContext.FieldValues.FirstOrDefault(x => x.Id == fv.Id);
                        if (en == null)
                        {
                            en = new CatalogFieldValue
                            {
                                EntityId = fv.EntityId,
                                FieldId = fv.FieldId,
                                FieldName = fv.FieldName,
                                FieldType = fv.FieldType,
                                FieldValue = fv.FieldValue,
                                FieldValueId = fv.FieldValueId,
                                Id = fv.Id,
                                IsVariantField = fv.IsVariantField,
                                OrderSection = fv.OrderSection,
                                Section = fv.Section
                            };
                            await dbContext.FieldValues.AddAsync(en);
                        }
                        else
                        {
                            dbContext.Entry(en).State = EntityState.Modified;
                            en.EntityId = fv.EntityId;
                            en.FieldId = fv.FieldId;
                            en.FieldName = fv.FieldName;
                            en.FieldType = fv.FieldType;
                            en.FieldValue = fv.FieldValue;
                            en.FieldValueId = fv.FieldValueId;
                            en.Id = fv.Id;
                            en.IsVariantField = fv.IsVariantField;
                            en.OrderSection = fv.OrderSection;
                            en.Section = fv.Section;
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            };
        }
    }
}

