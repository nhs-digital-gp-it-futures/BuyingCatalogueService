using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.CatalogueItems
{
    [TestFixture]
    internal sealed class GetCatalogueItemByIdQueryHandlerTests
    {
        [Test]
        public void Constructor_NullCatalogueItemReader_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new GetCatalogueItemByIdQueryHandler(null));
        }

        [Test]
        public async Task Handle_Query_ReturnsExpectedValue()
        {
            var expected = new CatalogueItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var catalogueItemResultMock = new Mock<ICatalogueItemResult>();
            catalogueItemResultMock.Setup(r => r.CatalogueItemId).Returns(expected.CatalogueItemId);
            catalogueItemResultMock.Setup(r => r.Name).Returns(expected.Name);

            var catalogueItemRepositoryMock = new Mock<ICatalogueItemRepository>();
            catalogueItemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<string>(), default))
                .ReturnsAsync(() => catalogueItemResultMock.Object);

            var handler = new GetCatalogueItemByIdQueryHandler(new CatalogueItemReader(catalogueItemRepositoryMock.Object));

            var actual = await handler.Handle(null, default);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
