using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class CatalogueItemsControllerTests
    {
        [Test]
        public void Constructor_NullMediator_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new CatalogueItemsController(null));
        }

        [Test]
        public async Task GetAsync_CatalogueItemFound_ReturnsExpectedResponse()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            var response = await context.Controller.GetAsync(context.GetCatalogueItemDtoResult.CatalogueItemId);

            var expected = GetCatalogueItemResultBuilder
                .Create()
                .WithCatalogueItemId(context.GetCatalogueItemDtoResult.CatalogueItemId)
                .WithName(context.GetCatalogueItemDtoResult.Name)
                .Build();

            response.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_NullCatalogueItem_ReturnsEmptyGetCatalogueItemResult()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            context.GetCatalogueItemDtoResult = null;

            var response = await context.Controller.GetAsync(string.Empty);

            var expected = GetCatalogueItemResultBuilder
                .Create()
                .WithCatalogueItemId(null)
                .WithName(null)
                .Build();

            response.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_Mediator_Send_CalledOnce()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            var catalogueItemId = context.GetCatalogueItemDtoResult.CatalogueItemId;

            await context.Controller.GetAsync(catalogueItemId);

            context.MediatorMock.Verify(x =>
                x.Send(It.IsNotNull<GetCatalogueItemByIdQuery>(), default), Times.Once);
        }

        [Test]
        public async Task ListAsync_CatalogueItemsExist_ReturnsExpectedResponse()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            var catalogueItem1 = CatalogueItemDtoBuilder
                .Create()
                .Build();

            var catalogueItem2 = CatalogueItemDtoBuilder
                .Create()
                .Build();

            context.ListCatalogueItemsDtoResult.Add(catalogueItem1);
            context.ListCatalogueItemsDtoResult.Add(catalogueItem2);

            var response = await context.Controller.ListAsync(
                "sup1",
                CatalogueItemType.Solution,
                PublishedStatus.Published);

            var expected = context.ListCatalogueItemsDtoResult.Select(CatalogueItemResultBuilder).ToList();

            response.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ListAsync_NoCatalogueItemsExist_ReturnsEmptyList()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            var response = await context.Controller.ListAsync(
                "sup1",
                CatalogueItemType.Solution,
                PublishedStatus.Published);

            response.Value.Should().BeEmpty();
        }

        [Test]
        public async Task ListAsync_Mediator_Send_CalledOnce()
        {
            var context = CatalogueItemsControllerTestContext.Create();
            await context.Controller.ListAsync(
                "sup1",
                CatalogueItemType.Solution,
                PublishedStatus.Published);

            context.MediatorMock.Verify(m => m.Send(It.IsNotNull<ListCatalogueItemQuery>(), default));
        }

        private static GetCatalogueItemResult CatalogueItemResultBuilder(CatalogueItemDto catalogueItem)
        {
            return GetCatalogueItemResultBuilder
                .Create()
                .WithCatalogueItemId(catalogueItem.CatalogueItemId)
                .WithName(catalogueItem.Name)
                .Build();
        }

        private sealed class CatalogueItemsControllerTestContext
        {
            private CatalogueItemsControllerTestContext()
            {
                GetCatalogueItemDtoResult = CatalogueItemDtoBuilder
                    .Create()
                    .Build();

                ListCatalogueItemsDtoResult = new List<CatalogueItemDto>();

                MediatorMock = new Mock<IMediator>();
                MediatorMock.Setup(x => x.Send(It.IsAny<GetCatalogueItemByIdQuery>(), default)).ReturnsAsync(() => GetCatalogueItemDtoResult);

                MediatorMock.Setup(x => x.Send(It.IsAny<ListCatalogueItemQuery>(), default)).ReturnsAsync(() =>
                    ListCatalogueItemsDtoResult);

                Controller = new CatalogueItemsController(MediatorMock.Object);
            }

            internal CatalogueItemDto GetCatalogueItemDtoResult { get; set; }

            internal List<CatalogueItemDto> ListCatalogueItemsDtoResult { get; set; }

            internal Mock<IMediator> MediatorMock { get; }

            internal CatalogueItemsController Controller { get; }

            internal static CatalogueItemsControllerTestContext Create() =>
                new CatalogueItemsControllerTestContext();
        }
    }
}
