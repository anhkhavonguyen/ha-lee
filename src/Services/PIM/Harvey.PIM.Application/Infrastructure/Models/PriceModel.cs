using AutoMapper;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class PriceModel
    {
        public Guid Id { get; set; }
        public float ListPrice { get; set; }
        public float StaffPrice { get; set; }
        public float MemberPrice { get; set; }
    }

    public class PriceModelProfile : Profile
    {
        public PriceModelProfile()
        {
            CreateMap<Price, PriceModel>().ReverseMap();
        }
    }
}
