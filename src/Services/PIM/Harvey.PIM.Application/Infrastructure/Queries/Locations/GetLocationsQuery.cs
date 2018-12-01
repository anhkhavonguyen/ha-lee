using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Locations
{
    public sealed class GetLocationsQuery : IQuery<PagedResult<LocationModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetLocationsQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }
    public sealed class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, PagedResult<LocationModel>>
    {
        private readonly IEfRepository<PimDbContext, Location, LocationModel> _repository;
        public GetLocationsQueryHandler(IEfRepository<PimDbContext, Location, LocationModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<LocationModel>> Handle(GetLocationsQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<LocationModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }

}
