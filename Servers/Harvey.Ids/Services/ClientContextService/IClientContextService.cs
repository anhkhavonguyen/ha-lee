using System.Threading.Tasks;

namespace Harvey.Ids.Services.ClientContextService
{
    public interface IClientContextService
    {
        Task<IdentityServer4.EntityFramework.Entities.Client> GetClientContextAsync(string returnUrl);
    }
}
