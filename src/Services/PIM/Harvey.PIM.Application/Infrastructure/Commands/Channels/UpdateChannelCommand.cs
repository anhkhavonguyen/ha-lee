using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Channels
{
    public class UpdateChannelCommand : ICommand<ChannelModel>
    {
        public Guid Updater { get; }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string ServerInformation { get; }
        public bool IsProvision { get; }
        public UpdateChannelCommand(Guid updater, Guid id, string name, string description, string server_information, bool isProvision)
        {
            Updater = updater;
            Id = id;
            Name = name;
            Description = description;
            ServerInformation = server_information;
            IsProvision = isProvision;
        }
    }

    public class UpdateChannelCommandHandler : ICommandHandler<UpdateChannelCommand, ChannelModel>
    {
        private readonly IEfRepository<PimDbContext, Channel> _repository;
        private readonly IMapper _mapper;
        public UpdateChannelCommandHandler(IEfRepository<PimDbContext, Channel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ChannelModel> Handle(UpdateChannelCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"assortment {command.Id} is not presented.");
            }
            entity.UpdatedBy = command.Updater;
            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.ServerInformation = command.ServerInformation;
            entity.IsProvision = command.IsProvision;
            await _repository.UpdateAsync(entity);

            return _mapper.Map<ChannelModel>(entity);
        }
    }
}
