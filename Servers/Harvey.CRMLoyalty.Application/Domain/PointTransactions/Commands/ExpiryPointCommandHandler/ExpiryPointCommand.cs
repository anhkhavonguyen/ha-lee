using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ExpiryPointCommandHandler
{
    public class ExpiryPointCommand: BaseRequest
    {
        public System.DateTime Date { get; set; }
    }
}
