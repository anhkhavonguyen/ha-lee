using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Fields
{
    public sealed class UpdateFieldCommand : ICommand<FieldModel>
    {
        public Guid Updater { get; }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public FieldType Type { get; }
        public string DefaultValue { get; }
        public UpdateFieldCommand(Guid updater, Guid id, string name, string description, string defaultValue)
        {
            Updater = updater;
            Id = id;
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }
    }

    public sealed class UpdateFieldCommandHandler : ICommandHandler<UpdateFieldCommand, FieldModel>
    {
        private readonly IEfRepository<PimDbContext, Field> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        public UpdateFieldCommandHandler(
            IEfRepository<PimDbContext, Field> repository,
            IEventBus eventBus,
            IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<FieldModel> Handle(UpdateFieldCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"category {command.Id} is not presented.");
            }
            entity.UpdatedBy = command.Updater;
            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.DefaultValue = command.DefaultValue;
            await _repository.UpdateAsync(entity);
            return _mapper.Map<FieldModel>(entity);
        }
    }
}
