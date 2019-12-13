using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserMobileFirstControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowserMobileFirstController _browserMobileFirstController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserMobileFirstController = new BrowserMobileFirstController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionBrowserMobileFirstViewModel();

            var validationResult = new UpdateSolutionBrowserMobileFirstValidationResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, viewModel).ConfigureAwait(false) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotUpdateAsValidationInValid()
        {
            var viewModel = new UpdateSolutionBrowserMobileFirstViewModel();

            var validationResult = new UpdateSolutionBrowserMobileFirstValidationResult()
            {
                Required = { "mobile-first-design" }
            };

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, viewModel).ConfigureAwait(false) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionBrowserMobileFirstResult).Required.Should()
                .BeEquivalentTo(new[] {"mobile-first-design"});

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
