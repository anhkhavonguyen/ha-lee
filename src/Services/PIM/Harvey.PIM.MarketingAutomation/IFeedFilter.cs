using System;
using System.Collections.Generic;

namespace Harvey.PIM.MarketingAutomation
{
    public interface IFeedFilter<TModel>
    {
        IEnumerable<TModel> Filter(Guid CorrelationId, IEnumerable<TModel> source);
    }
}
