using Harvey.Search.Abstractions;

namespace Harvey.Search
{
    public abstract class IndexedItem<TSearchItem> : IIndexedItem
        where TSearchItem : ISearchItem
    {
        public abstract string IndexName { get; }
        public TSearchItem Item { get; set; }
        public IndexedItem(TSearchItem item)
        {
            Item = item;
        }
    }
}
