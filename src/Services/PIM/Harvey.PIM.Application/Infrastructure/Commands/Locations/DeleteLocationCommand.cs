using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Locations
{
    public sealed class DeleteLocationCommand: ICommand<LocationModel>
    {
        public Guid Id { get; set; }
        public DeleteLocationCommand(Guid id)
        {
            Id = id;
        }
    }
    public sealed class DeleteLocationCommandHandler : ICommandHandler<DeleteLocationCommand, LocationModel>
    {
        private readonly IEfRepository<PimDbContext, Location> _repository;
        private readonly IMapper _mapper;
        public DeleteLocationCommandHandler(IEfRepository<PimDbContext, Location> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LocationModel> Handle(DeleteLocationCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if(entity == null)
            {
                throw new ArgumentException($"location {command.Id} is not presented.");
            }
            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();

            return _mapper.Map<LocationModel>(entity);
        }
    }
}
