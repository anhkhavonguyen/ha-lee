using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateMemberProfileCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.Customers
{
    public class UpdateMemberProfileConsumer : IConsumer<UpdateMemberProfileCommand>
    {
        private readonly IUpdateMemberProfileCommandHandler _updateMemberProfileCommandHandler;

        public UpdateMemberProfileConsumer(IUpdateMemberProfileCommandHandler updateMemberProfileCommandHandler)
        {
            _updateMemberProfileCommandHandler = updateMemberProfileCommandHandler;
        }

        public async Task Consume(ConsumeContext<UpdateMemberProfileCommand> context)
        {
            var message = context.Message;
            await _updateMemberProfileCommandHandler.ExecuteAsync(new UpdatememberProfileCommand
            {
                Email = message.Email,
                FirstName = message.FirstName,
                LastName = message.LastName,
                ProfileImage = message.ProfileImage,
                UserId = message.UserId,
                UpdatedBy = message.UpdatedBy,
                DateOfBirth = message.DateOfBirth,
                Gender = message.Gender
            });
        }
    }
}
