using System.Linq;
using System.Security.Claims;

namespace Harvey.Ids.Utils
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            var idClaim = claims.Claims.FirstOrDefault(c => c.Type == "sub");
            if (idClaim == null)
                return null;
            return idClaim.Value;
        }
    }
}
