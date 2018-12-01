using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.VoidPointCommandHandler
{
    public class VoidPointCommand : BaseRequest
    {
        public string PointTransactionId { get; set; }

        public string IpAddress { get; set; }

        public string VoidByName { get; set; }
    }
}
