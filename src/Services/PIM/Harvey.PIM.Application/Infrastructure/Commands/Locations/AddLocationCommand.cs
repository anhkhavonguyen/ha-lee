using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Locations
{
    public sealed class AddLocationCommand : ICommand<LocationModel>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }
        public Guid Creator { get; }
        public AddLocationCommand(string name, string address, LocationType type, Guid creator)
        {
            Name = name;
            Address = address;
            Type = type;
            Creator = creator;
        }
    }

    public sealed class AddLocationCommandHandler : ICommandHandler<AddLocationCommand, LocationModel>
    {
        private readonly IEfRepository<PimDbContext, Location> _repository;
        private readonly IMapper _mapper;
        public AddLocationCommandHandler(IEfRepository<PimDbContext, Location> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LocationModel> Handle(AddLocationCommand command)
        {
            Location newLocation = new Location()
            {
                Name = command.Name,
                Address = command.Address,
                Type = command.Type,
                CreatedBy = command.Creator,
                UpdatedBy = command.Creator
            };
            var entity = await _repository.AddAsync(newLocation);
            return _mapper.Map<LocationModel>(entity);
        }
    }
}
