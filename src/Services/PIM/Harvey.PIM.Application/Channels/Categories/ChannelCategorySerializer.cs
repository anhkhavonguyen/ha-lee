using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Categories
{
    public class ChannelCategorySerializer : IFeedSerializer<CatalogCategory>
    {
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        public ChannelCategorySerializer(IEfRepository<TransientPimDbContext, Channel> efRepository)
        {
            _efRepository = efRepository;
        }

        public async Task SerializeAsync(IEnumerable<CatalogCategory> feedItems)
        {
            var channel = await _efRepository.GetByIdAsync(feedItems.First().CorrelationId);
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                foreach (var item in feedItems)
                {
                    var entity = dbContext.Categories.FirstOrDefault(x => x.Id == item.Id);
                    if (entity == null)
                    {
                        entity = new CatalogCategory()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        };
                        dbContext.Categories.Add(item);
                    }
                    else
                    {
                        dbContext.Entry(entity).State = EntityState.Modified;
                        entity.Name = item.Name;
                        entity.Description = item.Description;
                    }

                }
                await dbContext.SaveChangesAsync();
            };
        }
    }
}
