using System;

namespace Harvey.Ids.Utils
{
    public class DataInvalidException : Exception
    {
        public DataInvalidException(string message) : base(message)
        {
        }
    }
}
