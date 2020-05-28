using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Suppliers
{
    [TestFixture]
    internal sealed class SuppliersControllerTests
    {
        [Test]
        public async Task Get_SupplierNotFound_ReturnsNotFound()
        {
            const string id = "1";

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<ISupplier>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISupplier)null);

            var controller = new SuppliersController(mockMediator.Object);

            var response = await controller.Get(id);
            var result = response.Result as NotFoundObjectResult;

            Assert.NotNull(result);
            result.Value.Should().BeEquivalentTo(id);
        }

        [Test]
        public async Task Get_SupplierFound_ReturnsExpectedResponse()
        {
            const string id = "1";

            var mockSupplier = new Mock<ISupplier>();
            mockSupplier.Setup(s => s.Id).Returns(id);
            mockSupplier.Setup(s => s.Name).Returns("Uncle Bob's Miracle Cures");

            var expectedSupplier = new GetSupplierModel(mockSupplier.Object);

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<ISupplier>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockSupplier.Object);

            var controller = new SuppliersController(mockMediator.Object);

            var result = await controller.Get(id);

            Assert.NotNull(result);
            result.Value.Should().BeEquivalentTo(expectedSupplier);
        }
    }
}
