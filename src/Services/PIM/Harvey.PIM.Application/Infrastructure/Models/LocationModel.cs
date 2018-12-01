using AutoMapper;
using Harvey.PIM.Application.Infrastructure.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class LocationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }
    }

    public class LocationProfile: Profile {
        public LocationProfile()
        {
            CreateMap<Location, LocationModel>().ReverseMap();
        }
    }
}
