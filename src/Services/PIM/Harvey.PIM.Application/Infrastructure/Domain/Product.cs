using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.Domain;
using Harvey.EventBus.Events.FileldValues;
using Harvey.EventBus.Events.Prices;
using Harvey.EventBus.Events.Products;
using Harvey.EventBus.Events.Variants;
using Harvey.PIM.Application.Events.Products;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Models;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Product : AggregateRootBase, IAuditable
    {
        public Dictionary<SectionModel, List<FieldValue>> Sections = new Dictionary<SectionModel, List<FieldValue>>();
        public Dictionary<Guid, List<FieldValue>> VariantFieldValues = new Dictionary<Guid, List<FieldValue>>();
        public Dictionary<Guid, PriceModel> VariantPrices = new Dictionary<Guid, PriceModel>();
        public const string IndexName = "product_index";
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid FieldTemplateId { get; set; }
        public FieldTemplate FieldTemplate { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public void AddProduct(Guid id, Guid categoryId, string name, string description, Guid fieldTemplateId, List<DetailFieldValueModel> fields, string indexingValue)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"id is required.");
            }

            RaiseEvent(new ProductCreatedEvent(id.ToString())
            {
                CategoryId = categoryId,
                Name = name,
                Description = description,
                FieldTemplateId = fieldTemplateId,
                IndexName = IndexName,
                IndexingValue = indexingValue
            });

            foreach (var item in fields)
            {
                RaiseEvent(new FieldValueCreatedEvent(id.ToString())
                {
                    FieldValueId = item.Id,
                    EntityId = id,
                    FieldId = item.FieldId,
                    FieldValue = item.FieldValue,
                    FieldType = item.FieldType,
                    Section = item.Section,
                    OrderSection = item.OrderSection,
                    IsVariantField = item.IsVariantField,
                    FieldName = item.FieldName
                });
            }
        }

        public void UpdateProduct(Guid id, Guid categoryId, string name, string description, List<DetailFieldValueModel> fields, string indexingValue)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"id is required.");
            }

            RaiseEvent(new ProductUpdatedEvent(id.ToString())
            {
                CategoryId = categoryId,
                Name = name,
                Description = description,
                IndexName = IndexName,
                IndexingValue = indexingValue
            });

            foreach (var item in fields)
            {
                RaiseEvent(new FieldValueUpdatedEvent(id.ToString())
                {
                    FieldValueId = item.Id,
                    EntityId = id,
                    FieldId = item.FieldId,
                    FieldValue = item.FieldValue,
                    FieldType = item.FieldType,
                    Section = item.Section,
                    OrderSection = item.OrderSection,
                    IsVariantField = item.IsVariantField,
                    FieldName = item.FieldName
                });
            }
        }

        public void AddVariant(Guid productId, Guid variantId, List<DetailFieldValueModel> fields, PriceModel priceModel)
        {
            if (productId == null)
            {
                throw new ArgumentNullException($"product id is required.");
            }

            if (variantId == null)
            {
                throw new ArgumentNullException($"variant id is required.");
            }

            RaiseEvent(new VariantCreatedEvent(productId.ToString())
            {
                ProductId = productId,
                VariantId = variantId
            });

            foreach (var item in fields)
            {
                RaiseEvent(new FieldValueCreatedEvent(productId.ToString())
                {
                    FieldValueId = item.Id,
                    EntityId = variantId,
                    FieldId = item.FieldId,
                    FieldValue = item.FieldValue,
                    FieldType = item.FieldType,
                    Section = item.Section,
                    OrderSection = item.OrderSection,
                    IsVariantField = item.IsVariantField,
                    FieldName = item.FieldName
                });
            }

            RaiseEvent(new PriceCreatedEvent(productId.ToString())
            {
                PriceId = Guid.NewGuid(),
                VariantId = variantId,
                ListPrice = priceModel.ListPrice,
                MemberPrice = priceModel.MemberPrice,
                StaffPrice = priceModel.StaffPrice,
            });
        }

        public void UpdateVariant(Guid productId, Guid variantId, List<DetailFieldValueModel> fields, PriceModel priceModel)
        {
            if (productId == null)
            {
                throw new ArgumentNullException($"product id is required.");
            }

            if (variantId == null)
            {
                throw new ArgumentNullException($"variant id is required.");
            }

            if (!VariantFieldValues.Keys.Any(x => x == variantId))
            {
                VariantFieldValues.Add(variantId, new List<FieldValue>());
            }

            foreach (var item in fields)
            {
                RaiseEvent(new FieldValueUpdatedEvent(productId.ToString())
                {
                    FieldValueId = item.Id,
                    EntityId = variantId,
                    FieldId = item.FieldId,
                    FieldValue = item.FieldValue,
                    FieldType = item.FieldType,
                    Section = item.Section,
                    OrderSection = item.OrderSection,
                    IsVariantField = item.IsVariantField,
                    FieldName = item.FieldName
                });
            }
            //TODO compare to current price, if not change don't fire this event
            RaiseEvent(new PriceCreatedEvent(productId.ToString())
            {
                PriceId = Guid.NewGuid(),
                VariantId = variantId,
                ListPrice = priceModel.ListPrice,
                MemberPrice = priceModel.MemberPrice,
                StaffPrice = priceModel.StaffPrice,
            });
        }

        public void Apply(ProductCreatedEvent @event)
        {
            Id = Guid.Parse(@event.AggregateId);
            Name = @event.Name;
            CategoryId = @event.CategoryId;
            Description = @event.Description;
            FieldTemplateId = @event.FieldTemplateId;
        }

        public void Apply(ProductUpdatedEvent @event)
        {
            Id = Guid.Parse(@event.AggregateId);
            Name = @event.Name;
            Description = @event.Description;
            CategoryId = @event.CategoryId;
        }

        public void Apply(VariantCreatedEvent @event)
        {
            if (VariantFieldValues.Keys.Any(x => x == @event.VariantId))
            {
                return;
            }
            VariantFieldValues.Add(@event.VariantId, new List<FieldValue>());
        }

        public void Apply(FieldValueCreatedEvent @event)
        {
            var fieldValue = FieldValueFactory.CreateFromFieldType(@event.FieldType, @event.FieldValue);
            fieldValue.Id = @event.FieldValueId;
            fieldValue.FieldId = @event.FieldId;
            fieldValue.EntityId = @event.EntityId;
            fieldValue.Field = new Field()
            {
                Id = @event.FieldId,
                Type = @event.FieldType,
                Name = @event.FieldName
            };
            if (!@event.IsVariantField)
            {
                bool shouldAddNewSection = true;

                foreach (var item in Sections.Keys)
                {
                    if (item.Name == @event.Section)
                    {
                        Sections[item].Add(fieldValue);
                        shouldAddNewSection = false;
                        break;
                    }
                }

                if (shouldAddNewSection)
                {
                    Sections.Add(new SectionModel()
                    {
                        IsVariantField = @event.IsVariantField,
                        Name = @event.Section,
                        OrderSection = @event.OrderSection
                    }, new List<FieldValue>() { fieldValue });
                }
            }
            else
            {
                VariantFieldValues[@event.EntityId].Add(fieldValue);
            }
        }

        public void Apply(FieldValueUpdatedEvent @event)
        {
            var fieldValue = FieldValueFactory.CreateFromFieldType(@event.FieldType, @event.FieldValue);
            fieldValue.Id = @event.FieldValueId;
            fieldValue.FieldId = @event.FieldId;
            fieldValue.EntityId = @event.EntityId;
            fieldValue.Field = new Field()
            {
                Id = @event.FieldId,
                Type = @event.FieldType,
                Name = @event.FieldName
            };
            if (!@event.IsVariantField)
            {
                var keys = Sections.Keys.Select(x => x).ToList();
                foreach (var item in keys)
                {
                    Sections[item] = Sections[item].Where(x => x.Id != @event.FieldValueId).ToList();
                    Sections[item].Add(fieldValue);
                }
            }
            else
            {
                VariantFieldValues[@event.EntityId] = VariantFieldValues[@event.EntityId].Where(x => x.Id != fieldValue.Id).ToList();
                VariantFieldValues[@event.EntityId].Add(fieldValue);
            }
        }

        public void Apply(PriceCreatedEvent @event)
        {
            if (VariantPrices.Keys.Any(x => x == @event.VariantId))
            {
                VariantPrices[@event.VariantId] = new PriceModel()
                {
                    Id = @event.PriceId,
                    ListPrice = @event.ListPrice,
                    MemberPrice = @event.MemberPrice,
                    StaffPrice = @event.StaffPrice
                };
                return;
            }
            VariantPrices.Add(@event.VariantId, new PriceModel()
            {
                Id = @event.PriceId,
                ListPrice = @event.ListPrice,
                MemberPrice = @event.MemberPrice,
                StaffPrice = @event.StaffPrice
            });
        }
    }
}
