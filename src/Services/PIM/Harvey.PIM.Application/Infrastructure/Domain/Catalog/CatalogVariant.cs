using Harvey.PIM.MarketingAutomation;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog
{
    public class CatalogVariant : FeedItemBase
    {
        public Guid ProductId { get; set; }
        public Guid PriceId { get; set; }
    }
}
