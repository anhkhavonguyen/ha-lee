using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Fields
{
    public sealed class GetFieldsQuery : IQuery<PagedResult<FieldModel>>
    {
        public PagingFilterCriteria PagingFilterCriteria { get; }
        public GetFieldsQuery(PagingFilterCriteria pagingFilterCriteria)
        {
            PagingFilterCriteria = pagingFilterCriteria;
        }
    }

    public sealed class GetFieldsQueryHandler : IQueryHandler<GetFieldsQuery, PagedResult<FieldModel>>
    {
        private readonly IEfRepository<PimDbContext, Field, FieldModel> _repository;
        public GetFieldsQueryHandler(IEfRepository<PimDbContext, Field, FieldModel> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<FieldModel>> Handle(GetFieldsQuery query)
        {
            var result = await _repository.GetAsync(query.PagingFilterCriteria.Page, query.PagingFilterCriteria.NumberItemsPerPage);
            var totalPages = await _repository.Count();
            return new PagedResult<FieldModel>()
            {
                CurrentPage = query.PagingFilterCriteria.Page,
                NumberItemsPerPage = query.PagingFilterCriteria.NumberItemsPerPage,
                TotalItems = totalPages,
                Data = result
            };
        }
    }
}
