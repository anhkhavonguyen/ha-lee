using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Brands
{
    public sealed class GetBrandByIdQuery : IQuery<BrandModel>
    {
        public Guid Id { get; }
        public GetBrandByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetBrandByIdQueryHandler : IQueryHandler<GetBrandByIdQuery, BrandModel>
    {
        private readonly IEfRepository<PimDbContext, Brand, BrandModel> _repository;
        public GetBrandByIdQueryHandler(IEfRepository<PimDbContext, Brand, BrandModel> repository)
        {
            _repository = repository;
        }
        public async Task<BrandModel> Handle(GetBrandByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
