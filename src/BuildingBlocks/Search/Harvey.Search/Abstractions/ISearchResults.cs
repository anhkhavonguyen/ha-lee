using System.Collections.Generic;

namespace Harvey.Search.Abstractions
{
    public interface ISearchResults<TSearchItem, TSearchResult>
        where TSearchItem : ISearchItem
        where TSearchResult : ISearchResult<TSearchItem>
    {
        IEnumerable<TSearchResult> Results { get; }
        long TotalItems { get; }
    }
}
