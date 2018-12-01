using AutoMapper;
using Harvey.Activity.Application.Entities;
using Harvey.Activity.Application.Model;

namespace Harvey.Activity.Application
{
    public static class MappingConfiguration
    {
        public static void Execute()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ActionActivity, ActionActivityModel>();
            });
        }
    }
}
