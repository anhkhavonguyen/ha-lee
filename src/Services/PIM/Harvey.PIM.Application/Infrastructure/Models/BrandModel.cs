using AutoMapper;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class BrandModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandModel>().ReverseMap();
        }
    }
}
