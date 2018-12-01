using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Indexing;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.Search.Abstractions;

namespace Harvey.PIM.Application.Infrastructure.Queries.Products
{
    public sealed class GetProductListQuery : IQuery<PagedResult<ProductListModel>>
    {
        public string QueryText { get; }
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetProductListQuery(PagingFilterCriteria pagingFilterCriteria, string queryText)
        {
            PagingFilterCriteria = pagingFilterCriteria;
            QueryText = queryText;
        }
    }

    public sealed class GetProductListQueryHandler : IQueryHandler<GetProductListQuery, PagedResult<ProductListModel>>
    {
        private readonly ISearchService _searchService;
        private readonly IMapper _mapper;
        public GetProductListQueryHandler(ISearchService searchService, IMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }
        public async Task<PagedResult<ProductListModel>> Handle(GetProductListQuery query)
        {
            var productSearchQuery = new ProductSearchQuery()
            {
                QueryText = query.QueryText,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                Page = query.PagingFilterCriteria.Page
            };

            var result = await _searchService.SearchAsync<ProductSearchItem, ProductSearchResult>(productSearchQuery);
            var data = result.Results.Select(x => new ProductListModel
            {
                Description = x.Item.Description,
                Name = x.Item.Name,
                Id = x.Item.Id
            });
            return new PagedResult<ProductListModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = (int)result.TotalItems,
                Data = data
            };
        }
    }
}
