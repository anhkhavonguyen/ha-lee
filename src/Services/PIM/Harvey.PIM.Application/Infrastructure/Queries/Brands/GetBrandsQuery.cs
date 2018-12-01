using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Brands
{
    public sealed class GetBrandsQuery : IQuery<PagedResult<BrandModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetBrandsQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }

    public sealed class GetBrandsQueryHandler : IQueryHandler<GetBrandsQuery, PagedResult<BrandModel>>
    {
        private readonly IEfRepository<PimDbContext, Brand, BrandModel> _repository;

        public GetBrandsQueryHandler(IEfRepository<PimDbContext, Brand, BrandModel> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<BrandModel>> Handle(GetBrandsQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<BrandModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }
}
