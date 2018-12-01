using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.Application.Infrastructure.Models;
using static Harvey.PIM.Application.Infrastructure.Models.AddProductModel;

namespace Harvey.PIM.Application.Infrastructure.Commands.Products
{
    public sealed class UpdateProductCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DetailFieldValueModel> ProductFields { get; }
        public Dictionary<AddVariantModel, List<DetailFieldValueModel>> VariantFields { get; }
        public Dictionary<Guid, PriceModel> VariantPrices { get; }
        public string IndexingValue { get; set; }

        public UpdateProductCommand(
           Guid id,
           Guid categoryId,
           string name,
           string description,
           List<DetailFieldValueModel> productFields,
           Dictionary<AddVariantModel, List<DetailFieldValueModel>> variantFields,
           Dictionary<Guid, PriceModel> variantPrices,
           string indexingValue)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            ProductFields = productFields;
            VariantFields = variantFields;
            VariantPrices = variantPrices;
            IndexingValue = indexingValue;
        }
    }

    public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, bool>
    {
        private readonly IEventStoreRepository<Product> _eventStoreRepository;
        public UpdateProductCommandHandler(IEventStoreRepository<Product> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }
        public async Task<bool> Handle(UpdateProductCommand command)
        {
            var product = new Product()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description
            };

            product.UpdateProduct(command.Id, command.CategoryId, command.Name, command.Description, command.ProductFields, command.IndexingValue);

            foreach (var item in command.VariantFields)
            {
                switch (item.Key.Action)
                {
                    case ItemActionType.Add:
                        product.AddVariant(product.Id, item.Key.Id, command.VariantFields[item.Key], command.VariantPrices[item.Key.Id]);
                        break;
                    case ItemActionType.Update:
                        product.UpdateVariant(product.Id, item.Key.Id, command.VariantFields[item.Key], command.VariantPrices[item.Key.Id]);
                        break;
                    case ItemActionType.Delete:
                        break;
                }
            }

            await _eventStoreRepository.SaveAsync(product);

            return true;
        }
    }
}
