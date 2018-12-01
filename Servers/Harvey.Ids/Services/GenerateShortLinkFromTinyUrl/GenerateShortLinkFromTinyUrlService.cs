using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.GenerateShortLinkFromTinyUrl
{
    internal class GenerateShortLinkFromTinyUrlService : IGenerateShortLinkFromTinyUrlService
    {
        public async Task<string> ExecuteAsync(string fullUrl)
        {
            string url = fullUrl;
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetAsync(string.Format("https://tinyurl.com/api-create.php?url={0}", fullUrl));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                url = RemoveHttpProtocol(await result.Content.ReadAsStringAsync());
            }
            return url;
        }

        private string RemoveHttpProtocol(string url)
        {
            return url.Replace("https://", "").Replace("http://", "");
        }
    }
}
