using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.SpendingWalletCommandHandler
{
    public class SpendingWalletCommand: BaseRequest
    {
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public decimal Value { get; set; }
        public string IpAddress { get; set; }
        public string Comment { get; set; }
        public string StaffId { get; set; }
        public string CreatedByName { get; set; }
    }
}
