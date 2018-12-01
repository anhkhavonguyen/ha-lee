using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Locations
{
    public sealed class UpdateLocationCommand : ICommand<LocationModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }
        public Guid Updater { get; set; }
        public UpdateLocationCommand(Guid id, string name, string address, LocationType type, Guid updater)
        {
            Id = id;
            Name = name;
            Address = address;
            Type = type;
            Updater = updater;
        }
    }

    public sealed class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand, LocationModel>
    {
        private readonly IEfRepository<PimDbContext, Location> _repository;
        private readonly IMapper _mapper;
        public UpdateLocationCommandHandler(IEfRepository<PimDbContext, Location> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LocationModel> Handle(UpdateLocationCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"location {command.Id} is not presented.");
            }

            entity.Name = command.Name;
            entity.Address = command.Address;
            entity.Type = command.Type;
            entity.UpdatedBy = command.Updater;

            await _repository.UpdateAsync(entity);

            return _mapper.Map<LocationModel>(entity);
        }
    }
}
