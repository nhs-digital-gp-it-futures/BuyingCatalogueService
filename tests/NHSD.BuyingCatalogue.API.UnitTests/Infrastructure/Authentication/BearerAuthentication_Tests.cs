using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.API.Infrastructure.Authentication;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.API.UnitTests.Infrastructure.Authentication
{
    [TestFixture]
    public sealed class BearerAuthentication_Tests
    {
        private Mock<IConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _config = new Mock<IConfiguration>();
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
        public async Task OnTokenValidated_Adds_PublicRole()
        {
            var ctx = Creator.GetTokenValidatedContext();
            var auth = Create();

            await auth.OnTokenValidated(ctx);

            ctx.Principal.Claims
                .ShouldContain(x =>
                    x.Type == ClaimTypes.Role && x.Value == Roles.Public);
        }

        private BearerAuthentication Create()
        {
            return new BearerAuthentication(_config.Object);
        }
    }
}
