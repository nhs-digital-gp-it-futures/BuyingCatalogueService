using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.API.Infrastructure.Authentication;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.UnitTests.Infrastructure.Authentication
{
    [TestFixture]
    public sealed class BearerAuthentication_Tests
    {
        private Mock<IConfiguration> _config;
        private Mock<IRolesProvider> _rolesProvider;
        private Mock<ILogger<BearerAuthentication>> _logger;

        [SetUp]
        public void SetUp()
        {
            _config = new Mock<IConfiguration>();
            _rolesProvider = new Mock<IRolesProvider>();
            _logger = new Mock<ILogger<BearerAuthentication>>();
        }

        [Test]
        public async Task OnMessageReceived_NoBearer_PublicRole()
        {
            var ctx = Creator.GetMessageReceivedContext();
            var auth = Create();

            await auth.OnMessageReceived(ctx);

            ctx.Principal.Claims
                .ShouldContain(x =>
                    x.Type == ClaimTypes.Role && x.Value == Roles.Public);
        }

        [Test]
        public async Task OnMessageReceived_NoBearer_BlankEmail()
        {
            var ctx = Creator.GetMessageReceivedContext();
            var auth = Create();

            await auth.OnMessageReceived(ctx);

            ctx.Principal.Claims
                .ShouldContain(x =>
                    x.Type == ClaimTypes.Email && x.Value == string.Empty);
        }

        [Test]
        public async Task OnMessageReceived_NoBearer_BlankAltEmail()
        {
            var ctx = Creator.GetMessageReceivedContext();
            var auth = Create();

            await auth.OnMessageReceived(ctx);

            ctx.Principal.Claims
                .ShouldContain(x =>
                    x.Type == "email" && x.Value == string.Empty);
        }

        [Test]
        public async Task OnMessageReceived_NoBearer_Authenticates()
        {
            var ctx = Creator.GetMessageReceivedContext();
            var auth = Create();

            await auth.OnMessageReceived(ctx);

            ctx.Result.Succeeded.ShouldBe(true);
        }

        [Test]
        public async Task OnMessageReceived_Bearer_DoesNotAuthenticate()
        {
            var ctx = Creator.GetMessageReceivedContext();
            ctx.HttpContext.Request.Headers.Add("Authorization", JwtBearerDefaults.AuthenticationScheme + " token_goes_here");
            var auth = Create();

            await auth.OnMessageReceived(ctx);

            ctx.Result.ShouldBeNull();
        }

        [Test]
        public async Task OnTokenValidated_CallsRolesProvider_WithClaimEmail()
        {
            const string email = "NHSD@hscic.gov.uk";

            var ctx = Creator.GetTokenValidatedContext(email);
            var auth = Create();

            await auth.OnTokenValidated(ctx);

            _rolesProvider.Verify(x => x.RolesByEmail(email), Times.Once);
        }

        [Test]
        public async Task OnTokenValidated_UsesRolesProvider_Claims()
        {
            var roles = new List<string> { "Role1", "Role2" };
            var ctx = Creator.GetTokenValidatedContext();
            var auth = Create();
            _rolesProvider.Setup(x => x.RolesByEmail(It.IsAny<string>()))
                .Returns(roles);

            await auth.OnTokenValidated(ctx);

            var validatedRoles = ctx.Principal.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(claim => claim.Value);
            roles.ShouldBeSubsetOf(validatedRoles);
        }

        private BearerAuthentication Create()
        {
            return new BearerAuthentication(
                _config.Object,
                _rolesProvider.Object,
                _logger.Object);
        }
    }
}
