using System;
using AutoMapper;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Infrastructure.Indexing;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class ProductListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductListModelProfile : Profile
    {
        public ProductListModelProfile()
        {
            CreateMap<Product, ProductListModel>()
                .ConstructUsing(src =>
                {
                    return new ProductListModel()
                    {
                        Id = src.Id,
                        Name = src.Name,
                        Description = src.Description
                    };
                })
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<ProductListModel, CatalogProduct>();
        }
    }
}
