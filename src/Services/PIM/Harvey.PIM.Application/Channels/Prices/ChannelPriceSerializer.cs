using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Prices
{
    public class ChannelPriceSerializer : IFeedSerializer<CatalogPrice>
    {
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;

        public ChannelPriceSerializer(IEfRepository<TransientPimDbContext, Channel> efRepository)
        {
            _efRepository = efRepository;
        }
        public async Task SerializeAsync(IEnumerable<CatalogPrice> feedItems)
        {
            var channel = await _efRepository.GetByIdAsync(feedItems.First().CorrelationId);
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                foreach (var item in feedItems)
                {
                    var entity = dbContext.Prices.FirstOrDefault(x => x.Id == item.Id);
                    if (entity == null)
                    {
                        entity = new CatalogPrice()
                        {
                            Id = item.Id,
                            ListPrice = item.ListPrice,
                            MemberPrice = item.MemberPrice,
                            StaffPrice = item.StaffPrice
                        };
                        dbContext.Prices.Add(item);
                    }
                    else
                    {
                        dbContext.Entry(entity).State = EntityState.Modified;
                        entity.ListPrice = item.ListPrice;
                        entity.MemberPrice = item.MemberPrice;
                        entity.StaffPrice = item.StaffPrice;
                    }

                }
                await dbContext.SaveChangesAsync();
            };
        }
    }
}
