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
    internal sealed class GetSuppliersByNameTests
    {
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly TestContext _context = new TestContext();

        [Test]
        public async Task Handle_ReturnsExpectedResults()
        {
            static ISupplierNameResult MockSupplierNameResult(string id, string name)
            {
                var mockResult = new Mock<ISupplierNameResult>();
                mockResult.Setup(r => r.Id).Returns(id);
                mockResult.Setup(r => r.Name).Returns(name);

                return mockResult.Object;
            }

            var expectedResult = new[]
            {
                MockSupplierNameResult("1", "Supplier 1"),
                MockSupplierNameResult("2", "Supplier 2")
            };

            _context.MockSupplierRepository
                .Setup(r => r.GetSuppliersByName("sup", _cancellationToken))
                .ReturnsAsync(expectedResult);

            var actualResult = await _context.GetSuppliersByNameHandler.Handle(
                new GetSuppliersByNameQuery("Supplier"),
                _cancellationToken);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
