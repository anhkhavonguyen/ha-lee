using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Queries.Assignments
{
    public class GetChannelAssignmentSelected : IQuery<List<ChannelAssignmentModel>>
    {
        public Guid ChannelId { get; set; }
        public GetChannelAssignmentSelected(Guid channelId)
        {
            ChannelId = channelId;
        }
    }

    public class GetChannelAssignmentSelectedHandler : IQueryHandler<GetChannelAssignmentSelected, List<ChannelAssignmentModel>>
    {
        private readonly IEfRepository<PimDbContext, Assortment> _repository;
        private readonly IEfRepository<PimDbContext, ChannelAssignment> _repositoryChannelAssignment;
        public GetChannelAssignmentSelectedHandler(IEfRepository<PimDbContext, Assortment> repository, 
                                                   IEfRepository<PimDbContext, ChannelAssignment> repositoryChannelAssignment)
        {
            _repository = repository;
            _repositoryChannelAssignment = repositoryChannelAssignment;
        }
        public async Task<List<ChannelAssignmentModel>> Handle(GetChannelAssignmentSelected query)
        {
            var assignments = new List<ChannelAssignmentModel>();
            var channels = await _repositoryChannelAssignment.ListAsync(x => x.ChannelId == query.ChannelId);

            var assortmentIds = channels.Where(x => x.EntityType == ChannelAssignmentType.Assortment).Select(x => x.ReferenceId);
            var assortments = await _repository.ListAsync(x => assortmentIds.Contains(x.Id));

            assortments.ToList().ForEach(assortment =>
            {
                var assignment = new ChannelAssignmentModel()
                {
                    Id = assortment.Id,
                    Name = assortment.Name,
                    Type = ChannelAssignmentType.Assortment
                };
                assignments.Add(assignment);
            });

            return assignments;
        }
    }
}
