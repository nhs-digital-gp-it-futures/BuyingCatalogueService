using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using NHSD.BuyingCatalogue.Application.Infrastructure;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    public static class Creator
    {
        public static DefaultHttpContext GetHttpContext(
          string role = Roles.Authority,
          string email = "NHS-GPIT@WigglyAmps.com")
        {
            var roleClaim = new Claim(ClaimTypes.Role, role);
            var emailClaim = new Claim(ClaimTypes.Email, email);
            var claimsIdentity = new ClaimsIdentity(new[] { roleClaim, emailClaim });
            var user = new ClaimsPrincipal(new[] { claimsIdentity });
            var ctx = new DefaultHttpContext { User = user };

            return ctx;
        }

        public static AuthenticationScheme GetAuthenticationScheme()
        {
            return new AuthenticationScheme("MyScheme", "My Scheme", typeof(DummyAuthenticationHandler));
        }

        public static MessageReceivedContext GetMessageReceivedContext()
        {
            var httpCtx = GetHttpContext();
            var scheme = GetAuthenticationScheme();
            var opts = new JwtBearerOptions();
            var ctx = new MessageReceivedContext(httpCtx, scheme, opts) { Principal = httpCtx.User };

            return ctx;
        }

        public static TokenValidatedContext GetTokenValidatedContext()
        {
            var httpCtx = GetHttpContext();
            var scheme = GetAuthenticationScheme();
            var opts = new JwtBearerOptions();
            var ctx = new TokenValidatedContext(httpCtx, scheme, opts) { Principal = httpCtx.User };

            return ctx;
        }
    }
}
