using Harvey.CRMLoyalty.Api.Models;
using Harvey.CRMLoyalty.Api.Utils;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ActiveCustomerCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.InitCustomerProfileCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.MigrationDataCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ReactiveCustomerWithNewPhoneCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer.Model;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers.Model;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetPremiumCustomers;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetPremiumCustomers.Model;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers.Model;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers.Model;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointBalance;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionBalance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly IGetCustomersQuery _getCustomersQuery;
        private readonly IGetCustomerQuery _getCustomerQuery;
        private readonly IGetPointBalance _getPointTransactionBalance;
        private IExportCSVQuery _exportCSVQuery;
        private readonly IGetWalletTransactionBalance _getWalletTransactionBalance;
        private readonly IGetCurrentMembershipQueryHandler _getCurrentMembershipQueryHandler;
        private readonly IMigrationDataCommandHandler _migrationDataCommandHandler;
        private readonly IGetNewCustomersQuery _getNewCustomerQuery;
        private readonly IGetExpiredCustomersQuery _getExpiredCustomerQuery;
        private readonly IGetVoidedCustomersQuery _getVoidedCustomersQuery;
        private readonly IInitCustomerProfileCommandHandler _initCustomerProfileCommandHandler;
        private readonly IGetPremiumCustomers _getPremiumCustomers;
        private readonly IActiveCustomerCommandHandler _activeCustomerCommandHandler;
        private readonly IReactiveCustomerWithNewPhoneCommandHandler _reactiveCustomerCommandHandler;
        private readonly IGetUpgradedCustomersQuery _getUpgradedCustomersQuery;
        private readonly IGetExtendedCutomersQuery _getExtendedCutomersQuery;
        private readonly IGetRenewedCustomersQuery _getRenewedCustomersQuery;
        public CustomersController(IGetCustomersQuery getCustomersQuery,
            IGetCustomerQuery getCustomerQuery,
            IGetPointBalance getPointBalance,
            IGetWalletTransactionBalance getWalletTransactionBalance,
            IGetCurrentMembershipQueryHandler getCurrentMembershipQueryHandler,
            IExportCSVQuery exportCSVQuery,
            IMigrationDataCommandHandler migrationDataCommandHandler,
            IGetNewCustomersQuery getNewCustomerQuery,
            IGetExpiredCustomersQuery getExpiredCustomerQuery,
            IGetVoidedCustomersQuery getVoidedCustomersQuery,
            IInitCustomerProfileCommandHandler initCustomerProfileCommandHandler,
            IGetPremiumCustomers getPremiumCustomers,
            IActiveCustomerCommandHandler activeCustomerCommandHandler,
            IReactiveCustomerWithNewPhoneCommandHandler reactiveCustomerCommandHandler,
            IGetUpgradedCustomersQuery getUpgradedCustomersQuery,
            IGetExtendedCutomersQuery getExtendedCutomersQuery,
            IGetRenewedCustomersQuery getRenewedCustomersQuery)
        {
            _getCustomersQuery = getCustomersQuery;
            _getCustomerQuery = getCustomerQuery;
            _exportCSVQuery = exportCSVQuery;
            _getPointTransactionBalance = getPointBalance;
            _getWalletTransactionBalance = getWalletTransactionBalance;
            _getCurrentMembershipQueryHandler = getCurrentMembershipQueryHandler;
            _migrationDataCommandHandler = migrationDataCommandHandler;
            _getNewCustomerQuery = getNewCustomerQuery;
            _getExpiredCustomerQuery = getExpiredCustomerQuery;
            _getVoidedCustomersQuery = getVoidedCustomersQuery;
            _initCustomerProfileCommandHandler = initCustomerProfileCommandHandler;
            _getPremiumCustomers = getPremiumCustomers;
            _activeCustomerCommandHandler = activeCustomerCommandHandler;
            _reactiveCustomerCommandHandler = reactiveCustomerCommandHandler;
            _getUpgradedCustomersQuery = getUpgradedCustomersQuery;
            _getExtendedCutomersQuery = getExtendedCutomersQuery;
            _getRenewedCustomersQuery = getRenewedCustomersQuery;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetCustomers(CustomersRequest request)
        {
            var result = _getCustomersQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getcustomerbyphone")]
        [Authorize(Roles = "Staff,AdminStaff")]
        public async Task<IActionResult> GetCustomerByPhoneAsync(CustomerRequest request)
        {
            var result = await _getCustomerQuery.GetCustomerByPhoneAsync(request);
            return Ok(result);
        }

        [HttpGet("getcustomerbyid")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetCustomerById(CustomerRequest request)
        {
            var result = _getCustomerQuery.GetCustomerById(request);
            return Ok(result);
        }

        [HttpGet("GetPointsCurrentUser")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetPointsCurrentUser()
        {
            var userId = User.GetUserId();
            return Ok(new
            {
                PointTransactionBalance = await _getPointTransactionBalance.ExecuteAsync(userId),
                WalletTransactionBalance = await _getWalletTransactionBalance.ExecuteAsync(userId)
            });
        }

        [HttpGet("GetMembershipCurrentUser")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetMembershipCurrentUser()
        {
            var userId = User.GetUserId();
            var result = await _getCurrentMembershipQueryHandler.ExecuteAsync(userId);
            return Ok(result);
        }

        [HttpPost("uploadfile")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult UploadFile()
        {
            string message = "";

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var listCommand = new List<MigrationDataCommand>();
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    string line = "";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var data = line.Split(new[] { ',' });
                        var model = new MigrationDataCommand()
                        {
                            CustomerId = data[0],
                            FirstName = data[1],
                            LastName = data[2],
                            Email = data[3],
                            FullPhoneNumber = data[4],
                            JoinedDate = data[5],
                            LastUsedDate = data[6],
                            Status = data[7],
                            DateOfBirth = data[8],
                            Notes = data[9],
                            LastEdited = data[10],
                            LastOutLetVisited = data[11],
                            FirstOutLet = data[12],
                            MembershipTier = data[13],
                            TransactionCreatedDate = data[14],
                            TransactionExpireDate = data[15],
                            WalletBalance = data[16],
                            RewardPointBalance = data[17],
                            LegacyPointBalance = data[18]
                        };
                        listCommand.Add(model);
                    }
                }
                listCommand.RemoveAt(0);
                message = _migrationDataCommandHandler.Excute(listCommand);
            }
            return Ok(message);
        }

        [HttpGet("exportcsv")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult ExportCSV()
        {
            var result = _exportCSVQuery.Excute();
            return File(result, "text/csv", $"Customers.csv");
        }


        [HttpGet("getnewcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetNewCustomers(GetNewCustomersRequest request)
        {
            var result = _getNewCustomerQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getexpiredcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetExpiredCustomers(GetExpiredCustomersRequest request)
        {
            var result = _getExpiredCustomerQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getdowngradedcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetVoidedCustomers(GetVoidedCustomersRequest request)
        {
            var result = _getVoidedCustomersQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("initcustomerprofile")]
        [Authorize(Roles = "Member,Staff,AdminStaff,Administrator")]
        public async Task<IActionResult> InitCustomerProfile([FromBody] InitCustomerProfileInputModel initCustomerProfileInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var initCustomerProfileCommand = new InitCustomerProfileCommand
            {
                CurrentUserId = User.GetUserId(),
                CreatedBy = initCustomerProfileInputModel.CreatedBy,
                OriginalUrl = initCustomerProfileInputModel.OriginalUrl,
                OutletId = initCustomerProfileInputModel.OutletId,
                OutletName = initCustomerProfileInputModel.OutletName,
                PhoneCountryCode = initCustomerProfileInputModel.PhoneCountryCode,
                PhoneNumber = initCustomerProfileInputModel.PhoneNumber,
                UserId = Guid.NewGuid().ToString()
            };
            var result = await _initCustomerProfileCommandHandler.ExecuteAsync(initCustomerProfileCommand);
            return Ok(result);
        }


        [HttpGet("getpremiumcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetPremiumCustomers(GetPremiumCustomersRequest request)
        {
            var result = _getPremiumCustomers.Execute(request);
            return Ok(result);
        }
        
        [HttpGet("getupgradedcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetUpgradedCustomers(GetUpgradedCustomersRequest request)
        {
            var result = _getUpgradedCustomersQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getextendedcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetExtendedCustomers(GetExtendedCutomersRequest request)
        {
            var result = _getExtendedCutomersQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getrenewedcustomers")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetRenewedCustomers(GetRenewedCustomersRequest request)
        {
            var result = _getRenewedCustomersQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("activeCustomer")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> ActiveCustomer([FromBody] ActiveCustomerCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _activeCustomerCommandHandler.ExecuteAsync(command);
            return Ok(result);
        }

        [HttpPost("reactiveCustomer")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> ReactiveCustomer([FromBody] ReactiveCustomerWithNewPhoneCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _reactiveCustomerCommandHandler.ExecuteAsync(command);
            return Ok(result);
        }

        [HttpGet("getcustomercodebyid")]
        [Authorize(Roles = "Member")]
        public IActionResult GetCustomerCodeById()
        {
            var userId = User.GetUserId();
            var result = _getCustomerQuery.GetCustomerCodeById(userId);
            return Ok(result);
        }

        [HttpGet("getcustomersbyCustomerCodes")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult getcustomersbyCustomerCodes(CustomersRequest request)
        {
            var result = _getCustomersQuery.GetCustomersbyCustomerCodes(request);
            return Ok(result);
        }
    }
}
