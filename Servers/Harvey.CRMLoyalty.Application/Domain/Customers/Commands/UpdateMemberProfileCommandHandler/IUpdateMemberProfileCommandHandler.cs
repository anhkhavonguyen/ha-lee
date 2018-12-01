using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateMemberProfileCommandHandler
{
    public interface IUpdateMemberProfileCommandHandler
    {
        Task ExecuteAsync(UpdatememberProfileCommand updatememberProfileCommand);
    }
}
