
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet.Model;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.TopUpWalletCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.SpendingWalletCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.VoidWalletCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics.Model;

namespace Harvey.CRMLoyalty.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/WalletTransactions")]
    public class WalletTransactionsController : Controller
    {
        private IGetWalletTransactionsQuery _getWalletTransactionsQuery;
        private IGetWalletTransactionsByCustomerQuery _getWalletTransactionsByCustomerQuery;
        private IGetWalletTransactionsByOutlet _getWalletTransactionsByOutlet;
        private ITopUpWalletCommandHandler _topUpWalletCommandHandler;
        private ISpendingWalletCommandHandler _spendingWalletCommandHandler;
        private IGetTotalBalanceWalletTransactionQuery _getTotalBalanceWalletTransactionQuery;
        private IGetCreditValueWalletTransactionQuery _getCreditValueWalletTransactionQuery;
        private IGetDebitValueWalletTransactionQuery _getDebitValueWalletTransactionQuery;
        private IVoidWalletCommandHandler _voidWalletCommandHandler;
        private readonly IGetVoidOfCreditWalletTransactionQuery _getVoidOfCreditWalletTransactionQuery;
        private readonly IGetVoidOfDebitWalletTransactionQuery _getVoidOfDebitWalletTransactionQuery;
        private IGetWalletStatisticsQuery _getWalletStatisticsQuery;

        public WalletTransactionsController(IGetWalletTransactionsQuery getWalletTransactionsQuery,
            IGetWalletTransactionsByCustomerQuery getWalletTransactionsByCustomerQuery,
            IGetWalletTransactionsByOutlet getWalletTransactionsByOutlet,
            ITopUpWalletCommandHandler topUpWalletCommandHandler,
            ISpendingWalletCommandHandler spendingWalletCommandHandler,
            IGetTotalBalanceWalletTransactionQuery getTotalBalanceWalletTransactionQuery,
            IGetCreditValueWalletTransactionQuery getCreditValueWalletTransactionQuery,
            IGetDebitValueWalletTransactionQuery getDebitValueWalletTransactionQuery,
            IGetVoidOfCreditWalletTransactionQuery getVoidOfCreditWalletTransactionQuery,
            IGetVoidOfDebitWalletTransactionQuery getVoidOfDebitWalletTransactionQuery,
            IVoidWalletCommandHandler voidWalletCommandHandler,
            IGetWalletStatisticsQuery getWalletStatisticsQuery
            )
        {
            _getWalletTransactionsQuery = getWalletTransactionsQuery;
            _getWalletTransactionsByCustomerQuery = getWalletTransactionsByCustomerQuery;
            _getWalletTransactionsByOutlet = getWalletTransactionsByOutlet;
            _topUpWalletCommandHandler = topUpWalletCommandHandler;
            _spendingWalletCommandHandler = spendingWalletCommandHandler;
            _getTotalBalanceWalletTransactionQuery = getTotalBalanceWalletTransactionQuery;
            _getCreditValueWalletTransactionQuery = getCreditValueWalletTransactionQuery;
            _getDebitValueWalletTransactionQuery = getDebitValueWalletTransactionQuery;
            _getVoidOfCreditWalletTransactionQuery = getVoidOfCreditWalletTransactionQuery;
            _getVoidOfDebitWalletTransactionQuery = getVoidOfDebitWalletTransactionQuery;
            _voidWalletCommandHandler = voidWalletCommandHandler;
            _getWalletStatisticsQuery = getWalletStatisticsQuery;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetWalletTransactions(GetWalletTransactionsRequest request)
        {
            var result = _getWalletTransactionsQuery.GetWalletTransactions(request);
            return Ok(result);
        }

        [HttpGet("getsbystaff")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetWalletTransactionsByStaff(GetWalletTransactionsRequest request)
        {
            var result = _getWalletTransactionsQuery.GetWalletTransactionsByStaff(request);
            return Ok(result);
        }

        [HttpGet("getwalletbalance")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public IActionResult GetWalletBalance(string request)
        {
            var result = _getWalletTransactionsQuery.GetWalletBalance(request);
            return Ok(result);
        }


        [HttpGet("getsbycustomer")]
        [Authorize(Roles = "Administrator,AdminStaff,Member,Staff")]
        public IActionResult GetWalletTransactionsByCustomer(GetWalletTransactionsByCustomerRequest request)
        {
            var result = _getWalletTransactionsByCustomerQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsbymember")]
        [Authorize(Roles = "Member")]
        public IActionResult GetWalletTransactionsByMember(GetWalletTransactionsByCustomerRequest request)
        {
            var result = _getWalletTransactionsByCustomerQuery.GetByMember(request);
            return Ok(result);
        }

        [HttpGet("getsbyoutlet")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetWalletTransactionsByOutlet(GetWalletTransactionsByOutletRequest request)
        {
            var result = _getWalletTransactionsByOutlet.Execute(request);
            return Ok(result);
        }

        [HttpPost("topupwallet")]
        [Authorize(Roles = "Staff,AdminStaff")]
        public async Task<IActionResult> TopUpWallet([FromBody] TopUpWalletCommand request)
        {
            var result = await _topUpWalletCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("spendingwallet")]
        [Authorize(Roles = "Staff,AdminStaff,Administrator")]
        public async Task<IActionResult> SpendingWallet([FromBody] SpendingWalletCommand request)
        {
            var result = await _spendingWalletCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("getstotalbalance")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetTotalBalance(GetTotalBalanceWalletTransactionRequest request)
        {
            var result = _getTotalBalanceWalletTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getscreditsummary")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetCreditSummary(GetCreditValueWalletTransactionRequest request)
        {
            var result = _getCreditValueWalletTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsdebitsummary")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetDebitSummary(GetDebitValueWalletTransactionRequest request)
        {
            var result = _getDebitValueWalletTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpPost("voidwallet")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> VoidWallet([FromBody] VoidWalletCommand request)
        {
            var result = await _voidWalletCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("getsvoidofcredit")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetVoidOfCredit(GetVoidOfCreditWalletTransactionRequest request)
        {
            var result = _getVoidOfCreditWalletTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsvoidofdebit")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetVoidOfDebit(GetVoidOfDebitWalletTransactionRequest request)
        {
            var result = _getVoidOfDebitWalletTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getWalletSatistics")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetWalletSatistics(GetWalletStatisticsRequest request)
        {
            var result = _getWalletStatisticsQuery.Execute(request);
            return Ok(result);
        }

    }
}
