using System;

namespace Harvey.Domain
{
    public abstract class EntityBase
    {
        public EntityBase()
        {

        }
        public Guid Id { get; set; }
    }
}
