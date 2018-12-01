using System;

namespace Harvey.EventBus.Publishers
{
    public class ProductPublisher : IPublisher
    {
        public string Name => "pim_product_operation";

        public Guid? CorrelationId { get; set; }
    }
}
