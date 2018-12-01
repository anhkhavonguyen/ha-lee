using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Fields
{
    public sealed class DeleteFieldCommand : ICommand<bool>
    {
        public Guid Id { get; }
        public DeleteFieldCommand(Guid id)
        {
            Id = id;
        }
    }

    public sealed class DeleteFieldCommandHandler : ICommandHandler<DeleteFieldCommand, bool>
    {
        private readonly IEventBus _eventBus;
        private IEfRepository<PimDbContext, Field> _repository;

        public DeleteFieldCommandHandler(
            IEfRepository<PimDbContext, Field> repository,
            IEventBus eventBus)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteFieldCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Field {command.Id} is not presented.");
            }
            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}
