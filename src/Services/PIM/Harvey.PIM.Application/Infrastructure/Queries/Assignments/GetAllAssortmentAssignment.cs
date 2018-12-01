using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Assignments
{
    public class GetAllAssortmentAssignment: IQuery<List<AssortmentAssignmentModel>>
    {

    }

    public class GetAllAssortmentAssignmentHandler : IQueryHandler<GetAllAssortmentAssignment, List<AssortmentAssignmentModel>>
    {
        private readonly IEfRepository<PimDbContext, Product> _repositoryProduct;
        private readonly IEfRepository<PimDbContext, Category> _repositoryCategory;
        public GetAllAssortmentAssignmentHandler(IEfRepository<PimDbContext, Product> repositoryProduct,
                                                    IEfRepository<PimDbContext, Category> repositoryCategory)
        {
            _repositoryProduct = repositoryProduct;
            _repositoryCategory = repositoryCategory;
        }
        public async Task<List<AssortmentAssignmentModel>> Handle(GetAllAssortmentAssignment query)
        {
            var assignments = new List<AssortmentAssignmentModel>();
            var products = await _repositoryProduct.GetAsync();
            var categories = await _repositoryCategory.GetAsync();
            products.ToList().ForEach(product =>
            {
                var assignment = new AssortmentAssignmentModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Type = AssortmentAssignmentType.Product
                };
                assignments.Add(assignment);
            });

            categories.ToList().ForEach(category =>
            {
                var assignment = new AssortmentAssignmentModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Type = AssortmentAssignmentType.Category
                };
                assignments.Add(assignment);
            });

            return assignments;
        }
    }
}
