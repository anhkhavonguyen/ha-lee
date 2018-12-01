using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IGetCustomerQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private readonly ILoggingActivityService _activityService;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;

        public GetCustomerQuery(HarveyCRMLoyaltyDbContext dbContext,
           ILoggingActivityService loggingActivityService,
           IOptions<ConfigurationRabbitMq> configurationRabbitMq)
        {
            _dbContext = dbContext;
            _activityService = loggingActivityService;
            _configurationRabbitMq = configurationRabbitMq;
        }

        public async Task<CustomerResponse> GetCustomerByPhoneAsync(CustomerRequest request)
        {
            var query = _dbContext.Customers.AsNoTracking().FirstOrDefault(a => a.Phone.Equals(request.Phone) && a.PhoneCountryCode.Equals(request.CountryCode) && a.Status == Status.Active);
            if (query != null)
            {
                if (request.IsServing)
                {
                    var phone = $"{query.PhoneCountryCode}{query.Phone}";
                    var user = _dbContext.Staffs.Where(a => a.Id == request.StaffId).FirstOrDefault();
                    var userName = user != null ? $"{user.FirstName} {user.LastName}" : (request.StaffId == LogInformation.AdministratorId ? LogInformation.AdministratorName : "");
                    await LogAction( request.StaffId, _configurationRabbitMq.Value.RabbitMqUrl, query.CustomerCode, phone, userName, request.OutletId);
                }
                return new CustomerResponse()
                {
                    Id = query.Id,
                };
            }
            return null;
        }

        public CustomerResponse GetCustomerById(CustomerRequest request)
        {
            var queryCustomer = _dbContext.Customers.AsNoTracking().FirstOrDefault(x => x.Id == request.CustomerId);

            var queryLastMembershipTransaction = queryCustomer != null ?
                _dbContext.MembershipTransactions.AsNoTracking().Include(m => m.MembershipType).OrderByDescending(m=>m.CreatedDate).FirstOrDefault(a => a.CustomerId.Contains(queryCustomer.Id)) : null;

            var membershipTransactions = queryCustomer != null ?
               _dbContext.MembershipTransactions.AsNoTracking().Where(a => a.CustomerId.Contains(queryCustomer.Id)).Count() : 0;

            var premiumMembershipTransaction = queryCustomer != null ?
                _dbContext.MembershipTransactions.AsNoTracking().OrderByDescending(m => m.CreatedDate).FirstOrDefault(a => a.CustomerId.Contains(queryCustomer.Id) && a.MembershipTypeId == 2) : null;
            var response = new CustomerResponse();
            if (queryCustomer != null)
            {
                response = new CustomerResponse()
                {
                    Id = queryCustomer.Id,
                    FirstName = queryCustomer.FirstName,
                    LastName = queryCustomer.LastName,
                    Phone = queryCustomer.Phone,
                    PhoneCountryCode = queryCustomer.PhoneCountryCode,
                    Email = queryCustomer.Email,
                    DateOfBirth = queryCustomer.DateOfBirth,
                    LastUsed = queryCustomer.LastUsed,
                    TotalStranstion = TotalTransactions(queryCustomer.Id),
                    Membership = GetMemberShipType(ref queryLastMembershipTransaction),
                    ExpiredDate = queryLastMembershipTransaction != null ? queryLastMembershipTransaction.ExpiredDate : null,
                    CommentMembership = queryLastMembershipTransaction != null ? queryLastMembershipTransaction.Comment : null,
                    ProfileImage = queryCustomer.ProfileImage,
                    HasPreminumMembershipTransaction = premiumMembershipTransaction != null,
                    CustomerCode = queryCustomer.CustomerCode,
                    Status = Convert.ToInt32(queryCustomer.Status),
                    Gender = queryCustomer.Gender
                };
            }

            return response;
        }

        public string GetCustomerCodeById(string Id)
        {
            var queryCustomer = _dbContext.Customers.AsNoTracking().FirstOrDefault(x => x.Id == Id);
            return queryCustomer.CustomerCode;
        }

        private int TotalTransactions(string customerId)
        {
            int mtransaction = _dbContext.MembershipTransactions.AsNoTracking().Count(a => a.CustomerId == customerId);
            int pTransaction = _dbContext.PointTransactions.AsNoTracking().Count(a => a.CustomerId == customerId);
            int wTransaction = _dbContext.WalletTransactions.AsNoTracking().Count(a => a.CustomerId == customerId);
            int total = 0 + mtransaction + pTransaction + wTransaction;
            return total;
        }

        private string GetMemberShipType(ref MembershipTransaction entity)
        {
            if (entity == null)
                return Enum.GetName(MembershipTranactionEnum.Basic.GetType(), MembershipTranactionEnum.Basic);
            return entity.MembershipType.TypeName;
        }

        private async Task LogAction(string staffId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName, string outletId)
        {
            var request = new LoggingActivityRequest();
            request.UserId = staffId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.LoginServingCustomer;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            request.Value = outletId;
            await _activityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
