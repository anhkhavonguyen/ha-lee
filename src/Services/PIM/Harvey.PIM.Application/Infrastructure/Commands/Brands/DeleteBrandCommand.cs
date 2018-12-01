using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Brands
{
    public sealed class DeleteBrandCommand : ICommand<bool>
    {
        public Guid Id { get; }
        public DeleteBrandCommand(Guid id)
        {
            Id = id;
        }
    }

    public sealed class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand, bool>
    {
        private readonly IEfRepository<PimDbContext, Brand> _repository;
        private readonly IMapper _mapper;

        public DeleteBrandCommandHandler(IEfRepository<PimDbContext, Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteBrandCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Brand {command.Id} is not presented.");
            }
            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}
