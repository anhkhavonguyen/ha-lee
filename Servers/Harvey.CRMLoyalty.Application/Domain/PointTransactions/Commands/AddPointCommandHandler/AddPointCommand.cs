using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddPointCommandHandler
{
    public class AddPointCommand: BaseRequest
    {
        public string CustomerId { get; set; }
        public decimal Value { get; set; }
        public string OutletId { get; set; }
        public string IpAddress { get; set; }
        public string Comment { get; set; }
    }
}
