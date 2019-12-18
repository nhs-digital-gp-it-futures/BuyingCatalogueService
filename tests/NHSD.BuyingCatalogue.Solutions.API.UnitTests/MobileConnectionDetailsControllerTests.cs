using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class MobileConnectionDetailsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private MobileConnectionDetailsController _controller;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new MobileConnectionDetailsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionMobileConnectionDetailsViewModel();

            var validationModel = new MaxLengthResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Details == viewModel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _controller.UpdateMobileConnectionDetails(SolutionId, viewModel)
                    .ConfigureAwait(false)) as NoContentResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Details == viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateSolutionMobileConnectionDetailsViewModel();

            var validationModel = new MaxLengthResult
            {
                MaxLength = { "connection-requirements-description" }
            };

            _mockMediator.Setup(m =>
                m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Details == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _controller.UpdateMobileConnectionDetails(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result?.Value as UpdateSolutionMobileConnectionDetailsResult)?.MaxLength.Should().BeEquivalentTo(validationModel.MaxLength);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Details ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
