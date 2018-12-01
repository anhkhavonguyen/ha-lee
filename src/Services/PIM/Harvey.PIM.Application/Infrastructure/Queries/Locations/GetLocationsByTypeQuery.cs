using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Locations
{
    public sealed class GetLocationsByTypeQuery : IQuery<IEnumerable<LocationModel>>
    {
        public LocationType LocationType { get; }
        public GetLocationsByTypeQuery(LocationType locationType)
        {
            LocationType = locationType;
        }
    }

    public sealed class GetLocationByTypeQueryHandler : IQueryHandler<GetLocationsByTypeQuery, IEnumerable<LocationModel>>
    {
        private readonly TransientPimDbContext _transientPimDbContext;
        private readonly IMapper _mapper;

        public GetLocationByTypeQueryHandler(
            TransientPimDbContext transientPimDbContext,
            IMapper mapper)
        {
            _transientPimDbContext = transientPimDbContext;
            _mapper = mapper;

        }
        public async Task<IEnumerable<LocationModel>> Handle(GetLocationsByTypeQuery query)
        {
            //var locations = await _efRepository.ListAsync(x => x.Typequery.LocationType);
            var locations = await _transientPimDbContext.Locations.Where(x => x.Type == query.LocationType).ToListAsync();
            var result = _mapper.Map<IList<LocationModel>>(locations);
            return result;
        }
    }
}
