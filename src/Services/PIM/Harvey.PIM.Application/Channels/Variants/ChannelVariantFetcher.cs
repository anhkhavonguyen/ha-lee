using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Variants
{
    public class ChannelVariantFetcher : IFeedFetcher<Variant>
    {
        private readonly TransientPimDbContext _pimDbContext;
        public ChannelVariantFetcher(TransientPimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }
        public async Task<IEnumerable<Variant>> FetchAsync()
        {
            return await _pimDbContext.Variants.AsNoTracking().ToListAsync();
        }
    }
}
