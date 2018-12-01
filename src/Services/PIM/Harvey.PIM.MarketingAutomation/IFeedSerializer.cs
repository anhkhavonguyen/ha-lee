using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation
{
    public interface IFeedSerializer<TTarget>
        where TTarget : FeedItemBase
    {
        Task SerializeAsync(IEnumerable<TTarget> feedItems);
    }
}
