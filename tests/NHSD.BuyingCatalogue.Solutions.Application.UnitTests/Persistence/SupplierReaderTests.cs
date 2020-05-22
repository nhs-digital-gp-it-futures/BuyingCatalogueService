using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Persistence
{
    [TestFixture]
    internal sealed class SupplierReaderTests
    {
        [Test]
        public async Task ByNameAsync_ReturnsExpectedValues()
        {
            const string supplierName = "Supplier";

            static ISupplierNameResult MockSupplierNameResult(string id, string name)
            {
                var mockResult = new Mock<ISupplierNameResult>();
                mockResult.Setup(r => r.Id).Returns(id);
                mockResult.Setup(r => r.Name).Returns(name);

                return mockResult.Object;
            }

            var expectedSuppliers = new[]
            {
                MockSupplierNameResult("1", "Supplier 1"),
                MockSupplierNameResult("2", "Supplier 2")
            };

            var mockRepo = new Mock<ISupplierRepository>();
            mockRepo.Setup(
                r => r.GetSuppliersByName(supplierName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSuppliers);

            var reader = new SupplierReader(mockRepo.Object);

            var actualSuppliers = await reader.ByNameAsync(supplierName, new CancellationToken());

            actualSuppliers.Should().BeEquivalentTo(expectedSuppliers, c => c.IncludingAllDeclaredProperties());
        }
    }
}
