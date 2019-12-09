using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.API.Infrastructure;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    internal sealed class SettingsTests
    {
        [Test]
        public void ShouldReadConnectionString()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x["ConnectionStrings__BuyingCatalogue"]).Returns("ConnectionString");

            var settings = new Settings(configuration.Object);
            settings.ConnectionString.Should().Be("ConnectionString");
        }
    }
}
