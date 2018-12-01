using AutoMapper;
using Harvey.Notification.Application.Models;

namespace Harvey.Notification.Application
{
    public static class MappingConfiguration
    {
        public static void Execute()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Notification, NotificationModel>();

            });
        }
    }
}
