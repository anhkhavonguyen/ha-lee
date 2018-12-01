using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace Harvey.Ids.Configs
{
    public static class ClientsConfig
    {
        public static readonly string HarveyMemberPage = "Harvey-member-page";
        public static readonly string HarveyAdministratorPage = "Harvey-administrator-page";
        public static readonly string HarveyStaffPage = "Harvey-staff-page";
        public const string HarveyRimsPage = "harvey-rims-page";

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    ClientId = HarveyMemberPage,
                    ClientName = HarveyMemberPage,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        "Harvey.CRMLoyalty.Api",
                        "Harvey.Activity.Api",
                        "Harvey.UserManagement.Api",
                        "roles"
                    },
                    RedirectUris = HarveyMemberPageUrls,
                    RequireConsent = false,
                    AccessTokenLifetime = 1300
                },
                new Client
                {
                    Enabled = true,
                    ClientId = HarveyAdministratorPage,
                    ClientName = HarveyAdministratorPage,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        "Harvey.CRMLoyalty.Api",
                        "Harvey.Activity.Api",
                        "Harvey.UserManagement.Api",
                        "Harvey.Notification.Api",
                        "roles"
                    },
                    RequireConsent = false,
                    RedirectUris = HarveyAdministratorPageUrls,
                    AccessTokenLifetime = 1300
                },
                new Client
                {
                    Enabled = true,
                    ClientId = HarveyStaffPage,
                    ClientName = HarveyStaffPage,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        "Harvey.CRMLoyalty.Api",
                        "Harvey.Activity.Api",
                        "Harvey.UserManagement.Api",
                        "roles"
                    },
                    RequireConsent = false,
                    RedirectUris = HarveyStorePageUrls,
                    AccessTokenLifetime = 36000
                },
                new Client
                {
                    Enabled = true,
                    ClientId = HarveyRimsPage,
                    ClientName = HarveyRimsPage,
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        "harvey.rims.api",
                        "roles"
                    },
                    RequireConsent = false,
                    RedirectUris = HarveyRimsRedirectUris,
                    PostLogoutRedirectUris = HarveyRimsPostLogoutRedirectUris,
                    AllowedCorsOrigins = HarveyRimsRedirectUris,
                    AccessTokenLifetime = 36000
                }
            };
        }

        public static List<string> HarveyMemberPageUrls => new List<string> {
            "https://localhost:4600",
            "https://localhost:50222",
            "https://178.128.212.67:50222",
            "https://crm.dev.retaildds.net:50222",
            "https://member.app.dev.toyorgame.com.sg",
            "https://member.app.toyorgame.com.sg"
        };

        public static List<string> HarveyAdministratorPageUrls => new List<string> {
            "https://localhost:4100",
            "https://localhost:50221",
            "https://178.128.212.67:50221",
            "https://crm.dev.retaildds.net:50221",
            "https://admin.app.dev.toyorgame.com.sg",
            "https://admin.app.toyorgame.com.sg"

        };

        public static List<string> HarveyStorePageUrls => new List<string> {
            "https://localhost:4500",
            "https://localhost:50223",
            "http://178.128.212.67:50223",
            "https://crm.dev.retaildds.net:50223",
            "https://store.app.dev.toyorgame.com.sg",
            "https://store.app.toyorgame.com.sg"
        };

        public static List<string> HarveyRimsRedirectUris => new List<string> {
            "http://localhost:4100/index.html",
            "http://localhost:4200/index.html",
            "http://localhost:4300/index.html",
            "http://192.168.70.170:4100/index.html",
            "http://192.168.70.170:4200/index.html",
            "http://192.168.70.170:4300/index.html",
            "http://178.128.212.67:4100/index.html",
            "http://178.128.212.67:4200/index.html",
            "http://178.128.212.67:4300/index.html"
        };

        public static List<string> HarveyRimsPostLogoutRedirectUris => new List<string> {
            "http://localhost:4100",
            "http://localhost:4200",
            "http://localhost:4300",
            "http://192.168.70.170:4100",
            "http://192.168.70.170:4200",
            "http://192.168.70.170:4300",
            "http://178.128.212.67:4100",
            "http://178.128.212.67:4200",
            "http://178.128.212.67:4300"
        };

        private static string[] GetRedirectUris(IEnumerable<string> clientUrls, string login, string silentRenew)
        {
            var ret = new List<string>();
            foreach (var clientUrl in clientUrls)
            {
                ret.Add(clientUrl + login);
                ret.Add(clientUrl + silentRenew);
            }
            return ret.ToArray();
        }
    }
}
