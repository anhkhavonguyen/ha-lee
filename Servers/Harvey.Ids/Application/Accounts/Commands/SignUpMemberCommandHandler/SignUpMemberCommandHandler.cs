using Harvey.Ids.Domains;
using Harvey.Ids.Models;
using Harvey.Ids.Utils;
using Harvey.Message.Customers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.SignUpMemberCommandHandler
{
    internal class SignUpMemberCommandHandler : ISignUpMemberCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HarveyIdsDbContext _harveyIdsDbContext;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public SignUpMemberCommandHandler(UserManager<ApplicationUser> userManager, HarveyIdsDbContext harveyIdsDbContext, IBus bus, IConfiguration configuration)
        {
            _userManager = userManager;
            _harveyIdsDbContext = harveyIdsDbContext;
            _bus = bus;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(SignUpMemberCommand signUpMemberCommand)
        {
            var shortLink = _harveyIdsDbContext.ShortLinks.FirstOrDefault(f => f.Id == signUpMemberCommand.Code);
            if (shortLink == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }
            SignupMemberDeserializeModel signupMemberDeserializeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SignupMemberDeserializeModel>(shortLink.Value);

            var user = await _userManager.FindByIdAsync(signupMemberDeserializeModel.UserId);
            if (user.EmailConfirmed || user.PhoneNumberConfirmed)
            {
                throw new DataInvalidException("USER_HAS_BEEN_REGISTERED");
            }
            if (!user.IsActive)
            {
                throw new EntityNotFoundException("User Not Found");
            }
            user.FirstName = signUpMemberCommand.FirstName;
            user.LastName = signUpMemberCommand.LastName;
            user.Email = signUpMemberCommand.Email;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, signUpMemberCommand.Password);
            user.DateOfBirth = signUpMemberCommand.DateOfBirth;
            user.Gender = signUpMemberCommand.Gender;
            user.Avatar = signUpMemberCommand.Avatar;
            user.SecurityStamp = Guid.NewGuid().ToString("D");
            await _userManager.UpdateAsync(user);

            ISendEndpoint sendEndpointTask = await _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "update_member_profile_queue")));
            await sendEndpointTask.Send<UpdateMemberProfileCommand>(new
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ProfileImage = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            });
        }
    }
}
