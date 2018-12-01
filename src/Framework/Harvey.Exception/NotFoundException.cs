using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Exception
{
    public class NotFoundException: ProductionException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) :
            base(message)
        {
        }

        public NotFoundException(string message, System.Exception inner) :
            base(message, inner)
        {
        }
    }
}
