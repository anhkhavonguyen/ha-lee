using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.PIM.Application.Channels.Categories
{
    public class ChannelCategoryConveter : IFeedConverter<Category, CatalogCategory>
    {
        public bool CanConvert(Type type) => type.GetType() == typeof(List<Category>).GetType();


        public IEnumerable<CatalogCategory> Convert(IEnumerable<Category> source)
        {
            return source.Select(x => new CatalogCategory() {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });
        }
    }
}
