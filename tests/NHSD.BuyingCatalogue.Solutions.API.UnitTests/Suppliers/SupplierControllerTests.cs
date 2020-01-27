using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
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
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _supplierController = new SupplierController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateSupplierCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
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

        [Test]
        public async Task UpdateSupplier()
        {
            var request = new UpdateSupplierViewModel();

            var result =
                (await _supplierController.Update(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateSupplierCommand>(c =>
                        c.SolutionId == _solutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("description", "maxLength");
            _resultDictionary.Add("link", "maxLength");

            var request = new UpdateSupplierViewModel();

            var result =
                (await _supplierController.Update(_solutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["description"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateSupplierCommand>(c =>
                        c.SolutionId == _solutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

