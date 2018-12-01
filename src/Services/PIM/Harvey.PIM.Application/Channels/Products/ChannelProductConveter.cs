using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.PIM.MarketingAutomation;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductConveter : IFeedConverter<ProductFeed, CatalogProductFeed>
    {
        public bool CanConvert(Type type) => type.GetType() == typeof(List<ProductFeed>).GetType();

        public IEnumerable<CatalogProductFeed> Convert(IEnumerable<ProductFeed> source)
        {
            return source.Select(x => new CatalogProductFeed()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                FieldValues = x.FieldValues
            });
        }
    }
}
