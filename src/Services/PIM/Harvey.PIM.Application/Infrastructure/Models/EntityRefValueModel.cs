using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class EntityRefValueModel
    {
        public Guid Id { get; }
        public string Value { get; }

        public EntityRefValueModel(Guid id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
