using Harvey.Search.Abstractions;

namespace Harvey.Search
{
    public class SearchResult<TSearchItem> : ISearchResult<TSearchItem>
        where TSearchItem : ISearchItem
    {
        public TSearchItem Item { get; private set; }

        public SearchResult()
        {

        }

        public SearchResult(TSearchItem item)
        {
            Item = item;
        }

        public void SetItem(TSearchItem item)
        {
            Item = item;
        }
    }
}
