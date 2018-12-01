using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.Brands
{
    public sealed class UpdateBrandCommand : ICommand<BrandModel>
    {
        public Guid Updater { get; }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public UpdateBrandCommand(Guid updater, Guid id, string name, string description)
        {
            Updater = updater;
            Id = id;
            Name = name;
            Description = description;
        }
    }

    public sealed class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand, BrandModel>
    {
        private readonly IEfRepository<PimDbContext, Brand> _repository;
        private readonly IMapper _mapper;

        public UpdateBrandCommandHandler(IEfRepository<PimDbContext, Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<BrandModel> Handle(UpdateBrandCommand command)
        {
            var entity = await _repository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Brand {command.Id} is not presented.");
            }
            entity.UpdatedBy = command.Updater;
            entity.Name = command.Name;
            entity.Description = command.Description;
            await _repository.UpdateAsync(entity);
            
            return _mapper.Map<BrandModel>(entity);
        }
    }
}
