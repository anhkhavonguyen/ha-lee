using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.Search.Abstractions
{
    public interface ISearchService
    {
        Task AddAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem;
        Task UpdateAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem;
        Task DeleteAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem;
        Task AddAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem)
            where TSearchItem : SearchItem;
        Task UpdateAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem)
            where TSearchItem : SearchItem;
        Task DeleteAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem)
            where TSearchItem : SearchItem;
        Task<ISearchResults<TSearchItem,TSearchResult>> SearchAsync<TSearchItem, TSearchResult>(ISearchQuery<TSearchItem> query)
            where TSearchItem : SearchItem
            where TSearchResult : SearchResult<TSearchItem>, new();

        Task<ISearchResults<TSearchItem, TSearchResult>> FuzzySearchAsync<TSearchItem, TSearchResult>(ISearchQuery<TSearchItem> query)
            where TSearchItem : SearchItem
            where TSearchResult : SearchResult<TSearchItem>, new();

        Task DeleteByQueryAsync<T>(string indexName) where T : SearchItem;

        Task InsertDocumentsAsync<T>(List<IndexedItem<T>> items) where T : SearchItem;
    }
}
