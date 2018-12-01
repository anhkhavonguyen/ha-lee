using AutoMapper;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
