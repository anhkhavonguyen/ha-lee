using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Locations
{
    public sealed class GetLocationByIdQuery: IQuery<LocationModel>
    {
        public Guid Id { get; }
        public GetLocationByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetLocationByIdQueryHandler: IQueryHandler<GetLocationByIdQuery, LocationModel>
    {
        private readonly IEfRepository<PimDbContext, Location, LocationModel> _repository;
        public GetLocationByIdQueryHandler(IEfRepository<PimDbContext, Location, LocationModel> repository)
        {
            _repository = repository;
        }

        public async Task<LocationModel> Handle(GetLocationByIdQuery query)
        {
           return await _repository.GetByIdAsync(query.Id);
        }
    }
}
