using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Harvey.Search.Abstractions
{
    public interface ISearchQuery<TSearchItem>
        where TSearchItem : ISearchItem
    {
        string QueryText { get; set; }
        string IndexName { get; }
        int NumberItemsPerPage { get; set; }
        int Page { get; set; }
        List<Expression<Func<TSearchItem, object>>> Matches { get; }
    }
}
