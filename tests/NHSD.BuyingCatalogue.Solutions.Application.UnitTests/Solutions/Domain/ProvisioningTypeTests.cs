using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
    public sealed class ProvisioningTypeTests
    {
        [TestCase("Patient")]
        [TestCase("Declarative")]
        [TestCase("OnDemand")]
        public void EnsureProvisioningTypeNamingMatchesOrdapi(string name)
        {
            //Arrange
            ProvisioningType provisioningType = Enumerator.FromName<ProvisioningType>(name);

            //Assert
            provisioningType.Name.Should().Be(name);
        }
    }
}
