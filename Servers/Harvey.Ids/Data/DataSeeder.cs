using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Data
{
    public class DataSeeder
    {
        internal static void Seed(IServiceProvider serviceProvider)
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
                    var HarveyIdsDbContext = serviceProvider.GetRequiredService<HarveyIdsDbContext>();
                    HarveyIdsDbContext.Database.Migrate();
                    SeedUsersAndRolesAsync(serviceProvider).Wait();

                    var configurationDbContext = serviceProvider.GetRequiredService<ConfigurationDbContext>();
                    configurationDbContext.Database.Migrate();

                    var persistedGrantDbContext = serviceProvider.GetRequiredService<PersistedGrantDbContext>();
                    persistedGrantDbContext.Database.Migrate();

                    SeedConfigurationData(configurationDbContext);
                });
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        private static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<HarveyIdsDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string[] roles = { "Administrator", "Staff", "Member", "AdminStaff", "Purchaser", "InventoryManager", "WarehouseKeeper", "SalesManager" };
            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    var applicationRole = new ApplicationRole(role);
                    await roleManager.CreateAsync(applicationRole);
                }
            }

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "53b1fc43-933c-4fb5-b855-408b87fca6d9",
                    FirstName = "CRM",
                    LastName = "Admin",
                    UserName = "administrator@gmail.com",
                    Email = "administrator@gmail.com",
                    PhoneNumber = "0934614xx9",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Admin,
                },
                new ApplicationUser
                {
                    Id = "02978172-86d3-42e9-a336-7353297568ec",
                    FirstName = "Tog",
                    LastName = "Admin",
                    UserName = "admin@tog.com.sg",
                    Email = "admin@tog.com.sg",
                    PhoneNumber = "0934614xx9",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Admin,
                },
                new ApplicationUser
                {
                    Id = "69ee2181-777e-4779-be5d-d4142d44406e",
                    FirstName = "Gan",
                    LastName = "Leong",
                    UserName = "gan.leong@toyorgame.com.sg",
                    Email = "gan.leong@toyorgame.com.sg",
                    PhoneNumber = "0934614xx9",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Admin,
                },
                new ApplicationUser
                {
                    Id = "f6e021af-a6a0-4039-83f4-152595b4671a",
                    FirstName = "TOG",
                    LastName = "Bugis+",
                    UserName = "bgp@tog.com.sg",
                    Email = "bgp@tog.com.sg",
                    PhoneNumber = "6634 2637",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },

                new ApplicationUser
                {
                    Id = "1f552daf-b628-4ab8-8294-be535af7ed7e",
                    FirstName = "TOG",
                    LastName = "Tiong Bahru Plaza",
                    UserName = "tb@tog.com.sg",
                    Email = "tb@tog.com.sg",
                    PhoneNumber = "6352 2672",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "d2d6fd97-fedc-4298-8c3c-464e68d06193",
                    FirstName = "TOG",
                    LastName = "WaterWay Point",
                    UserName = "wp@tog.com.sg",
                    Email = "wp@tog.com.sg",
                    PhoneNumber = "6385 9172",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "e17c1c0d-0da7-49fe-9556-1f1a286a607e",
                    FirstName = "TOG",
                    LastName = "Jurong Point Shopping Centre",
                    UserName = "jp@tog.com.sg",
                    Email = "jp@tog.com.sg",
                    PhoneNumber = "6316 5984",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "065edde7-5620-48c7-9060-7fbddfe4c2c6",
                    FirstName = "TOG",
                    LastName = "Lot 1 Shopper's Mall",
                    UserName = "lt1@tog.com.sg",
                    Email = "lt1@tog.com.sg",
                    PhoneNumber = "6769 6129",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "b157896b-c807-4a09-9dce-f482e0fde6e3",
                    FirstName = "TOG",
                    LastName = "Clementi Mall",
                    UserName = "cm@tog.com.sg",
                    Email = "cm@tog.com.sg",
                    PhoneNumber = "6570 6084",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "e96a19b7-e59f-41f4-9eb2-32d38d19adea",
                    FirstName = "TOG",
                    LastName = "Plaza Singapura",
                    UserName = "ps@tog.com.sg",
                    Email = "ps@tog.com.sg",
                    PhoneNumber = "6238 7632",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "4fdce97f-207c-42ad-879f-87b2fee87aec",
                    FirstName = "",
                    LastName = "Causeway Point Shopping Centre",
                    UserName = "cw@tog.com.sg",
                    Email = "cw@tog.com.sg",
                    PhoneNumber = "0934614xx9",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "aa156fcb-250c-4f23-b661-c8c858bfe94d",
                    FirstName = "TOG",
                    LastName = "JEM",
                    UserName = "jm@tog.com.sg",
                    Email = "jm@tog.com.sg",
                    PhoneNumber = "6334 6753",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "3437cbff-cc72-4f25-b0d5-cf46f2d066c6",
                    FirstName = "TOG",
                    LastName = "Tampines Mall",
                    UserName = "tm@tog.com.sg",
                    Email = "tm@tog.com.sg",
                    PhoneNumber = "6538 0144",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.Staff,
                },
                new ApplicationUser
                {
                    Id = "ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    FirstName = "Eleen",
                    LastName = "Tan",
                    UserName = "eleen.tan@toyorgame.com.sg",
                    Email = "eleen.tan@toyorgame.com.sg",
                    PhoneNumber = "xxxxx357",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.AdminStaff,
                },
                new ApplicationUser
                {
                    Id = "cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    FirstName = "Adeline",
                    LastName = "Ngu",
                    UserName = "adeline.ngu@toyorgame.com.sg",
                    Email = "adeline.ngu@toyorgame.com.sg",
                    PhoneNumber = "xxxxx358",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.AdminStaff,
                },
                new ApplicationUser
                {
                    Id = "6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    FirstName = "Hiew",
                    LastName = "Yuhang",
                    UserName = "hiew.yuhang@toyorgame.com.sg",
                    Email = "hiew.yuhang@toyorgame.com.sg",
                    PhoneNumber = "xxxxx359",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.AdminStaff,
                },
                new ApplicationUser
                {
                    Id = "4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    FirstName = "Tech",
                    LastName = "Admin",
                    UserName = "tech.admin@toyorgame.com.sg",
                    Email = "tech.admin@toyorgame.com.sg",
                    PhoneNumber = "xxxxx359",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserType = UserType.AdminStaff,
                }
            };
            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "123456");
                    var userStore = new UserStore<ApplicationUser>(context);
                    await userStore.CreateAsync(user);
                    var userFromDb = await userManager.FindByIdAsync(user.Id);
                    var listRoles = new List<string>();
                    if (user.UserType == UserType.Admin)
                    {
                        listRoles.Add("Administrator");
                    }
                    if (user.UserType == UserType.Staff)
                    {
                        listRoles.Add("Staff");
                    }
                    if (user.UserType == UserType.Member)
                    {
                        listRoles.Add("Member");
                    }
                    if (user.UserType == UserType.AdminStaff)
                    {
                        listRoles.Add("AdminStaff");
                    }
                    if (user.Id == "69ee2181-777e-4779-be5d-d4142d44406e")
                    {
                        listRoles.Add("AdminStaff");
                    }
                    await userManager.AddToRolesAsync(userFromDb, listRoles);
                }
            }
        }

        private static void SeedClientsConfig(ConfigurationDbContext context)
        {
            foreach (var client in ClientsConfig.GetClients().ToList())
            {
                var clientEntity = context.Clients.Where(c => c.ClientId == client.ClientId).Include(p => p.Properties).FirstOrDefault();
                if (clientEntity == null)
                {
                    Console.WriteLine($"Client {client.ClientId} added");
                    context.Clients.Add(client.ToEntity());
                }
                else
                {
                    //Check if any client property is missing in DB then do insert
                    foreach (var item in client.Properties)
                    {
                        if (clientEntity.Properties.All(p => p.Key != item.Key))
                        {
                            clientEntity.Properties.Add(new ClientProperty
                            {
                                Key = item.Key,
                                Value = item.Value
                            });
                        }
                    }
                }
            }
            context.SaveChanges();
        }
        private static void SeedResourcesConfig(ConfigurationDbContext context)
        {
            var allResources = ResourcesConfig.GetIdentityResources().ToList();
            var allResourceNames = allResources.Select(p => p.Name).ToList();
            var existedResources = context.IdentityResources.Where(p => allResourceNames.Contains(p.Name));
            var existedResourceNames = existedResources.Select(p => p.Name).ToList();
            context.IdentityResources.AddRange(allResources.Where(p => !existedResourceNames.Contains(p.Name)).Select(p => p.ToEntity()));
            context.SaveChanges();
        }
        private static void SeedApiResources(ConfigurationDbContext context)
        {
            var allApiResources = ResourcesConfig.GetApiResources().ToList();
            var allApiResourceNames = allApiResources.Select(p => p.Name).ToList();
            var existedApiResources = context.ApiResources.Where(p => allApiResourceNames.Contains(p.Name));
            var existedApiResourceNames = existedApiResources.Select(p => p.Name).ToList();
            context.ApiResources.AddRange(allApiResources.Where(p => !existedApiResourceNames.Contains(p.Name)).Select(p => p.ToEntity()));
            context.SaveChanges();
        }

        private static void SeedConfigurationData(ConfigurationDbContext context)
        {
            SeedClientsConfig(context);
            SeedResourcesConfig(context);
            SeedApiResources(context);
        }
    }
}
