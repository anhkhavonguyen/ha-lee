using System;
using System.Linq;
using System.Security.Claims;

namespace Harvey.PIM.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var result = user.Claims.FirstOrDefault(x => x.Type == "sub");
            if (result == null)
            {
                throw new UnauthorizedAccessException();
            }
            return Guid.Parse(result.Value);
        }
    }
}
