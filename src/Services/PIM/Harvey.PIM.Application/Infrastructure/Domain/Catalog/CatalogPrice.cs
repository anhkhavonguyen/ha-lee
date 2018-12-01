using Harvey.PIM.MarketingAutomation;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog
{
    public class CatalogPrice : FeedItemBase
    {
        public float ListPrice { get; set; }
        public float StaffPrice { get; set; }
        public float MemberPrice { get; set; }
    }
}
