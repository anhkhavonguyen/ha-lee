using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.MigrationDataCommandHandler;
using Harvey.Message.Customers;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.Customers
{
    public class UpdateGenderValueConsumer : IConsumer<UpdateGenderValueCommandMessage>
    {
        private readonly IMigrationDataCommandHandler _migrationDataCommandHandler;
        public UpdateGenderValueConsumer(IMigrationDataCommandHandler migrationDataCommandHandler)
        {
            _migrationDataCommandHandler = migrationDataCommandHandler;
        }

        public async Task Consume(ConsumeContext<UpdateGenderValueCommandMessage> context)
        {
            var message = context.Message;
            await _migrationDataCommandHandler.ExcuteAsync(message.updatedApplicationUsers);
        }
    }
}
