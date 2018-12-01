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
    public sealed class AddCategoryCommand : ICommand<CategoryModel>
    {
        public Guid Creator { get; }
        public string Name { get; }
        public string Description { get; }
        public AddCategoryCommand(Guid creator, string name, string description)
        {
            Creator = creator;
            Name = name;
            Description = description;
        }
    }

    public sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, CategoryModel>
    {
        private readonly IEventBus _eventBus;
        private readonly IEfRepository<PimDbContext, Category> _repository;
        private readonly IMapper _mapper;
        public AddCategoryCommandHandler(
            IEfRepository<PimDbContext, Category> repository,
            IEventBus eventBus,
            IMapper mapper
            )
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        public async Task<CategoryModel> Handle(AddCategoryCommand command)
        {
            var category = new Category
            {
                UpdatedBy = command.Creator,
                CreatedBy = command.Creator,
                Name = command.Name,
                Description = command.Description,
            };

            var entity = await _repository.AddAsync(category);
            await _eventBus.PublishAsync(new CategoryCreatedEvent(entity.Id.ToString())
            {
                Name = entity.Name,
                Description = entity.Description
            });
            return _mapper.Map<CategoryModel>(entity);
        }
    }
}
