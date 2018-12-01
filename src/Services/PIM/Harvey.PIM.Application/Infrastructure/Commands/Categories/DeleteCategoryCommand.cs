using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Categories
{
    public sealed class DeleteCategoryCommand : ICommand<bool>
    {
        public Guid Id { get; }
        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }

    public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly IEventBus _eventBus;
        private IEfRepository<PimDbContext, Category> _repository;

        public DeleteCategoryCommandHandler(
            IEfRepository<PimDbContext, Category> repository,
            IEventBus eventBus)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCategoryCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Category {command.Id} is not presented.");
            }
            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}
