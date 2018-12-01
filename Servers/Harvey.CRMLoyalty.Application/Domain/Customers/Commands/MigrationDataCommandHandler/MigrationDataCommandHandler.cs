using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.Message.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.MigrationDataCommandHandler
{
    public class MigrationDataCommandHandler : IMigrationDataCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _harveyCRMLoyaltyDbContext;

        public MigrationDataCommandHandler(HarveyCRMLoyaltyDbContext harveyCRMLoyaltyDbContext)
        {
            _harveyCRMLoyaltyDbContext = harveyCRMLoyaltyDbContext;
        }

        public string Excute(IList<MigrationDataCommand> models)
        {
            string outletId = "10ecc00f-6e24-49bc-b735-c18127433f8b";
            string staffId = "76af5bea-6af1-4a18-b38e-1396875518e5";
            string basic = "Basic";
            string primeumPlus = "Premium+";
            var message = "";
            var _outletId = "10ecc00f-6e24-49bc-b735-c18127433f8b";
            var _outletCode = "MO";
            var _staffId = "76af5bea-6af1-4a18-b38e-1396875518e5";
            if (models.Count > 0)
            {
                int itemperLoop = 500;
                int totalItem = models.Count();
                int itemReaded = 0;
                int index = 0;
                while (itemReaded < totalItem)
                {
                    int skipItem = index * itemperLoop;
                    int takeItem = totalItem - (skipItem + itemperLoop) > 0 ? itemperLoop : totalItem - skipItem;
                    itemReaded = skipItem + takeItem;

                    var customers = new List<Customer>();
                    var memberShipTransactions = new List<MembershipTransaction>();
                    var walletTransactions = new List<WalletTransaction>();
                    var pointTransactions = new List<PointTransaction>();
                    var itemIndex = index * itemperLoop;

                    foreach (var item in models.Skip(skipItem).Take(takeItem))
                    {

                        itemIndex++;
                        if (string.IsNullOrEmpty(item.FullPhoneNumber))
                        {
                            message += $"Invalid phone number at item ID {item.CustomerId} \r\n";
                            continue;
                        }
                      
                        var customer = new Customer();
                        customer.Id = item.CustomerId;
                        customer.FirstName = item.FirstName.Replace(";", ",");
                        customer.LastName = item.LastName.Replace(";", ",");
                        customer.Email = item.Email;
                        customer.JoinedDate = !string.IsNullOrEmpty(item.JoinedDate) ? (DateTime?)DateTime.ParseExact(item.JoinedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : null;
                        customer.LastUsed = !string.IsNullOrEmpty(item.LastUsedDate) ? (DateTime?)DateTime.ParseExact(item.LastUsedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : null;
                        customer.DateOfBirth = !string.IsNullOrEmpty(item.DateOfBirth) ? (DateTime?)DateTime.ParseExact(item.DateOfBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : null;
                        customer.UpdatedDate = DateTime.UtcNow;
                        customer.CustomerCode = $"{_outletCode}{DateTime.UtcNow.ToString("MMyy")}{itemIndex.ToString().PadLeft(5, '0')}";
                        customer.Status = Status.Active;
                        if (item.FullPhoneNumber.Split(' ').Length >= 2)
                        {
                            customer.PhoneCountryCode = item.FullPhoneNumber.Split(' ')[0].Replace("+", "");
                            customer.Phone = item.FullPhoneNumber.Split(' ')[1];
                        }
                        else
                        {
                            message += $"Invalid phone number at item ID {item.CustomerId} \r\n";
                            continue;
                        }

                        customers.Add(customer);
                        var membershipTransaction = new MembershipTransaction();

                        membershipTransaction.Id = Guid.NewGuid().ToString();
                        membershipTransaction.Comment = "System Migration";
                        membershipTransaction.MembershipTypeId = item.MembershipTier == basic ? (int)MembershipTranactionEnum.Basic :
                                                                 item.MembershipTier == primeumPlus ? (int)MembershipTranactionEnum.PremiumPlus
                                                                                                      : (int)MembershipTranactionEnum.Basic;
                        membershipTransaction.CustomerId = item.CustomerId;
                        membershipTransaction.OutletId = _outletId;
                        membershipTransaction.StaffId = _staffId;
                        membershipTransaction.CreatedDate = !string.IsNullOrEmpty(item.TransactionCreatedDate) ? (DateTime?)DateTime.ParseExact(item.TransactionCreatedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : null;
                        membershipTransaction.ExpiredDate = item.MembershipTier == primeumPlus
                                                     ? (!string.IsNullOrEmpty(item.TransactionExpireDate) || item.TransactionExpireDate != "None")
                                                     ? (DateTime?)DateTime.ParseExact(item.TransactionExpireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : null : null;
                        membershipTransaction.IPAddress = "System Migration";
                        membershipTransaction.BrowserName = "System Migration";
                        membershipTransaction.Device = "System Migration";
                        membershipTransaction.CreatedBy = "System Migration";
                        membershipTransaction.UpdatedBy = "System Migration";
                        memberShipTransactions.Add(membershipTransaction);
                        decimal walletValue = 0;
                        walletValue = !string.IsNullOrEmpty(item.WalletBalance) ? decimal.Parse(item.WalletBalance.Trim()) : 0;
                        if (walletValue > 0)
                        {
                            var walletTransaction = new WalletTransaction()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Comment = "System Migration",
                                CustomerId = item.CustomerId,
                                OutletId = outletId,
                                StaffId = staffId,
                                IPAddress = "System Migration",
                                BrowserName = "System Migration",
                                Device = "System Migration",
                                CreatedBy = "System Migration",
                                UpdatedBy = "System Migration",
                                Credit = walletValue,
                                BalanceCredit = walletValue,
                                BalanceTotal = walletValue,
                            };
                            walletTransactions.Add(walletTransaction);
                        }
                        decimal pointValue = 0;
                        pointValue = !string.IsNullOrEmpty(item.RewardPointBalance) ? decimal.Parse(item.RewardPointBalance.Trim()) : 0;
                        if (pointValue > 0)
                        {
                            var pointTransaction = new PointTransaction()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Comment = "System Migration",
                                CustomerId = item.CustomerId,
                                OutletId = outletId,
                                StaffId = staffId,
                                IPAddress = "System Migration",
                                BrowserName = "System Migration",
                                Device = "System Migration",
                                CreatedBy = "System Migration",
                                UpdatedBy = "System Migration",
                                PointTransactionTypeId = (int)PointTransactionTypeEnum.AddPoint,
                                Credit = pointValue,
                                BalanceCredit = pointValue,
                                BalanceTotal = pointValue,
                                ExpiredDate = DateTime.UtcNow.AddYears(1)
                            };
                            pointTransactions.Add(pointTransaction);
                        }
                    }
                    _harveyCRMLoyaltyDbContext.Customers.AddRange(customers);

                    _harveyCRMLoyaltyDbContext.MembershipTransactions.AddRange(memberShipTransactions);

                    _harveyCRMLoyaltyDbContext.WalletTransactions.AddRange(walletTransactions);

                    _harveyCRMLoyaltyDbContext.PointTransactions.AddRange(pointTransactions);

                    _harveyCRMLoyaltyDbContext.SaveChanges();

                    index++;
                }

            }
            return message;
        }

        public async Task ExcuteAsync(IList<UpdatedApplicationUser> updatedApplicationUsers)
        {
            var customers = _harveyCRMLoyaltyDbContext.Customers
                .Where(c => updatedApplicationUsers.Select(u => u.Id).Contains(c.Id))
                .ToList();
                
            for(var i = 0; i < customers.Count; i++)
            {
                for(var j = 0; j < updatedApplicationUsers.Count; j++)
                {
                    if (customers[i].Id == updatedApplicationUsers[j].Id)
                    {
                        customers[i].Gender = (Data.Gender)updatedApplicationUsers[j].Gender;
                        break;
                    }
                }
            }

            await _harveyCRMLoyaltyDbContext.SaveChangesAsync();
        }
    }
}
