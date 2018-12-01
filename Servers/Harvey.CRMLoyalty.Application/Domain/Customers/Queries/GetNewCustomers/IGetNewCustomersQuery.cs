namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public interface IGetNewCustomersQuery
    {
        GetNewCustomersResponse Execute(GetNewCustomersRequest request);
    }
}
