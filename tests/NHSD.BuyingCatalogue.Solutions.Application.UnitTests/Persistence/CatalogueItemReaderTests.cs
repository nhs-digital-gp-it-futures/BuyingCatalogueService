using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Persistence
{
    [TestFixture]
    internal sealed class CatalogueItemReaderTests
    {
        [Test]
        public void Constructor_NullCatalogueItemRepository_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new CatalogueItemReader(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsExpectedValue()
        {
            var expected = new CatalogueItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var catalogueItemResultMock = new Mock<ICatalogueItemResult>();
            catalogueItemResultMock.Setup(r => r.CatalogueItemId).Returns(expected.CatalogueItemId);
            catalogueItemResultMock.Setup(r => r.Name).Returns(expected.Name);

            var catalogueItemRepositoryMock = new Mock<ICatalogueItemRepository>();
            catalogueItemRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>(), default))
                .ReturnsAsync(() => catalogueItemResultMock.Object);

            var reader = new CatalogueItemReader(catalogueItemRepositoryMock.Object);
            var actual = await reader.GetByIdAsync(string.Empty, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetByIdAsync_CatalogueItemNotFound_ThrowsNotFoundException()
        {
            var catalogueItemRepositoryMock = new Mock<ICatalogueItemRepository>();
            catalogueItemRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>(), default))
                .ReturnsAsync(() => null);

            var reader = new CatalogueItemReader(catalogueItemRepositoryMock.Object);
            Assert.ThrowsAsync<NotFoundException>(() => reader.GetByIdAsync(string.Empty, CancellationToken.None));
        }

        [Test]
        public async Task GetByIdAsync_CatalogueItemRepository_GetByIdAsync_CalledOnce()
        {
            var expected = new CatalogueItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var catalogueItemId = expected.CatalogueItemId;

            var catalogueItemResultMock = new Mock<ICatalogueItemResult>();
            catalogueItemResultMock.Setup(r => r.CatalogueItemId).Returns(catalogueItemId);
            catalogueItemResultMock.Setup(r => r.Name).Returns(expected.Name);

            var catalogueItemRepositoryMock = new Mock<ICatalogueItemRepository>();
            catalogueItemRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>(), default))
                .ReturnsAsync(() => catalogueItemResultMock.Object);

            var reader = new CatalogueItemReader(catalogueItemRepositoryMock.Object);
            await reader.GetByIdAsync(catalogueItemId, CancellationToken.None);

            catalogueItemRepositoryMock.Verify(x => 
                x.GetByIdAsync(catalogueItemId, default), Times.Once);
        }
    }
}
