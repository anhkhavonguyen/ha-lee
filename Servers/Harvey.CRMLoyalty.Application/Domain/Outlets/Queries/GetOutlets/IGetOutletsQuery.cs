namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Queries
{
    public interface IGetOutletsQuery
    {
        GetOutletsResponse GetOutlets(GetOutletsRequest request);
        GetOutletsResponse GetOutlets();
        GetOutletsResponse GetOutletsByStaff(GetOutletsRequest request);
        GetOutletsResponse GetOutletsWithStoreAccount(GetOutletsRequest request);
    }
}
