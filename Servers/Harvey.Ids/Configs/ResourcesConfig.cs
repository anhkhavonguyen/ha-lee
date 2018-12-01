using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Harvey.Ids.Configs
{
    public static class ResourcesConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Harvey.Activity.Api", "Harvey Activity Api"){
                    UserClaims = new []{ ClaimTypes.Role, JwtClaimTypes.Role }
                },
                new ApiResource("Harvey.CRMLoyalty.Api", "Harvey CRMLoyalty Api"){
                     UserClaims = new []{ ClaimTypes.Role, JwtClaimTypes.Role }
                },
                new ApiResource("Harvey.UserManagement.Api", "Harvey UserManagement Api"){
                     UserClaims = new []{ ClaimTypes.Role, JwtClaimTypes.Role }
                },
                new ApiResource("Harvey.Notification.Api", "Harvey Notification Api"){
                     UserClaims = new []{ ClaimTypes.Role, JwtClaimTypes.Role }
                }
                ,new ApiResource("harvey.rims.api", "harvey rims api"){
                     UserClaims = new []{ ClaimTypes.Role, JwtClaimTypes.Role },
                     ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>() {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource{
                    Name = "roles",
                    UserClaims = new []{ JwtClaimTypes.Role }
                }
            };
        }
    }
}
