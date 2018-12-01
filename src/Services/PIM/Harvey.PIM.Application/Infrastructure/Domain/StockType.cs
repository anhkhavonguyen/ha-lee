using Harvey.Domain;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class StockType : EntityBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
