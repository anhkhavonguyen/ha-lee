using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.PIM.Application.Channels.Prices
{
    public class ChannelPriceConveter : IFeedConverter<Price, CatalogPrice>
    {
        public bool CanConvert(Type type) => type.GetType() == typeof(List<Price>).GetType();

        public IEnumerable<CatalogPrice> Convert(IEnumerable<Price> source)
        {
            return source.Select(x => new CatalogPrice()
            {
                Id = x.Id,
                ListPrice = x.ListPrice,
                MemberPrice = x.MemberPrice,
                StaffPrice = x.StaffPrice
            });
        }
    }
}
