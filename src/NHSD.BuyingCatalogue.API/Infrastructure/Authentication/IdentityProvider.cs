using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Authentication
{
    public sealed class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _context;

        public IdentityProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ClaimsPrincipal Identity => _context.HttpContext.User;
    }
}
