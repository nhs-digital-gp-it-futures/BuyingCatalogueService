using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    internal sealed class SolutionSupplierControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private SolutionSupplierController solutionSupplierController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            solutionSupplierController = new SolutionSupplierController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionSupplierCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("Some description", "Some link")]
        [TestCase("Some description", null)]
        [TestCase(null, "Some link")]
        [TestCase(null, null)]
        public async Task PopulatedAboutSupplierShouldReturnCorrectDetails(string summary, string url)
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetSupplierBySolutionIdQuery>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolutionSupplier>(s => s.Summary == summary && s.Url == url));

            var result = await solutionSupplierController.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetSolutionSupplierResult>();

            var aboutSupplierResult = result.Value as GetSolutionSupplierResult;

            Assert.NotNull(aboutSupplierResult);
            aboutSupplierResult.Description.Should().Be(summary);
            aboutSupplierResult.Link.Should().Be(url);
        }

        [Test]
        public async Task NullAboutSupplierShouldReturnNull()
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetSupplierBySolutionIdQuery>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as ISolutionSupplier);

            var result = await solutionSupplierController.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetSolutionSupplierResult>();

            var aboutSupplierResult = result.Value as GetSolutionSupplierResult;

            Assert.NotNull(aboutSupplierResult);
            aboutSupplierResult.Description.Should().BeNull();
            aboutSupplierResult.Link.Should().BeNull();
        }

        [Test]
        public async Task UpdateSupplier()
        {
            var request = new UpdateSolutionSupplierViewModel();

            var result = await solutionSupplierController.Update(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateSolutionSupplierCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("description", "maxLength");
            resultDictionary.Add("link", "maxLength");

            var request = new UpdateSolutionSupplierViewModel();

            var result = await solutionSupplierController.Update(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["description"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateSolutionSupplierCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
