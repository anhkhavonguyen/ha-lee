using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Assignments
{
    public class GetAssortmentAssignmentSelected : IQuery<List<AssortmentAssignmentModel>>
    {
        public Guid AssortmentId { get; set; }
        public GetAssortmentAssignmentSelected(Guid assortmentId)
        {
            AssortmentId = assortmentId;
        }
    }

    public class GetAssortmentAssignmentSelectedHandler : IQueryHandler<GetAssortmentAssignmentSelected, List<AssortmentAssignmentModel>>
    {
        private readonly IEfRepository<PimDbContext, Product> _repositoryProduct;
        private readonly IEfRepository<PimDbContext, Category> _repositoryCategory;
        private readonly IEfRepository<PimDbContext, AssortmentAssignment> _repositoryAssortmentAssignment;
        public GetAssortmentAssignmentSelectedHandler(IEfRepository<PimDbContext, Product> repositoryProduct, 
            IEfRepository<PimDbContext, Category> repositoryCategory, 
            IEfRepository<PimDbContext, AssortmentAssignment> repositoryAssortmentAssignment)
        {
            _repositoryProduct = repositoryProduct;
            _repositoryCategory = repositoryCategory;
            _repositoryAssortmentAssignment = repositoryAssortmentAssignment;
        }
        public async Task<List<AssortmentAssignmentModel>> Handle(GetAssortmentAssignmentSelected query)
        {
            var assignments = new List<AssortmentAssignmentModel>();
            var assorments = await _repositoryAssortmentAssignment.ListAsync(x => x.AssortmentId == query.AssortmentId);

            var productIds = assorments.Where(x => x.EntityType == AssortmentAssignmentType.Product).Select(x => x.ReferenceId);
            var categoryIds = assorments.Where(x => x.EntityType == AssortmentAssignmentType.Category).Select(x => x.ReferenceId);

            var products = await _repositoryProduct.ListAsync(x => productIds.Contains(x.Id));
            var categories = await _repositoryCategory.ListAsync(x => productIds.Contains(x.Id));

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