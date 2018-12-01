using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Assortments
{
    public sealed class DeleteAssortmentCommand : ICommand<bool>
    {
        public Guid Id { get; }
        public DeleteAssortmentCommand(Guid id)
        {
            Id = id;
        }
    }

    public sealed class DeleteAssortmentCommandHandler : ICommandHandler<DeleteAssortmentCommand, bool>
    {
        private readonly IEventBus _eventBus;
        private IEfRepository<PimDbContext, Assortment> _repository;

        public DeleteAssortmentCommandHandler(
            IEfRepository<PimDbContext, Assortment> repository,
            IEventBus eventBus)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteAssortmentCommand command)
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
