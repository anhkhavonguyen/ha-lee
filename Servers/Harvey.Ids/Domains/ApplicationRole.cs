using Microsoft.AspNetCore.Identity;

namespace Harvey.Ids.Domains
{
    public class ApplicationRole: IdentityRole<string>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName;
        }
    }
}
