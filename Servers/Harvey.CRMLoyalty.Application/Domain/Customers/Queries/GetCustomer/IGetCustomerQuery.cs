using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer.Model;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer
{
    public interface IGetCustomerQuery
    {
        Task<CustomerResponse> GetCustomerByPhoneAsync(CustomerRequest request);

        CustomerResponse GetCustomerById(CustomerRequest request);

        string GetCustomerCodeById(string Id);
    }
}
