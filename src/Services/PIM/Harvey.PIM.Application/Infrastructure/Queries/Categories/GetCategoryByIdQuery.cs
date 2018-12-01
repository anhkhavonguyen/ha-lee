using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Categories
{
    public sealed class GetCategoryByIdQuery : IQuery<CategoryModel>
    {
        public Guid Id { get; }
        public GetCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryModel>
    {
        private readonly IEfRepository<PimDbContext, Category, CategoryModel> _repository;
        public GetCategoryByIdQueryHandler(IEfRepository<PimDbContext, Category, CategoryModel> repository)
        {
            _repository = repository;
        }
        public async Task<CategoryModel> Handle(GetCategoryByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
