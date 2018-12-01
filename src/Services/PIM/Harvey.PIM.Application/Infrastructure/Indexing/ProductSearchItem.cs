using Harvey.PIM.Application.Events.Products;
using Harvey.Search;
using Harvey.Search.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Harvey.PIM.Application.Infrastructure.Indexing
{
    public class ProductSearchItem : SearchItem, ISearchItem
    {
        public ProductSearchItem()
        {
        }
        public ProductSearchItem(Guid id) : base(id)
        {
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IndexingValue { get; set; }
    }

    public class ProductSearchIndexedItem : IndexedItem<ProductSearchItem>
    {
        public ProductSearchIndexedItem(ProductSearchItem item) : base(item)
        {
        }

        public override string IndexName => Domain.Product.IndexName;
    }

    public class ProductSearchQuery : SearchQuery<ProductSearchItem>, ISearchQuery<ProductSearchItem>
    {
        public override string IndexName => Domain.Product.IndexName;

        public override List<Expression<Func<ProductSearchItem, object>>> Matches => new List<Expression<Func<ProductSearchItem, object>>>()
        {
            x=>x.Name,
            x=>x.Description,
            x=>x.IndexingValue
        };
    }

    public class ProductSearchResults : SearchResults<ProductSearchItem, ProductSearchResult>, ISearchResults<ProductSearchItem, ProductSearchResult>
    {

    }

    public class ProductSearchResult : SearchResult<ProductSearchItem>, ISearchResult<ProductSearchItem>
    {
        public ProductSearchResult()
        {

        }

        public ProductSearchResult(ProductSearchItem item) : base(item)
        {

        }
    }
}
