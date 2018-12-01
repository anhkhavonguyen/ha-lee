using Harvey.Ids.Configs;
using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Accounts;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.MigrationDataCommandHandler
{
    internal class MigrationDataCommandHandler : IMigrationDataCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;

        public MigrationDataCommandHandler(UserManager<ApplicationUser> userManager,
            HarveyIdsDbContext harveyIdsDbContext, IOptions<ConfigurationRabbitMq> configurationRabbitMq, IBusControl bus)
        {
            _userManager = userManager;
            _harveyIdsDbContext = harveyIdsDbContext;
            _bus = bus;
            _configurationRabbitMq = configurationRabbitMq;
        }

        public string Execute(List<MigrationDataCommand> migrationCommands)
        {
            string message = "";
            var roleId = _harveyIdsDbContext.Roles.FirstOrDefault(a => a.Name == "Member").Id;
           
            if (migrationCommands.Count > 0)
            {
                int itemperLoop = 2000;
                int totalItem = migrationCommands.Count();
                int itemReaded = 0;
                int index = 0;
                while (itemReaded < totalItem)
                {
                    List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
                    List<IdentityUserRole<string>> identityUserRoles = new List<IdentityUserRole<string>>();
                    int skipItem = index * itemperLoop;
                    int takeItem = totalItem - (skipItem + itemperLoop) > 0 ? itemperLoop : totalItem - skipItem;
                    itemReaded = skipItem + takeItem;
                    foreach (var item in migrationCommands.Skip(skipItem).Take(takeItem))
                    {
                        var applicationUser = new ApplicationUser();
                        applicationUser.UserName = item.FullPhoneNumber.Split(" ")[0].Replace("+", "").Trim() + item.FullPhoneNumber.Split(" ")[1].Trim();
                        applicationUser.PhoneNumber = item.FullPhoneNumber.Split(" ")[1];
                        applicationUser.PhoneCountryCode = item.FullPhoneNumber.Split(" ")[0].Replace("+", "");
                        applicationUser.PhoneNumberConfirmed = true;
                        applicationUser.SecurityStamp = Guid.NewGuid().ToString("D");
                        applicationUser.Email = item.Email;
                        applicationUser.EmailConfirmed = true;
                        applicationUser.FirstName = item.FirstName.Replace(";", ",");
                        applicationUser.LastName = item.LastName.Replace(";", ",");
                        applicationUser.IsActive = true;
                        applicationUser.Pin = StringExtension.GeneratePIN();
                        applicationUser.IsMigrateData = true;
                        applicationUser.Id = item.CustomerId;
                        applicationUser.UserType = Data.UserType.Member;
                        if (applicationUsers.Any(a=>a.Id == item.CustomerId))
                        {
                            message += $"The ID: {item.CustomerId} is duplicate";
                            continue;
                        }
                        if (applicationUsers.Any(a => a.PhoneNumber == applicationUser.PhoneNumber))
                        {
                            message = $"The phone number at item Id {item.CustomerId} is already exist \r\n";
                            continue;
                        }
                        applicationUsers.Add(applicationUser);

                        var userRole = new IdentityUserRole<string>();
                        userRole.RoleId = roleId;
                        userRole.UserId = item.CustomerId;

                        identityUserRoles.Add(userRole);
                    }
                    _harveyIdsDbContext.Users.AddRange(applicationUsers);
                    _harveyIdsDbContext.UserRoles.AddRange(identityUserRoles);

                    _harveyIdsDbContext.SaveChanges();
                    index++;
                }
            }
            return message;
        }

        public async Task<int> ExecuteAsync()
        {
            var updatedUsers = _harveyIdsDbContext.Users.Where(u => u.Gender.HasValue)
                .Select(c => new UpdatedApplicationUser
                {
                    Id = c.Id,
                    Gender = (int)c.Gender
                });

            //Fire message to CRM update customer phone
            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "update_gender_value_queue")));
            await sendEndpointTask.Send<UpdateGenderValueCommandMessage>(new
            {
                updatedApplicationUsers = updatedUsers
            });

            return 1;
        }

    }
}
