using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddPointCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.RedeemPointCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.VoidPointCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints.Model;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointBalance;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics.Model;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PointTransactions")]
    public class PointTransactionsController : Controller
    {
        private readonly IGetPointTransactionsQuery _getPointTransactionsQuery;
        private readonly IGetPointTransactionsByCustomerQuery _getPointTransactionsByCustomerQuery;
        private readonly IAddPointCommandHandler _addPointCommandHandler;
        private readonly IRedeemPointCommandHandler _redeemPointCommandHandler;
        private readonly IGetPointBalance _getPointBalance;
        private readonly IVoidPointCommandHandler _voidPointCommandHandler;
        private readonly IGetDebitValuePointTransactionQuery _getDebitValuePointTransactionQuery;
        private readonly IGetCreditValuePointTransactionQuery _getCreditValuePointTransactionQuery;
        private readonly IGetTotalBalancePointTransactionQuery _getTotalBalancePointTransactionQuery;
        private IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IGetVoidOfCreditPointTransactionQuery _getVoidOfCreditPointTransactionQuery;
        private readonly IGetVoidOfDebitPointTransactionQuery _getVoidOfDebitPointTransactionQuery;
        private readonly IGetExpiryPointsQuery _getExpiryPointsQuery;
        private readonly IGetPointsStatisticsQuery _getPointsStatisticsQuery;
        public PointTransactionsController(IGetPointTransactionsQuery getPointTransactionsQuery,
                                           IGetPointTransactionsByCustomerQuery getPointTransactionsByCustomerQuery,
                                           IAddPointCommandHandler addPointCommandHandler,
                                           IRedeemPointCommandHandler redeemPointCommandHandler,
                                           IGetPointBalance getPointBalance, IOptions<ConfigurationRabbitMq> configurationRabbitMq,
                                           IVoidPointCommandHandler voidPointCommandHandler,
                                           IGetDebitValuePointTransactionQuery getDebitValuePointTransactionQuery,
                                           IGetCreditValuePointTransactionQuery getCreditValuePointTransactionQuery,
                                           IGetTotalBalancePointTransactionQuery getTotalBalancePointTransactionQuery,
                                           IGetVoidOfCreditPointTransactionQuery getVoidOfCreditPointTransactionQuery,
                                           IGetVoidOfDebitPointTransactionQuery getVoidOfDebitPointTransactionQuery,
                                           IGetExpiryPointsQuery getExpiryPointsQuery,
                                           IGetPointsStatisticsQuery getPointsStatisticsQuery)
        {
            _getPointTransactionsQuery = getPointTransactionsQuery;
            _getPointTransactionsByCustomerQuery = getPointTransactionsByCustomerQuery;
            _addPointCommandHandler = addPointCommandHandler;
            _redeemPointCommandHandler = redeemPointCommandHandler;
            _getPointBalance = getPointBalance;
            _configurationRabbitMq = configurationRabbitMq;
            _voidPointCommandHandler = voidPointCommandHandler;
            _getDebitValuePointTransactionQuery = getDebitValuePointTransactionQuery;
            _getCreditValuePointTransactionQuery = getCreditValuePointTransactionQuery;
            _getTotalBalancePointTransactionQuery = getTotalBalancePointTransactionQuery;
            _getVoidOfCreditPointTransactionQuery = getVoidOfCreditPointTransactionQuery;
            _getVoidOfDebitPointTransactionQuery = getVoidOfDebitPointTransactionQuery;
            _getExpiryPointsQuery = getExpiryPointsQuery;
            _getPointsStatisticsQuery = getPointsStatisticsQuery;
        }


        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetPointTransactions(GetPointTransactionsRequest request)
        {
            var result = _getPointTransactionsQuery.GetPointTransactions(request);
            return Ok(result);
        }

        [HttpGet("getsbystaff")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetPointTransactionsByStaff(GetPointTransactionsRequest request)
        {
            var result = _getPointTransactionsQuery.GetPointTransactionsByStaff(request);
            return Ok(result);
        }

        [HttpGet("getpointbalance")]
        [Authorize(Roles = "Administrator,Staff,AdminStaff")]
        public async Task<IActionResult> GetPointBalance(string customerId)
        {
            var result = await _getPointBalance.ExecuteAsync(customerId);
            return Ok(result);
        }

        [HttpGet("getsbycustomer")]
        [Authorize(Roles = "Administrator,AdminStaff,Member,Staff")]
        public IActionResult GetPointTransactionsByCustomer(GetPointTransactionsByCustomerRequest request)
        {
            var result = _getPointTransactionsByCustomerQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsbymember")]
        [Authorize(Roles = "Member")]
        public IActionResult GetPointTransactionsByMember(GetPointTransactionsByCustomerRequest request)
        {
            var result = _getPointTransactionsByCustomerQuery.GetByMember(request);
            return Ok(result);
        }

        [HttpPost("addpoint")]
        [Authorize(Roles = "Staff,AdminStaff")]
        public async Task<IActionResult>  AddPoint([FromBody] AddPointCommand request)
        {
            var result = await _addPointCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("redeempoint")]
        [Authorize(Roles = "Staff,AdminStaff")]
        public async Task<IActionResult> RedeemPoint([FromBody] RedeemPointCommand request)
        {
            var result = await _redeemPointCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }


        [HttpGet("getsbyoutlet")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetPointTransactionsByOutlet(GetPointTransactionsByOutletRequest request)
        {
            var result = _getPointTransactionsQuery.GetPointTransactionsByOutlet(request);
            return Ok(result);
        }

        [HttpPost("voidpoint")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult>  VoidPoint([FromBody] VoidPointCommand request)
        {
            var result = await _voidPointCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("getsdebitsummary")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetDebitSummary(GetDebitValuePointTransactionRequest request)
        {
            var result = _getDebitValuePointTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getscreditsummary")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetCreditSummary(GetCreditValuePointTransactionRequest request)
        {
            var result = _getCreditValuePointTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getstotalbalance")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetTotalBalance(GetTotalBalancePointTransactionRequest request)
        {
            var result = _getTotalBalancePointTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsvoidofcredit")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetVoidOfCredit(GetVoidOfCreditPointTransactionRequest request)
        {
            var result = _getVoidOfCreditPointTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getsvoidofdebit")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetVoidOfDebit(GetVoidOfDebitPointTransactionRequest request)
        {
            var result = _getVoidOfDebitPointTransactionQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getexpirypoints")]
        [Authorize(Roles = "Staff,AdminStaff,Member")]
        public async Task<IActionResult> GetExpiryPoints(GetExpiryPointsRequest request)
        {
            var result = await _getExpiryPointsQuery.GetExpiryPointAsync(request);
            return Ok(result);
        }

        [HttpGet("getPointsStatistics")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetPointsStatistics(GetPointsStatisticsRequest request)
        {
            var result = _getPointsStatisticsQuery.Execute(request);
            return Ok(result);
        }
    }
}
