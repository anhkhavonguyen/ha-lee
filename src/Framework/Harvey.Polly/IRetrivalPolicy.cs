using System;
using System.Collections.Generic;

namespace Harvey.Polly
{
    public interface IRetrivalPolicy
    {
        int NumbersOfRetrival { get; }
        RetrivalStategy RetrivalStategy { get; }
        int Delay { get; }
        List<Exception> HandledExceptions { get; }
    }
}
