using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers;
using NHSD.BuyingCatalogue.Solutions.API.QueryModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
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

            response.Should().BeEquivalentTo(new ActionResult<GetSupplierModel>(new NotFoundResult()));
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

            var response = await controller.Get(id);

            response.Should().BeEquivalentTo(new ActionResult<GetSupplierModel>(new OkObjectResult(expectedSupplier)));
        }

        [Test]
        public async Task GetList_SuppliersFound_ReturnsExpectedResponse()
        {
            var supplier1 = MockSupplier("1", "Uncle Bob's Miracle Cures");
            var supplier2 = MockSupplier("2", "Uncle Bob's Magic Mead");

            var expectedSuppliers = new[]
            {
                new GetSupplierModel(supplier1),
                new GetSupplierModel(supplier2),
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<IEnumerable<ISupplier>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { supplier1, supplier2 });

            var controller = new SuppliersController(mockMediator.Object);

            var response = await controller.GetList(new SupplierSearchQueryModel());

            response.Should().BeEquivalentTo(
                new ActionResult<IEnumerable<GetSupplierModel>>(new OkObjectResult(expectedSuppliers)));
        }

        [Test]
        public async Task GetList_NoSuppliersFound_ReturnsExpectedResponse()
        {
            var expectedSuppliers = Array.Empty<GetSupplierModel>();

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<IEnumerable<ISupplier>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ISupplier>());

            var controller = new SuppliersController(mockMediator.Object);

            var response = await controller.GetList(new SupplierSearchQueryModel());

            response.Should().BeEquivalentTo(
                new ActionResult<IEnumerable<GetSupplierModel>>(new OkObjectResult(expectedSuppliers)));
        }

        [Test]
        public async Task GetList_WithItemType_PassesExpectedQuery()
        {
            GetSuppliersByNameQuery actualQuery = null;

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<IEnumerable<ISupplier>>>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<ISupplier>>, CancellationToken>((q, t) => actualQuery = (GetSuppliersByNameQuery)q)
                .ReturnsAsync(Array.Empty<ISupplier>());

            const string expectedName = "Supplier Name";
            const CatalogueItemType expectedCatalogueItemType = CatalogueItemType.AdditionalService;
            const PublishedStatus expectedPublishedStatus = PublishedStatus.Draft;

            var controller = new SuppliersController(mockMediator.Object);
            var searchQuery = new SupplierSearchQueryModel
            {
                CatalogueItemType = expectedCatalogueItemType,
                Name = expectedName,
                SolutionPublicationStatus = expectedPublishedStatus,
            };

            await controller.GetList(searchQuery);

            var expectedQuery = new GetSuppliersByNameQuery(expectedName, expectedPublishedStatus, expectedCatalogueItemType);

            actualQuery.Should().BeEquivalentTo(expectedQuery);
        }

        [Test]
        public async Task GetList_WithoutItemType_PassesExpectedQuery()
        {
            GetSuppliersByNameQuery actualQuery = null;

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsNotNull<IRequest<IEnumerable<ISupplier>>>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<ISupplier>>, CancellationToken>((q, t) => actualQuery = (GetSuppliersByNameQuery)q)
                .ReturnsAsync(Array.Empty<ISupplier>());

            const string expectedName = "Supplier Name";
            const PublishedStatus expectedPublishedStatus = PublishedStatus.Draft;

            var controller = new SuppliersController(mockMediator.Object);
            var searchQuery = new SupplierSearchQueryModel
            {
                Name = expectedName,
                SolutionPublicationStatus = expectedPublishedStatus,
            };

            await controller.GetList(searchQuery);

            var expectedQuery = new GetSuppliersByNameQuery(expectedName, expectedPublishedStatus, CatalogueItemType.Solution);

            actualQuery.Should().BeEquivalentTo(expectedQuery);
        }

        private static ISupplier MockSupplier(string id, string name)
        {
            var mockSupplier = new Mock<ISupplier>();
            mockSupplier.Setup(s => s.Id).Returns(id);
            mockSupplier.Setup(s => s.Name).Returns(name);

            return mockSupplier.Object;
        }
    }
}
