using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Products;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Harvey.PIM.MarketingAutomation;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Channels
{
    public sealed class AddChannelCommand : ICommand<ChannelModel>
    {
        public Guid Creator { get; }
        public string Name { get; }
        public string Description { get; }
        public string ServerInformation { get; }
        public AddChannelCommand(Guid creator, string name, string description, string server_information)
        {
            Creator = creator;
            Name = name;
            Description = description;
            ServerInformation = server_information;
        }
    }
    public sealed class AddChannelCommandHandler : ICommandHandler<AddChannelCommand, ChannelModel>
    {
        private readonly IEfRepository<PimDbContext, Channel> _repository;
        private readonly IMapper _mapper;
        public AddChannelCommandHandler(
            IEfRepository<PimDbContext, Channel> repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ChannelModel> Handle(AddChannelCommand command)
        {
            var channel = new Channel
            {
                UpdatedBy = command.Creator,
                CreatedBy = command.Creator,
                Name = command.Name,
                Description = command.Description,
                ServerInformation = command.ServerInformation,
                IsProvision = false
            };

            var entity = await _repository.AddAsync(channel);
            return _mapper.Map<ChannelModel>(entity);
        }
    }
}
