using System;
using System.Collections.Generic;
using Harvey.Polly;

namespace Harvey.PurchaseControl.Application.Infrastructure
{
    public class DataSeedRetrivalPolicy : IRetrivalPolicy
    {
        public int NumbersOfRetrival => 3;

        public RetrivalStategy RetrivalStategy => RetrivalStategy.Exponential;

        public int Delay => 2;

        public List<System.Exception> HandledExceptions => new List<System.Exception>() { new System.Exception() };
    }
}
