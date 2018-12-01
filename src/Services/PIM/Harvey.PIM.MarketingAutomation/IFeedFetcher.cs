using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation
{
    public interface IFeedFetcher<TModel>
    {
        Task<IEnumerable<TModel>> FetchAsync();
    }
}
