using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Exception
{
    public abstract class ProductionException: System.Exception
    {
        public ProductionException()
        {
        }

        public ProductionException(string message) :
            base(message)
        {
        }

        public ProductionException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
