using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Variants
{
    public class ChannelVariantSerializer : IFeedSerializer<CatalogVariant>
    {
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;

        public ChannelVariantSerializer(IEfRepository<TransientPimDbContext, Channel> efRepository)
        {
            _efRepository = efRepository;
        }
        public async Task SerializeAsync(IEnumerable<CatalogVariant> feedItems)
        {
            var channel = await _efRepository.GetByIdAsync(feedItems.First().CorrelationId);
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                foreach (var item in feedItems)
                {
                    var entity = dbContext.Variants.FirstOrDefault(x => x.Id == item.Id);
                    if (entity == null)
                    {
                        entity = new CatalogVariant()
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            PriceId = item.PriceId
                        };
                        dbContext.Variants.Add(item);
                    }
                    else
                    {
                        dbContext.Entry(entity).State = EntityState.Modified;
                        entity.ProductId = item.ProductId;
                        entity.PriceId = item.PriceId;
                    }

                }
                await dbContext.SaveChangesAsync();
            };
        }
    }
}
