using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Exception
{
    public class BadModelException: ProductionException
    {
        public BadModelException()
        {
        }

        public BadModelException(string message) :
            base(message)
        {
        }

        public BadModelException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
