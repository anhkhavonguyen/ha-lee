using Harvey.Ids.Domains;
using Harvey.Ids.Utils;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler
{
    internal class UpdateInfoMemberAccountCommanHandler : IUpdateInfoMemberAccountCommanHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IBusControl _bus;

        public UpdateInfoMemberAccountCommanHandler(UserManager<ApplicationUser> userManager,
             IBusControl bus,
             IConfiguration configuration)
        {
            _userManager = userManager;
            _bus = bus;
            _configuration = configuration;
        }


        public async Task ExecuteAsync(UpdateInfoMemberAccountCommand updateInfoMemberAccountCommand)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == updateInfoMemberAccountCommand.Id && x.IsActive);
            if(user == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }
            user.FirstName = updateInfoMemberAccountCommand.FirstName;
            user.LastName = updateInfoMemberAccountCommand.LastName;
            user.Email = updateInfoMemberAccountCommand.Email;
            user.Gender = updateInfoMemberAccountCommand.Gender;
            user.Avatar = updateInfoMemberAccountCommand.Avatar;
            user.ZipCode = updateInfoMemberAccountCommand.ZipCode;
            user.DateOfBirth = updateInfoMemberAccountCommand.DateOfBirth;
            
            await _userManager.UpdateAsync(user);

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "update_member_profile_queue")));
            await sendEndpointTask.Send<UpdateMemberProfileCommand>(new
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImage = user.Avatar,
                UpdatedBy = updateInfoMemberAccountCommand.CurrentUserId,
                user.DateOfBirth,
                Gender = user.Gender
            });
        }
    }
}
