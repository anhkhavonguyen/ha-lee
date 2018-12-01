using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.Infrastructure.Commands.Assignments
{
    public class AddChannelAssignmentCommand: ICommand<bool>
    {
        public Guid ChannelId { get; set; }
        public List<AddChannelAssignmentModel> ChannelAssignments { get; set; }
        public AddChannelAssignmentCommand(List<AddChannelAssignmentModel> channelAssignments, Guid channelId)
        {
            ChannelAssignments = channelAssignments;
            ChannelId = channelId;
        }
    }

    public class AddChannelAssignmentCommandHandler : ICommandHandler<AddChannelAssignmentCommand, bool>
    {
        private readonly IEfRepository<PimDbContext, ChannelAssignment> _repository;
        public AddChannelAssignmentCommandHandler(IEfRepository<PimDbContext, ChannelAssignment> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(AddChannelAssignmentCommand command)
        {
            var channelAssignments = new List<ChannelAssignment>();
            var channelSelected = await _repository.ListAsync(x => x.ChannelId == command.ChannelId);
            command.ChannelAssignments.ForEach(x =>
            {
                var channelAssignment = new ChannelAssignment()
                {
                    ChannelId = command.ChannelId,
                    EntityType = x.EntityType,
                    ReferenceId = x.ReferenceId,
                };
                channelAssignments.Add(channelAssignment);
            });
            await _repository.DeleteAsync(channelSelected.ToList());
            await _repository.AddAsync(channelAssignments);
            return true;
        }
    }
}
