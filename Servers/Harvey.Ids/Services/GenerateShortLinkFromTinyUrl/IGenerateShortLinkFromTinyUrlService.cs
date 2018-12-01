using System.Threading.Tasks;

namespace Harvey.Ids.Services.GenerateShortLinkFromTinyUrl
{
    public interface IGenerateShortLinkFromTinyUrlService
    {
        Task<string> ExecuteAsync(string fullUrl);
    }
}
