namespace Harvey.CRMLoyalty.Application.Entities
{
    public class Staff_Outlet
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public virtual Staff Staff { get; set; }
        public string OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }
    }

    public enum TypeOfStaff
    {
        StoreAccount,
        StaffAccount,
        StaffAdminAccount
    }
}
