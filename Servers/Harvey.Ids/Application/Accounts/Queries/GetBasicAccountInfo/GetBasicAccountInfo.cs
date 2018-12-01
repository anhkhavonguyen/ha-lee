namespace Harvey.Ids.Application.Accounts.Queries.GetBasicAccountInfo
{
    public class GetBasicAccountInfo
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool IsMigrateData { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
    }
}
