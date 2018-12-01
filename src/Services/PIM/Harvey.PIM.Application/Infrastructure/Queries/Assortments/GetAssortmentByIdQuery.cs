using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Assortments
{
    public sealed class GetAssortmentByIdQuery : IQuery<AssortmentModel>
    {
        public Guid Id { get; }
        public GetAssortmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetAssortmentByIdQueryHandler : IQueryHandler<GetAssortmentByIdQuery, AssortmentModel>
    {
        private readonly IEfRepository<PimDbContext, Assortment, AssortmentModel> _repository;
        public GetAssortmentByIdQueryHandler(IEfRepository<PimDbContext, Assortment, AssortmentModel> repository)
        {
            _repository = repository;
        }
        public async Task<AssortmentModel> Handle(GetAssortmentByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
