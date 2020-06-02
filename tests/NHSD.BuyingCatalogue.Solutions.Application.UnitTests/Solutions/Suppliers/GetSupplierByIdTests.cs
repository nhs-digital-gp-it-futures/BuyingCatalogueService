using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Suppliers
{
    [TestFixture]
    internal sealed class GetSupplierByIdTests
    {
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly TestContext _context = new TestContext();

        [Test]
        public async Task Handle_ReturnsExpectedResults()
        {
            const string supplier = "10000";

            var mockResult = new Mock<ISupplierResult>();
            mockResult.Setup(r => r.Id).Returns(supplier);
            mockResult.Setup(r => r.Name).Returns("Healthy Supplier");
            mockResult.Setup(r => r.AddressLine1).Returns("Address line 1");
            mockResult.Setup(r => r.PrimaryContactFirstName).Returns("Bob");
            mockResult.Setup(r => r.HasAddress).Returns(true);
            mockResult.Setup(r => r.HasContact).Returns(true);

            var expectedSupplier = mockResult.Object;

            _context.MockSupplierRepository
                .Setup(r => r.GetSupplierById(supplier, _cancellationToken))
                .ReturnsAsync(expectedSupplier);

            var actualResult = await _context.GetSupplierByIdHandler.Handle(
                new GetSupplierByIdQuery(supplier),
                _cancellationToken);

            actualResult.Id.Should().BeEquivalentTo(expectedSupplier.Id);
            actualResult.Name.Should().BeEquivalentTo(expectedSupplier.Name);
            actualResult.Address.Line1.Should().BeEquivalentTo(expectedSupplier.AddressLine1);
            actualResult.PrimaryContact.FirstName.Should().BeEquivalentTo(expectedSupplier.PrimaryContactFirstName);
        }
    }
}
