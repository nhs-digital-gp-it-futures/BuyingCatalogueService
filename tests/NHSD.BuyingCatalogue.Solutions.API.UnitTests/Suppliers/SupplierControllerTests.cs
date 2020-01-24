using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Suppliers
{
    [TestFixture]
    public sealed class SupplierControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private SupplierController _supplierController;
        private readonly string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _supplierController = new SupplierController(_mediatorMock.Object);
        }


        [TestCase("Some description", "Some link")]
        [TestCase("Some description", null)]
        [TestCase(null, "Some link")]
        [TestCase(null, null)]
        public async Task PopulatedAboutSupplierShouldReturnCorrectDetails(string summary, string url)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetSupplierBySolutionIdQuery>(q => q.SolutionId == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISupplier>(s => s.Summary == summary && s.Url == url));
            var result = await _supplierController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetSupplierResult>();

            var aboutSupplierResult = result.Value as GetSupplierResult;
            aboutSupplierResult.SupplierDescription.Should().Be(summary);
            aboutSupplierResult.SupplierLink.Should().Be(url);
        }

        [Test]
        public async Task NullAboutSupplierShouldReturnNull()
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetSupplierBySolutionIdQuery>(q => q.SolutionId == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as ISupplier);

            var result = await _supplierController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetSupplierResult>();

            var aboutSupplierResult = result.Value as GetSupplierResult;
            aboutSupplierResult.SupplierDescription.Should().BeNull();
            aboutSupplierResult.SupplierLink.Should().BeNull();
        }
    }
}

