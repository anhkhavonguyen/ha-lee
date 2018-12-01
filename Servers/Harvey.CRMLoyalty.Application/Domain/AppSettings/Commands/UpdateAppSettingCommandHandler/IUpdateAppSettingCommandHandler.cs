using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.UpdateAppSettingCommandHandler
{
    public interface IUpdateAppSettingCommandHandler
    {
        Task<string> ExecuteAsync(UpdateAppSettingCommand command);
    }
}
