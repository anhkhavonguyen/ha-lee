using System;
using System.Threading.Tasks;
using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Categories;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.Infrastructure.Commands.Categories
{
    public sealed class UpdateCategoryCommand : ICommand<CategoryModel>
    {
        public Guid Updater { get; }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public UpdateCategoryCommand(Guid updater, Guid id, string name, string description)
        {
            Updater = updater;
            Id = id;
            Name = name;
            Description = description;
        }
    }

    public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryModel>
    {
        private readonly IEfRepository<PimDbContext, Category> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        public UpdateCategoryCommandHandler(
            IEfRepository<PimDbContext, Category> repository,
            IEventBus eventBus,
            IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<CategoryModel> Handle(UpdateCategoryCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"category {command.Id} is not presented.");
            }
            entity.UpdatedBy = command.Updater;
            entity.Name = command.Name;
            entity.Description = command.Description;
            await _repository.UpdateAsync(entity);
            await _eventBus.PublishAsync(new CategoryUpdatedEvent(entity.Id.ToString())
            {
                Name = entity.Name,
                Description = entity.Description
            });
            return _mapper.Map<CategoryModel>(entity);
        }
    }
}
