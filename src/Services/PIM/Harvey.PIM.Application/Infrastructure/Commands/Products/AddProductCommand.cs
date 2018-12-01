using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.Infrastructure.Commands.Products
{
    public sealed class AddProductCommand : ICommand<Product>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Guid FieldTemplateId { get; }
        public Guid CategoryId { get; set; }
        public List<DetailFieldValueModel> ProductFields { get; }
        public Dictionary<Guid, List<DetailFieldValueModel>> VariantFields { get; }
        public Dictionary<Guid, PriceModel> VariantPrices { get; }
        public string IndexingValue { get; }

        public AddProductCommand(
            Guid id,
            Guid categoryId,
            string name,
            string description,
            Guid fieldTemplateId,
            List<DetailFieldValueModel> productFields,
            Dictionary<Guid, List<DetailFieldValueModel>> variantFields,
            Dictionary<Guid, PriceModel> variantPrices,
            string indexingValue)
        {
            Id = id;
            Name = name;
            Description = description;
            FieldTemplateId = fieldTemplateId;
            ProductFields = productFields;
            VariantFields = variantFields;
            VariantPrices = variantPrices;
            IndexingValue = indexingValue;
            CategoryId = categoryId;
        }
    }

    public sealed class AddProductCommandHandler : ICommandHandler<AddProductCommand, Product>
    {
        private readonly IEventStoreRepository<Product> _eventStoreRepository;
        public AddProductCommandHandler(IEventStoreRepository<Product> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }
        public async Task<Product> Handle(AddProductCommand command)
        {
            var product = new Product()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                FieldTemplateId = command.FieldTemplateId
            };

            product.AddProduct(command.Id, command.CategoryId, command.Name, command.Description, command.FieldTemplateId, command.ProductFields, command.IndexingValue);

            foreach (var item in command.VariantFields)
            {
                product.AddVariant(product.Id, item.Key, command.VariantFields[item.Key], command.VariantPrices[item.Key]);
            }

            await _eventStoreRepository.SaveAsync(product);
            return product;
        }
    }
}
