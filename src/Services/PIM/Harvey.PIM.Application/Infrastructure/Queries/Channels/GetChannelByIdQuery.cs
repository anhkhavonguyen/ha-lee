using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Channels
{
    public sealed class GetChannelByIdQuery : IQuery<ChannelModel>
    {
        public Guid Id { get; }
        public GetChannelByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public sealed class GetChannelByIdQueryHandler : IQueryHandler<GetChannelByIdQuery, ChannelModel>
    {
        private readonly IEfRepository<PimDbContext, Channel, ChannelModel> _repository;
        public GetChannelByIdQueryHandler(IEfRepository<PimDbContext, Channel, ChannelModel> repository)
        {
            _repository = repository;
        }
        public async Task<ChannelModel> Handle(GetChannelByIdQuery query)
        {
            return await _repository.GetByIdAsync(query.Id);
        }
    }
}
