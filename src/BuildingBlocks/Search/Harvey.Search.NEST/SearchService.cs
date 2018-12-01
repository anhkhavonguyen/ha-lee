using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Harvey.Exception;
using Harvey.Search.Abstractions;
using Nest;
using Newtonsoft.Json;

namespace Harvey.Search.NEST
{
    public class SearchService : ISearchService
    {
        private readonly SearchSettings _searchSettings;
        public SearchService(SearchSettings searchSettings)
        {
            _searchSettings = searchSettings;
        }

        public async Task AddAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem
        {
            var result = await WithElasticClient(x => x.CreateAsync(searchItem.Item, idx => idx.Index(searchItem.IndexName)));
            if (result.Result == Result.Created)
            {
                return;
            }
            throw new IndexingException($"Cannot add document {JsonConvert.SerializeObject(searchItem)}") { Item = searchItem };

        }

        public async Task UpdateAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem
        {
            var result = await WithElasticClient(x => x
                                .UpdateAsync(
                                    DocumentPath<TSearchItem>.Id(searchItem.Item.Id),
                                    u => u.Index(searchItem.IndexName)
                                        .DocAsUpsert()
                                        .Doc(searchItem.Item)));
            if (result.Result == Result.Updated)
            {
                return;
            }
            throw new IndexingException($"Cannot update document {JsonConvert.SerializeObject(searchItem)}") { Item = searchItem };
        }

        public async Task DeleteAsync<TSearchItem>(IndexedItem<TSearchItem> searchItem)
            where TSearchItem : SearchItem
        {
            var result = await WithElasticClient(x => x.DeleteAsync<TSearchItem>(searchItem.Item, idx => idx.Index(searchItem.IndexName)));
            if (result.Result == Result.Deleted)
            {
                return;
            }
            throw new IndexingException($"Cannot update document {JsonConvert.SerializeObject(searchItem)}") { Item = searchItem };
        }

        public async Task AddAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem) where TSearchItem : SearchItem
        {
            foreach (var item in searchItem)
            {
                await AddAsync(searchItem);
            }
            return;
        }

        public async Task UpdateAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem) where TSearchItem : SearchItem
        {
            foreach (var item in searchItem)
            {
                await UpdateAsync(searchItem);
            }
            return;
        }

        public async Task DeleteAsync<TSearchItem>(List<IndexedItem<TSearchItem>> searchItem) where TSearchItem : SearchItem
        {
            foreach (var item in searchItem)
            {
                await DeleteAsync(searchItem);
            }
            return;
        }

        public async Task<ISearchResults<TSearchItem, TSearchResult>> SearchAsync<TSearchItem, TSearchResult>(ISearchQuery<TSearchItem> query)
            where TSearchItem : SearchItem
            where TSearchResult : SearchResult<TSearchItem>, new()
        {
            var result = await WithElasticClient(
                x => x.SearchAsync<TSearchItem>(s => s
                      .Size(query.NumberItemsPerPage)
                      .Skip(query.NumberItemsPerPage * (query.Page - 1))
                      .Index(query.IndexName)
                      .Query(q =>
                            q.Bool(b =>
                                    b.Should(GetMatches(query))))
                      ));

            var totalItems = await WithElasticClient(x => x.CountAsync<TSearchItem>(c => c.Index(query.IndexName)));

            return new SearchResults<TSearchItem, TSearchResult>()
            {
                Results = result.Documents.Select(x =>
                {
                    var item = new TSearchResult();
                    item.SetItem(x);
                    return item;
                }),
                TotalItems = totalItems.Count
            };
        }


        public async Task<ISearchResults<TSearchItem, TSearchResult>> FuzzySearchAsync<TSearchItem, TSearchResult>(ISearchQuery<TSearchItem> query)
            where TSearchItem : SearchItem
            where TSearchResult : SearchResult<TSearchItem>, new()
        {
            var fuzzyField = query.Matches.FirstOrDefault();
            //TODO need to support multiple field
            var result = await WithElasticClient(
                x => x.SearchAsync<TSearchItem>(s => s
                      .Size(query.NumberItemsPerPage)
                      .Skip(query.NumberItemsPerPage * (query.Page - 1))
                      .Index(query.IndexName)
                      .Query(q => q
                              .Fuzzy(c => c
                                .Name($"fuzzyField.ToString()_{query}")
                                .Field(fuzzyField)
                                .Fuzziness(Fuzziness.Auto)
                                .Value(query.QueryText)
                                .MaxExpansions(100)
                                .PrefixLength(0)
                                .Transpositions()
                                ))));

            var totalItems = await WithElasticClient(x => x.CountAsync<TSearchItem>(c => c.Index(query.IndexName)));

            return new SearchResults<TSearchItem, TSearchResult>()
            {
                Results = result.Documents.Select(x =>
                {
                    var item = new TSearchResult();
                    item.SetItem(x);
                    return item;
                }),
                TotalItems = totalItems.Count
            };
        }

        private List<Func<QueryContainerDescriptor<TSearchItem>, QueryContainer>> GetMatches<TSearchItem>(ISearchQuery<TSearchItem> query)
            where TSearchItem : SearchItem
        {
            var result = new List<Func<QueryContainerDescriptor<TSearchItem>, QueryContainer>>();
            query.Matches.ForEach(match =>
            {
                result.Add((mu) =>
                {
                    mu.Match(m => m.Field(match).Query(query.QueryText));
                    return mu;
                });
            });

            return result;
        }

        private List<Func<QueryContainerDescriptor<TSearchItem>, QueryContainer>> GetFyzzyMatches<TSearchItem>(ISearchQuery<TSearchItem> query)
         where TSearchItem : SearchItem
        {
            var result = new List<Func<QueryContainerDescriptor<TSearchItem>, QueryContainer>>();
            query.Matches.ForEach(match =>
            {
                result.Add((mu) =>
                {
                    mu.Match(m => m.Field(match).Fuzziness(Fuzziness.Auto).Query(query.QueryText));
                    return mu;
                });
            });

            return result;
        }


        public void WithConnectionStrings(Action<ConnectionSettings> action)
        {
            var connectionSettings = new ConnectionSettings(new Uri(_searchSettings.Url));
            action(connectionSettings);
        }

        public Task<T> WithElasticClient<T>(Func<ElasticClient, Task<T>> func)
        {
            var client = new ElasticClient(new Uri(_searchSettings.Url));
            return func(client);
        }

        public async Task DeleteByQueryAsync<T>(string indexName) where T : SearchItem
        {
            var client = new ElasticClient(new Uri(_searchSettings.Url));
            await client.DeleteByQueryAsync<T>(idx => idx.Index(indexName).MatchAll());
            return;
        }

        public async Task InsertDocumentsAsync<T>(List<IndexedItem<T>> items) where T : SearchItem
        {
            if (!items.Any())
            {
                return;
            }
            var client = new ElasticClient(new Uri(_searchSettings.Url));
            var indexName = items.FirstOrDefault().IndexName;

            var descriptor = new BulkDescriptor();
            foreach (var item in items)
            {
                descriptor.Index<T>(op => op.Index(indexName).Document(item.Item));
            }
            await client.BulkAsync(descriptor);
        }


    }
}
