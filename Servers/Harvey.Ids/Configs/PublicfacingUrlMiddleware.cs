using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Configs
{
    public class PublicfacingUrlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _publicFacingUri;

        public PublicfacingUrlMiddleware(RequestDelegate next, string publicFacingUri)
        {
            _next = next;
            _publicFacingUri = publicFacingUri;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            context.SetIdentityServerOrigin(_publicFacingUri);
            context.SetIdentityServerBasePath(request.PathBase.Value.TrimEnd('/'));
            await _next(context);
        }

    }
}
