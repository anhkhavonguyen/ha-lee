using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Exception
{
    public class ForBiddenException: ProductionException
    {
        public ForBiddenException()
        {
        }

        public ForBiddenException(string message) :
            base(message)
        {
        }

        public ForBiddenException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
