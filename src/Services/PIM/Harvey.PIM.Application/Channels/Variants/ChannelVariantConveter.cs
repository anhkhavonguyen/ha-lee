using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.PIM.Application.Channels.Variants
{
    public class ChannelVariantConveter : IFeedConverter<Variant, CatalogVariant>
    {
        public bool CanConvert(Type type) => type.GetType() == typeof(List<Variant>).GetType();

        public IEnumerable<CatalogVariant> Convert(IEnumerable<Variant> source)
        {
            return source.Select(x => new CatalogVariant()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                PriceId = x.PriceId
            });
        }
    }
}
