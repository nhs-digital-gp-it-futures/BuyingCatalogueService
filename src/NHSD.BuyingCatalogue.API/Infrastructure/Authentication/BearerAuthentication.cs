using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;

namespace NHSD.BuyingCatalogue.API.Infrastructure.Authentication
{
    public sealed class BearerAuthentication : IBearerAuthentication
    {
        private const string AltEmail = "email";

        private readonly IConfiguration _config;
        private readonly IRolesProvider _rolesProvider;
        private readonly ILogger<BearerAuthentication> _logger;

        public BearerAuthentication(
          IConfiguration config,
          IRolesProvider rolesProvider,
          ILogger<BearerAuthentication> logger)
        {
            _config = config;
            _rolesProvider = rolesProvider;
            _logger = logger;
        }

        public Task OnMessageReceived(MessageReceivedContext context)
        {
            var authorization = context.Request.Headers["Authorization"];
            if (authorization.ToString().StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.InvariantCultureIgnoreCase))
            {
                // Bearer token, so use it
                _logger.LogInformation($"Found bearer token: {authorization}");
                return Task.CompletedTask;
            }

            _logger.LogInformation("No bearer token found");

            // everyone is Joe Public with a blank email address
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, Roles.Public),
                new Claim(ClaimTypes.Email, string.Empty),
                new Claim(AltEmail, string.Empty),
            };
            var claimIdent = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(new[] { claimIdent });
            context.Principal = principal;
            context.Success();

            return Task.CompletedTask;
        }

        public Task OnTokenValidated(TokenValidatedContext context)
        {
            // TODO     validate JWT token against _config.Jwt_UserInfo();

            // ClaimTypes.Email --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
            // but some OIDC providers use 'email'
            var email = context.Principal.Claims
                .FirstOrDefault(x =>
                    x.Type == ClaimTypes.Email ||
                    x.Type.ToLowerInvariant() == AltEmail)?.Value;

            var claims = _rolesProvider
                .RolesByEmail(email)
                .Select(role => new Claim(ClaimTypes.Role, role));

            context.Principal.AddIdentity(new ClaimsIdentity(claims));

            return Task.CompletedTask;
        }
    }
}
