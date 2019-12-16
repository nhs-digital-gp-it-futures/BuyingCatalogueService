using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserAdditionalInformationControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowserAdditionalInformationController _browserAdditionalInformationController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserAdditionalInformationController = new BrowserAdditionalInformationController(_mockMediator.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        public async Task ShouldGetBrowserAdditionalInformation(string information)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.AdditionalInformation == information));

            var result = (await _browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                .ConfigureAwait(false)) as ObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browserAdditionalInformation = result?.Value as GetBrowserAdditionalInformationResult;

            browserAdditionalInformation?.AdditionalInformation.Should().Be(information);
            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result =
                (await _browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                    .ConfigureAwait(false)) as NotFoundResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionBrowserAdditionalInformationViewModel();

            var validationResult = new MaxLengthResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as NoContentResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateSolutionBrowserAdditionalInformationViewModel();

            var validationResult = new MaxLengthResult()
            {
                MaxLength = { "additional-information" }
            };

            _mockMediator.Setup(m => m.Send(
                It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                    q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel == viewModel),
                It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as BadRequestObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result?.Value as UpdateSolutionBrowserAdditionalInformationResult)?.MaxLength.Should().BeEquivalentTo(new[] { "additional-information" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
