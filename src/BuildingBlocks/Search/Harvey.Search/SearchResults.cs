using System.Collections.Generic;
using Harvey.Search.Abstractions;

namespace Harvey.Search
{
    public class SearchResults<TSearchItem, TSearchResult> : ISearchResults<TSearchItem, TSearchResult>
        where TSearchItem : ISearchItem
        where TSearchResult : ISearchResult<TSearchItem>
    {
        public IEnumerable<TSearchResult> Results { get; set; }
        public long TotalItems { get; set; }
    }
}
