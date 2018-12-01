using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Harvey.Search.Abstractions;

namespace Harvey.Search
{
    public abstract class SearchQuery<TSearchItem> : ISearchQuery<TSearchItem>
        where TSearchItem : SearchItem
    {
        public string QueryText { get; set; }
        public int NumberItemsPerPage { get; set; }
        public int Page { get; set; }
        public abstract string IndexName { get; }
        public abstract List<Expression<Func<TSearchItem, object>>> Matches { get; }
    }
}
