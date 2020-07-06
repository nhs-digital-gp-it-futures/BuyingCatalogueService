using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
	public class ProvisioningTypeTests
	{
        [Test]
        public void EnsureProvioningTypeNamingMatchesOrdapi()
        {
            //Arrange
            var patient = Enumerator.FromName<ProvisioningType>("Patient");
            var declarative = Enumerator.FromName<ProvisioningType>("Declarative");
            var onDemand = Enumerator.FromName<ProvisioningType>("OnDemand");

            //Assert
            patient.Name.Should().Be("Patient");
            declarative.Name.Should().Be("Declarative");
            onDemand.Name.Should().Be("OnDemand");
        }
    }
}
