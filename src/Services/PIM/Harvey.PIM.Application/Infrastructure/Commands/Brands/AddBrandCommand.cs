using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Brands
{
    public sealed class AddBrandCommand : ICommand<BrandModel>
    {
        public Guid Creator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddBrandCommand(Guid creator, string name, string description)
        {
            Creator = creator;
            Name = name;
            Description = description;
        }
    }

    public sealed class AddBrandCommandHandler : ICommandHandler<AddBrandCommand, BrandModel>
    {
        private readonly IEfRepository<PimDbContext, Brand> _repository;
        private readonly IMapper _mapper;
        public AddBrandCommandHandler(IEventBus eventBus, IEfRepository<PimDbContext, Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<BrandModel> Handle(AddBrandCommand command)
        {
            var brand = new Brand
            {
                UpdatedBy = command.Creator,
                CreatedBy = command.Creator,
                Name = command.Name,
                Description = command.Description,
            };

            var entity = await _repository.AddAsync(brand);

            return _mapper.Map<BrandModel>(entity);
        }
    }
}
