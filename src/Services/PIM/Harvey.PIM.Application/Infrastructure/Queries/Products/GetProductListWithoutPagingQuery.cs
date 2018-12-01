using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Products
{
    public sealed class GetProductListWithoutPagingQuery : IQuery<IEnumerable<ProductListModel>>
    {
        public GetProductListWithoutPagingQuery()
        {
        }
    }

    public sealed class GetProductListWithoutPagingQueryHandler : IQueryHandler<GetProductListWithoutPagingQuery, IEnumerable<ProductListModel>>
    {
        private readonly IEfRepository<PimDbContext, Product, ProductListModel> _efRepository;

        public GetProductListWithoutPagingQueryHandler(IEfRepository<PimDbContext, Product, ProductListModel> efRepository)
        {
            _efRepository = efRepository;
        }
        public async Task<IEnumerable<ProductListModel>> Handle(GetProductListWithoutPagingQuery query)
        {
            var result = await _efRepository.GetAsync();
            return result;
        }
    }
}
