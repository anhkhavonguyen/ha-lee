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
    public sealed class AddFieldCommand : ICommand<FieldModel>
    {
        public Guid Creator { get; }
        public string Name { get; }
        public string Description { get; }
        public FieldType Type { get; }
        public string DefaultValue { get; }
        public AddFieldCommand(Guid creator, string name, string description, FieldType type, string defaultValue)
        {
            Creator = creator;
            Name = name;
            Description = description;
            Type = type;
            DefaultValue = defaultValue;
        }
    }

    public sealed class AddFieldCommandHandler : ICommandHandler<AddFieldCommand, FieldModel>
    {
        private readonly IEventBus _eventBus;
        private readonly IEfRepository<PimDbContext, Field> _repository;
        private readonly IMapper _mapper;
        public AddFieldCommandHandler(
            IEfRepository<PimDbContext, Field> repository,
            IEventBus eventBus,
            IMapper mapper
            )
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<FieldModel> Handle(AddFieldCommand command)
        {
            var field = new Field
            {
                Id = Guid.NewGuid(),
                UpdatedBy = command.Creator,
                CreatedBy = command.Creator,
                Name = command.Name,
                Description = command.Description,
                DefaultValue = command.DefaultValue,
                Type = command.Type
            };

            var entity = await _repository.AddAsync(field);
            return _mapper.Map<FieldModel>(entity);
        }
    }
}
