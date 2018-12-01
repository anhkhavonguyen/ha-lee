using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Categories
{
    public sealed class GetAllCategoriesQuery : IQuery<IEnumerable<CategoryModel>>
    {
        public GetAllCategoriesQuery()
        {
        }
    }

    public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryModel>>
    {
        private readonly IEfRepository<PimDbContext, Category, CategoryModel> _repository;
        public GetAllCategoriesQueryHandler(IEfRepository<PimDbContext, Category, CategoryModel> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<CategoryModel>> Handle(GetAllCategoriesQuery query)
        {
            return await _repository.GetAsync();
        }
    }
}
