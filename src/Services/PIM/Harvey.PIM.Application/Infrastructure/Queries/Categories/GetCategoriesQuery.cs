using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Categories
{
    public sealed class GetCategoriesQuery : IQuery<PagedResult<CategoryModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetCategoriesQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }

    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, PagedResult<CategoryModel>>
    {
        private readonly IEfRepository<PimDbContext, Category, CategoryModel> _repository;
        public GetCategoriesQueryHandler(IEfRepository<PimDbContext, Category, CategoryModel> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<CategoryModel>> Handle(GetCategoriesQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<CategoryModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }
}
