using System;

namespace Harvey.CRMLoyalty.Application.Extensions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
