namespace Harvey.CRMLoyalty.Application.Domain.Staffs.Queries
{
    public interface IGetStaffsQuery
    {
        GetStaffsResponse GetStaffs(GetStaffsRequest request);

        GetStaffsByOutletResponse GetStaffsByOutlet(GetStaffsByOutletRequest request);
    }
}
