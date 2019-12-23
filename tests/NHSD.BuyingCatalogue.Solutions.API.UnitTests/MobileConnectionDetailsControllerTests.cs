using System.Collections.Generic;
using System.Linq;
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
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
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
        public async Task ShouldGetOperatingSystems()
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileConnectionDetails == Mock.Of<IMobileConnectionDetails>(m =>
                        m.ConnectionType == new HashSet<string>() { "4G", "3G" } &&
                        m.Description == "desc" &&
                        m.MinimumConnectionSpeed == "1GBps")));

            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var connectionDetails = result?.Value as GetMobileConnectionDetailsResult;
            connectionDetails?.ConnectionType.Should().BeEquivalentTo(new[] { "4G", "3G" });
            connectionDetails?.ConnectionRequirementsDescription.Should().Be("desc");
            connectionDetails?.MinimumConnectionSpeed.Should().Be("1GBps");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetConnectionDetailsAreNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileConnectionDetails).Returns<IMobileConnectionDetails>(null);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientMock.Object);

            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var connectionDetails = (result?.Value as GetMobileConnectionDetailsResult);
            connectionDetails?.ConnectionType.Count().Should().Be(0);
            connectionDetails?.ConnectionRequirementsDescription.Should().BeNull();
            connectionDetails?.MinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as NotFoundResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
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

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateFormMaxLengthResult).MaxLength.Should().BeEquivalentTo(validationModel.MaxLength);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Details ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
