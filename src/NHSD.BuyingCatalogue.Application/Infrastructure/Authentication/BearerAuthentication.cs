using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Authentication
{
    public sealed class BearerAuthentication : IBearerAuthentication
    {
        private const string AltEmail = "email";

        private readonly IConfiguration _config;

        public BearerAuthentication(
          IConfiguration config)
        {
            _config = config;
        }

        public Task OnMessageReceived(MessageReceivedContext context)
        {
            var authorization = context.Request.Headers["Authorization"];
            if (authorization.ToString().StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.InvariantCultureIgnoreCase))
            {
                // Bearer token, so use it
                return Task.CompletedTask;
            }

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
            var claims = new List<Claim>
            {
                // everyone is Joe Public
                new Claim(ClaimTypes.Role, Roles.Public)
            };

            // TODO     validate JWT token against _config.Jwt_UserInfo();

            // TODO     set roles based on email-->contact-->organisation-->org.PrimaryRoleId-->role

            // ClaimTypes.Email --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
            // but some OIDC providers use 'email'
            var email = context.Principal.Claims
                .FirstOrDefault(x =>
                    x.Type == ClaimTypes.Email ||
                    x.Type.ToLowerInvariant() == AltEmail)?.Value;

            //claims.Add(new Claim(ClaimTypes.Role, Roles.Authority));
            //claims.Add(new Claim(ClaimTypes.Role, Roles.Buyer));

            context.Principal.AddIdentity(new ClaimsIdentity(claims));

            return Task.CompletedTask;
        }
    }
}
