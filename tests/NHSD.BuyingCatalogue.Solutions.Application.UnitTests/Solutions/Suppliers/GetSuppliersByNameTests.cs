using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Suppliers
{
    [TestFixture]
    internal sealed class GetSuppliersByNameTests
    {
        private readonly TestContext context = new();

        [Test]
        public async Task Handle_ReturnsExpectedResults()
        {
            const string supplier = "Supplier";
            const PublishedStatus solutionStatus = PublishedStatus.Published;
            const CatalogueItemType itemType = CatalogueItemType.Solution;

            static ISupplierResult MockSupplierNameResult(string id, string name)
            {
                var mockResult = new Mock<ISupplierResult>();
                mockResult.Setup(r => r.Id).Returns(id);
                mockResult.Setup(r => r.Name).Returns(name);

                return mockResult.Object;
            }

            var cancellationToken = default(CancellationToken);
            var expectedResult = new[]
            {
                MockSupplierNameResult("1", "Supplier 1"),
                MockSupplierNameResult("2", "Supplier 2"),
            };

            context.MockSupplierRepository
                .Setup(r => r.GetSuppliersByNameAsync(supplier, solutionStatus, itemType, cancellationToken))
                .ReturnsAsync(expectedResult);

            var actualResult = await context.GetSuppliersByNameHandler.Handle(
                new GetSuppliersByNameQuery(supplier, solutionStatus, itemType),
                cancellationToken);

            actualResult.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }
    }
}
