using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.Infrastructure.Queries.Fields
{
    public sealed class GetAllFieldsQuery : IQuery<IEnumerable<FieldModel>>
    {
    }

    public sealed class GetAllFieldsQueryHandler : IQueryHandler<GetAllFieldsQuery, IEnumerable<FieldModel>>
    {
        private readonly IEfRepository<PimDbContext, Field, FieldModel> _repository;
        public GetAllFieldsQueryHandler(IEfRepository<PimDbContext, Field, FieldModel> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<FieldModel>> Handle(GetAllFieldsQuery query)
        {
            var result = await _repository.GetAsync();
            return result;
        }
    }
}