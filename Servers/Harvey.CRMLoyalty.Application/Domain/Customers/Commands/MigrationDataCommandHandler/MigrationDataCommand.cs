using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.MigrationDataCommandHandler
{
    public class MigrationDataCommand
    {
        public string CustomerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string FullPhoneNumber { get; set; }
        public string JoinedDate { get; set; }
        public string LastUsedDate { get; set; }
        public string Status { get; set; }
        public string DateOfBirth { get; set; }
        public string Notes { get; set; }
        public string LastEdited { get; set; }
        public string LastOutLetVisited { get; set; }
        public string FirstOutLet { get; set; }
        public string MembershipTier { get; set; }
        public string TransactionCreatedDate { get; set; }
        public string TransactionExpireDate { get; set; }
        public string WalletBalance { get; set; }
        public string RewardPointBalance { get; set; }
        public string LegacyPointBalance { get; set; }
    }
}
