using AutoMapper;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateMemberProfileCommandHandler
{
    internal class UpdateMemberProfileCommandHandler : IUpdateMemberProfileCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _harveyCRMLoyaltyDbContext;

        public UpdateMemberProfileCommandHandler(HarveyCRMLoyaltyDbContext harveyCRMLoyaltyDbContext)
        {
            _harveyCRMLoyaltyDbContext = harveyCRMLoyaltyDbContext;
        }

        public async Task ExecuteAsync(UpdatememberProfileCommand updatememberProfileCommand)
        {
            var profile = await _harveyCRMLoyaltyDbContext.Customers.FirstOrDefaultAsync(f => f.Id == updatememberProfileCommand.UserId);
            if (profile == null)
            {
                throw new EntityNotFoundException("Entity Not Found");
            }

            profile.FirstName = updatememberProfileCommand.FirstName;
            profile.LastName = updatememberProfileCommand.LastName;
            profile.Email = updatememberProfileCommand.Email;
            profile.ProfileImage = updatememberProfileCommand.ProfileImage;
            profile.UpdatedBy = updatememberProfileCommand.UpdatedBy;
            profile.UpdatedDate = DateTime.UtcNow;
            profile.DateOfBirth = updatememberProfileCommand.DateOfBirth != null ? updatememberProfileCommand.DateOfBirth : profile.DateOfBirth;
            profile.Gender = updatememberProfileCommand.Gender != null ? (Data.Gender)(updatememberProfileCommand.Gender) : profile.Gender;

            await _harveyCRMLoyaltyDbContext.SaveChangesAsync();
        }
    }
}
