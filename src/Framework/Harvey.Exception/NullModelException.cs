using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Exception
{
    public class NullModelException: ProductionException
    {
        public NullModelException()
        {
        }

        public NullModelException(string message) :
            base(message)
        {
        }

        public NullModelException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
