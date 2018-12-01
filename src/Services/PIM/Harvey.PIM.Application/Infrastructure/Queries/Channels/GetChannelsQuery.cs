using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Channels
{
    public sealed class GetChannelsQuery : IQuery<PagedResult<ChannelModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetChannelsQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }

    public sealed class GetChannelsQueryHandler : IQueryHandler<GetChannelsQuery, PagedResult<ChannelModel>>
    {
        private IEfRepository<PimDbContext, Channel, ChannelModel> _repository;
        public GetChannelsQueryHandler (IEfRepository<PimDbContext, Channel, ChannelModel> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ChannelModel>> Handle(GetChannelsQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<ChannelModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }
}
