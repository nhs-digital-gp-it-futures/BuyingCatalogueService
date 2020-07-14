﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.CatalogueItems
{
    [TestFixture]
    internal sealed class ListCatalogueItemHandlerTests
    {
        [Test]
        public void Constructor_NullCatalogueItemReader_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ListCatalogueItemHandler(null));
        }

        [Test]
        public async Task Handle_Query_ReturnsExpectedValue()
        {
            var catalogueItemsResult = new List<ICatalogueItemResult>
            {
                Mock.Of<ICatalogueItemResult>(r => r.CatalogueItemId == "Id1" && r.Name == "Name 1"),
                Mock.Of<ICatalogueItemResult>(r => r.CatalogueItemId == "Id2" && r.Name == "Name2"),
            };

            var catalogueItemRepositoryMock = new Mock<ICatalogueItemRepository>();
            catalogueItemRepositoryMock.Setup(x => x.ListAsync(It.IsAny<string>(), It.IsAny<CatalogueItemType?>(), default))
                .ReturnsAsync(() => catalogueItemsResult);

            var handler = new ListCatalogueItemHandler(new CatalogueItemReader(catalogueItemRepositoryMock.Object));
            var actual = await handler.Handle(null, default);

            var expected = catalogueItemsResult.Select(x => new CatalogueItemDto(x.CatalogueItemId, x.Name));

            actual.Should().BeEquivalentTo(expected);
        }
    }
}