using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure.Authentication
{
    [TestFixture]
    public sealed class RolesProvider_Tests
    {
        [TestCase("some_email@somewhere.com")]
        [TestCase("some_arbitrary_string")]
        [TestCase("")]
        [TestCase(null)]
        public void RolesByEmail_Includes_PublicRole(string email)
        {
            var roleProvider = Create();

            var roles = roleProvider.RolesByEmail(email);

            roles
                .ShouldContain(x =>
                    x == Roles.Public);
        }

        private RolesProvider Create()
        {
            return new RolesProvider();
        }
    }
}
