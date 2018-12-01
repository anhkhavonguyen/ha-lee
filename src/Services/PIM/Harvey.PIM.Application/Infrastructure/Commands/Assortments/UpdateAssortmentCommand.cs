using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Assortments
{
    public sealed class UpdateAssortmentCommand : ICommand<AssortmentModel>
    {
        public Guid Updater { get; }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public UpdateAssortmentCommand(Guid updater, Guid id, string name, string description)
        {
            Updater = updater;
            Id = id;
            Name = name;
            Description = description;
        }
    }

    public sealed class UpdateAssortmentCommandHandler : ICommandHandler<UpdateAssortmentCommand, AssortmentModel>
    {
        private readonly IEfRepository<PimDbContext, Assortment> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        public UpdateAssortmentCommandHandler(
            IEfRepository<PimDbContext, Assortment> repository,
            IEventBus eventBus,
            IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<AssortmentModel> Handle(UpdateAssortmentCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"assortment {command.Id} is not presented.");
            }
            entity.UpdatedBy = command.Updater;
            entity.Name = command.Name;
            entity.Description = command.Description;
            await _repository.UpdateAsync(entity);

            return _mapper.Map<AssortmentModel>(entity);
        }
    }
}
