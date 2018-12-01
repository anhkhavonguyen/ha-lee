using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class GIWDocument : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
