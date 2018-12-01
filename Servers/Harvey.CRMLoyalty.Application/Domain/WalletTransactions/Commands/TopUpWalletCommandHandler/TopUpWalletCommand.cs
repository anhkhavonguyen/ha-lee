using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.TopUpWalletCommandHandler
{
    public class TopUpWalletCommand: BaseRequest
    {
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public decimal Value { get; set; }
        public string IpAddress { get; set; }

        public string Comment { get; set; }
    }
}
