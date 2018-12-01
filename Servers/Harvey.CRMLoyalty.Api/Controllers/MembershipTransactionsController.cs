using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddMembershipCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.VoidMembershipCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions.Model;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/MembershipTransactions")]
    public class MembershipTransactionsController : Controller
    {

        private IGetMembershipTransactionsByCustomerQuery _getMembershipTransaction;
        private IAddMembershipCommandHandler _addMembershipCommandHandler;
        private readonly IGetMembershipTransactions _getMembershipTransactions;
        private IVoidMembershipCommandHandler _voidMembershipCommandHandler;
        private IGetVoidMembershipTransactionsQuery _getVoidMembershipTransactions;

        public MembershipTransactionsController(IGetMembershipTransactionsByCustomerQuery getMembershipTransaction,
                                                IAddMembershipCommandHandler addMembershipCommandHandler,
                                                IGetMembershipTransactions getMembershipTransactions,
                                                IVoidMembershipCommandHandler voidMembershipCommandHandler,
                                                IGetVoidMembershipTransactionsQuery getVoidMembershipTransactions
                                                )
        {
            _getMembershipTransaction = getMembershipTransaction;
            _addMembershipCommandHandler = addMembershipCommandHandler;
            _getMembershipTransactions = getMembershipTransactions;
            _voidMembershipCommandHandler = voidMembershipCommandHandler;
            _getVoidMembershipTransactions = getVoidMembershipTransactions;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff,Member,Staff")]
        public IActionResult GetMembershipTransactionsByCustomer(GetMembershipTransactionsByCustomerRequest request)
        {
            var result = _getMembershipTransaction.Execute(request);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Staff,AdminStaff,Administrator")]
        public async Task<IActionResult>  AddMembership([FromBody] AddMembershipCommand request)
        {
            var result = await _addMembershipCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("getsbyoutlet")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetMembershipTransactionsByOutlet(GetMembershipTransactionsRequest request)
        {
            var result = _getMembershipTransactions.Execute(request);
            return Ok(result);
        }

        [HttpPost("void")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> VoidMembership([FromBody] VoidMembershipCommand request)
        {
            var result = await _voidMembershipCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("getvoidmembershiptransactions")]
        [Authorize(Roles = "Administrator,AdminStaff,Staff")]
        public IActionResult GetVoidMembershipTransactions(GetVoidMembershipTransactionsRequest request)
        {
            var result = _getVoidMembershipTransactions.Execute(request);
            return Ok(result);
        }
    }
}
    
