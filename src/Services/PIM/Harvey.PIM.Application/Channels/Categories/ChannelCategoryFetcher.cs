using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.MarketingAutomation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Categories
{
    public class ChannelCategoryFetcher : IFeedFetcher<Category>
    {
        private readonly TransientPimDbContext _pimDbContext;
        public ChannelCategoryFetcher(TransientPimDbContext pimDbContext)
        {
            _pimDbContext = pimDbContext;
        }
        public async Task<IEnumerable<Category>> FetchAsync()
        {
            return await _pimDbContext.Categories.AsNoTracking().ToListAsync();
        }
    }
}
