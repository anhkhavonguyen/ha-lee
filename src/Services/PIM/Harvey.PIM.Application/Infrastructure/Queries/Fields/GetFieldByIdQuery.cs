using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Fields
{
    public sealed class GetFieldByIdQuery : IQuery<FieldModel>
    {
        public Guid Id { get; }
        public GetFieldByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetFieldByIdQueryHandler : IQueryHandler<GetFieldByIdQuery, FieldModel>
    {
        private readonly IEfRepository<PimDbContext, Field, FieldModel> _repository;
        public GetFieldByIdQueryHandler(IEfRepository<PimDbContext, Field, FieldModel> repository)
        {
            _repository = repository;
        }
        public async Task<FieldModel> Handle(GetFieldByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
