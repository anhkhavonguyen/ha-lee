using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Channels
{
    public class DeleteChannelCommand: ICommand<bool>
    {
        public Guid Id { get; }
        public DeleteChannelCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteChannelCommandHandler : ICommandHandler<DeleteChannelCommand, bool>
    {
        private IEfRepository<PimDbContext, Channel> _repository;
        public DeleteChannelCommandHandler(IEfRepository<PimDbContext, Channel> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteChannelCommand command)
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
