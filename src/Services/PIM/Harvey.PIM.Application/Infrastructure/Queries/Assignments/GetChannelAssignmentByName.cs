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
    public class GetChannelAssignmentByName: IQuery<List<ChannelAssignmentModel>>
    {
        public string Name { get; set; }
        public GetChannelAssignmentByName(string name)
        {
            Name = name;
        }
    }

    public class GetChannelAssignmentByNameHandler : IQueryHandler<GetChannelAssignmentByName, List<ChannelAssignmentModel>>
    {
        private readonly IEfRepository<PimDbContext, Assortment> _repository;
        public GetChannelAssignmentByNameHandler(IEfRepository<PimDbContext, Assortment> repository)
        {
            _repository = repository;
        }
        public async Task<List<ChannelAssignmentModel>> Handle(GetChannelAssignmentByName query)
        {
            var assignments = new List<ChannelAssignmentModel>();
            var channels = await _repository.ListAsync(x => x.Name.ToUpper().Contains(query.Name.ToUpper()));
            channels.ToList().ForEach(channel =>
            {
                var assignment = new ChannelAssignmentModel()
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Type = ChannelAssignmentType.Assortment
                };
                assignments.Add(assignment);
            });

            return assignments;
        }
    }
}
