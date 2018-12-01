using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Data
{
    public class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            try
            {
                Policy.Handle<Exception>()
               .WaitAndRetry(new[] {
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10)
               }, (exception, timeSpan, retryCount, context) =>
               {

               }).Execute(() =>
               {
                   var harveyNotificationDbContext = serviceProvider.GetRequiredService<HarveyNotificationDbContext>();
                   harveyNotificationDbContext.Database.Migrate();

                   SeedNotificationTypes(harveyNotificationDbContext);
                   SeedTemplates(harveyNotificationDbContext);
                   SeedStatus(harveyNotificationDbContext);
                   SeedErrorSourceAsync(harveyNotificationDbContext).Wait();
               });
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }


        private static void SeedNotificationTypes(HarveyNotificationDbContext context)
        {
            var notificationTypes = NotificationTypeConfig.GetNotificationTypes();
            foreach (var notificationType in notificationTypes)
            {
                var notificationTypeEntity = context.NotificationTypes.Where(w => w.TypeName == notificationType.TypeName).FirstOrDefault();
                if (notificationTypeEntity == null)
                {
                    context.NotificationTypes.Add(notificationType);
                }
            }
            context.SaveChanges();
        }

        private static void SeedTemplates(HarveyNotificationDbContext context)
        {
            var templates = TemplateConfig.GetTemplates();
            foreach (var template in templates)
            {
                var templateEntity = context.Templates.FirstOrDefault(w => w.TemplateKey == template.TemplateKey);
                if (templateEntity == null)
                {
                    context.Templates.Add(template);
                }
                else {
                    templateEntity.Content = template.Content;
                    templateEntity.Title = template.Title;
                    templateEntity.DisplayName = template.DisplayName;
                }
            }
            context.SaveChanges();
        }

        private static void SeedStatus(HarveyNotificationDbContext context)
        {
            var status = NotificationStatusConfig.GetNotificationStatus();
            foreach(var st in status)
            {
                var stEntity = context.NotificationStatus.Where(w => w.Id == st.Id).FirstOrDefault();
                if (stEntity == null)
                {
                    context.NotificationStatus.Add(st);
                }
            }
            context.SaveChanges();
        }

        #region Log Error Source
        private static async Task SeedErrorSourceAsync(HarveyNotificationDbContext context)
        {
            if (context.ErrorLogSources.Any())
                return;

            var errorLogSources = new List<ErrorLogSource>()
            {
                new ErrorLogSource
                {
                    Id = 1,
                    SourceName = "AdminApp"
                },
                new ErrorLogSource
                {
                    Id =2,
                    SourceName = "MemberApp"
                },
                new ErrorLogSource
                {
                    Id =3,
                    SourceName = "StoreApp"
                },
                new ErrorLogSource
                {
                    Id = 4,
                    SourceName = "BackEnd"
                },
                new ErrorLogSource
                {
                    Id = 5,
                    SourceName = "FrontEnd"
                }
            };
            context.AddRange(errorLogSources);
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
