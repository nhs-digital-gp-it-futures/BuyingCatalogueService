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

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionBrowserAdditionalInformationViewModel();

            var validationResult = new UpdateSolutionBrowserAdditionalInformationValidationResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
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

            var validationResult = new UpdateSolutionBrowserAdditionalInformationValidationResult()
            {
                MaxLength = { "additional-information" }
            };

            _mockMediator.Setup(m => m.Send(
                It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                    q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel == viewModel),
                It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionBrowserAdditionalInformationResult).MaxLength.Should().BeEquivalentTo(new[] { "additional-information" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserAdditionalInformationViewModel ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
