using AutoMapper;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Assortments
{
    public sealed class AddAssortmentCommand : ICommand<AssortmentModel>
    {
        public Guid Creator { get; }
        public string Name { get; }
        public string Description { get; }
        public AddAssortmentCommand(Guid creator, string name, string description)
        {
            Creator = creator;
            Name = name;
            Description = description;
        }
    }
    public sealed class AddAssortmentCommandHandler : ICommandHandler<AddAssortmentCommand, AssortmentModel>
    {
        private readonly IEventBus _eventBus;
        private readonly IEfRepository<PimDbContext, Assortment> _repository;
        private readonly IMapper _mapper;
        public AddAssortmentCommandHandler(
            IEfRepository<PimDbContext, Assortment> repository,
            IEventBus eventBus,
            IMapper mapper
            )
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<AssortmentModel> Handle(AddAssortmentCommand command)
        {
            var assortment = new Assortment
            {
                UpdatedBy = command.Creator,
                CreatedBy = command.Creator,
                Name = command.Name,
                Description = command.Description
            };

            var entity = await _repository.AddAsync(assortment);
           
            return _mapper.Map<AssortmentModel>(entity);
        }
    }
}
