using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Prices
{
    public class ChannelPriceFetcher : IFeedFetcher<Price>
    {
        private readonly TransientPimDbContext _pimDbContext;
        public ChannelPriceFetcher(TransientPimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }
        public async Task<IEnumerable<Price>> FetchAsync()
        {
            return await _pimDbContext.Prices.AsNoTracking().ToListAsync();
        }
    }
}
