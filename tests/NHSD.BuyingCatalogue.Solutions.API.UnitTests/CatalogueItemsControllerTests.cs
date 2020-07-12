using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
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

        private sealed class CatalogueItemsControllerTestContext
        {
            private CatalogueItemsControllerTestContext()
            {
                GetCatalogueItemDtoResult = CatalogueItemDtoBuilder
                    .Create()
                    .Build();

                MediatorMock = new Mock<IMediator>();
                MediatorMock.Setup(x => x.Send(It.IsAny<GetCatalogueItemByIdQuery>(), default)).ReturnsAsync(() => GetCatalogueItemDtoResult);

                Controller = new CatalogueItemsController(MediatorMock.Object);
            }

            internal CatalogueItemDto GetCatalogueItemDtoResult { get; set; }

            internal Mock<IMediator> MediatorMock { get; }

            internal CatalogueItemsController Controller { get; }

            internal static CatalogueItemsControllerTestContext Create() =>
                new CatalogueItemsControllerTestContext();
        }
    }
}
