using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class MobileOperatingSystemsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private MobileOperatingSystemsController _mobileOperatingSystemsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mobileOperatingSystemsController = new MobileOperatingSystemsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionMobileOperatingSystemsViewModel();

            var validationModel = new RequiredMaxLengthResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, viewModel)
                    .ConfigureAwait(false)) as NoContentResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateSolutionMobileOperatingSystemsViewModel();

            var validationModel = new RequiredMaxLengthResult()
            {
                Required = { "operating-systems" },
                MaxLength = { "operating-systems-description" }
            };

            _mockMediator.Setup(m =>
                m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result?.Value as UpdateSolutionMobileOperatingSystemsResult)?.Required.Should().BeEquivalentTo(new[] { "operating-systems" });
            (result?.Value as UpdateSolutionMobileOperatingSystemsResult)?.MaxLength.Should().BeEquivalentTo(new[] { "operating-systems-description" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
