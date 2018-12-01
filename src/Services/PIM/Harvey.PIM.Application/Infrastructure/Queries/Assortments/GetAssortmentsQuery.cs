using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Assortments
{
    public sealed class GetAssortmentsQuery : IQuery<PagedResult<AssortmentModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetAssortmentsQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }

    public sealed class GetAssortmentsQueryHandler : IQueryHandler<GetAssortmentsQuery, PagedResult<AssortmentModel>>
    {
        private readonly IEfRepository<PimDbContext, Assortment, AssortmentModel> _repository;
        public GetAssortmentsQueryHandler(IEfRepository<PimDbContext, Assortment, AssortmentModel> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<AssortmentModel>> Handle(GetAssortmentsQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<AssortmentModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }
}
