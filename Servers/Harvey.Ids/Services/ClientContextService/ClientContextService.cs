using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.ClientContextService
{
    internal class ClientContextService : IClientContextService
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientContextService(IIdentityServerInteractionService interaction, ConfigurationDbContext configurationDbContext)
        {
            _interaction = interaction;
            _configurationDbContext = configurationDbContext;
        }

        public async Task<IdentityServer4.EntityFramework.Entities.Client> GetClientContextAsync(string returnUrl)
        {
            AuthorizationRequest context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context == null)
            {
                var client = GetClient(returnUrl);
                return client;
            }
            return null;
        }

        private IdentityServer4.EntityFramework.Entities.Client GetClient(string returnUrl)
        {
            if (returnUrl == null) return null;

            var clients = GetAllClients();
            foreach (var client in clients)
            {
                var redirectUris = client.RedirectUris.Select(p => p.RedirectUri).ToList();
                if (redirectUris.Contains(returnUrl))
                {
                    return client;
                }
            }
            return null;
        }

        private List<IdentityServer4.EntityFramework.Entities.Client> GetAllClients()
        {
            return _configurationDbContext.Clients.Include(p => p.RedirectUris).Include(p => p.Properties).ToList();
        }
    }
}
