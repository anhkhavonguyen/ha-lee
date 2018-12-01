namespace Harvey.Search.Abstractions
{
    public interface ISearchResult<TSearchItem>
        where TSearchItem : ISearchItem
    {
        TSearchItem Item { get; }
        void SetItem(TSearchItem item);
    }
}
