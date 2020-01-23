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
        private SupplierController _aboutSupplierController;
        private readonly string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _aboutSupplierController = new SupplierController(_mediatorMock.Object);
        }


        [TestCase("Some description", "Some link")]
        [TestCase("Some description", null)]
        [TestCase(null, "Some link")]
        [TestCase(null, null)]
        public async Task PopulatedAboutSupplierShouldReturnCorrectDetails(string description, string link)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetSupplierBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISupplier>(s => s.Description == description && s.Link == link));
            var result = await _aboutSupplierController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetSupplierResult>();

            var aboutSupplierResult = result.Value as GetSupplierResult;
            aboutSupplierResult.Description.Should().Be(description);
            aboutSupplierResult.Link.Should().Be(link);
        }

        [Test]
        public async Task NullAboutSupplierShouldReturnNull()
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetSupplierBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as ISupplier);

            var result = await _aboutSupplierController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetSupplierResult>();

            var aboutSupplierResult = result.Value as GetSupplierResult;
            aboutSupplierResult.Description.Should().BeNull();
            aboutSupplierResult.Link.Should().BeNull();
        }
    }
}

