using System;
using System.Collections.Generic;

namespace Harvey.PIM.MarketingAutomation
{
    public interface IFeedConverter<TSource, out TTarget>
        where TTarget : FeedItemBase
    {
        bool CanConvert(Type type);
        IEnumerable<TTarget> Convert(IEnumerable<TSource> source);
    }
}
