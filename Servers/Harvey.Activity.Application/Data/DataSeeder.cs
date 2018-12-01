using Harvey.Activity.Api;
using Harvey.Activity.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Activity.Application.Data
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
                   var harveyActivityDbContext = serviceProvider.GetRequiredService<HarveyActivityDbContext>();
                   harveyActivityDbContext.Database.Migrate();
                   SeedAreaActivityAsync(harveyActivityDbContext).Wait();
                   SeedActionTypeAsync(harveyActivityDbContext).Wait();
                   SeedErrorSourceAsync(harveyActivityDbContext).Wait();
               });
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        #region App Setting Type
        private static async Task SeedAreaActivityAsync(HarveyActivityDbContext context)
        {
            if (context.AreaActivities.Any())
                return;

            var areaActivities = new List<AreaActivity>()
            {
                new AreaActivity
                {
                    Id = "0",
                    AreaPath = "StoreApp"
                },
                new AreaActivity
                {
                    Id = "1",
                    AreaPath = "MemberApp"
                },
                new AreaActivity
                {
                    Id = "2",
                    AreaPath = "AdminApp"
                },
                new AreaActivity
                {
                    Id = "3",
                    AreaPath = "Job"
                },

            };
            context.AddRange(areaActivities);
            await context.SaveChangesAsync();
        }
        #endregion


        #region App Action Type
        private static async Task SeedActionTypeAsync(HarveyActivityDbContext context)
        {
            if (context.ActionTypies.Any())
                return;

            var actionTypes = new List<Entities.ActionType>()
            {
                new Entities.ActionType
                {
                    Id = "0",
                    Name = "AddPoint"
                },
                new Entities.ActionType
                {
                    Id = "1",
                    Name = "RedeemPoint"
                },
                new Entities.ActionType
                {
                    Id = "2",
                    Name = "TopUp"
                },
                new Entities.ActionType
                {
                    Id = "3",
                    Name = "Spending"
                },
                new Entities.ActionType
                {
                    Id = "4",
                    Name = "Void"
                },
                new Entities.ActionType
                {
                    Id = "5",
                    Name = "ExpiryPoint"
                },
                new Entities.ActionType
                {
                    Id = "6",
                    Name = "UpdateAppSetting"
                },
                new Entities.ActionType
                {
                    Id = "7",
                    Name = "InitCustomer"
                },
                new Entities.ActionType
                {
                    Id = "8",
                    Name = "LogInStoreApp"
                },
                new Entities.ActionType
                {
                    Id = "9",
                    Name = "LogInAdminApp"
                },
                new Entities.ActionType
                {
                    Id = "10",
                    Name = "LogInMemberApp"
                },
                new Entities.ActionType
                {
                    Id = "11",
                    Name = "ChangePhoneNumber"
                },
            };
            context.AddRange(actionTypes);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Log Error Source
        private static async Task SeedErrorSourceAsync(HarveyActivityDbContext context)
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
