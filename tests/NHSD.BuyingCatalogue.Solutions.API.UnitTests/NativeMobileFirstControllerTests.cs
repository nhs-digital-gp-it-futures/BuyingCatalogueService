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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class NativeMobileFirstControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeMobileFirstController _nativeMobileFirstController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeMobileFirstController = new NativeMobileFirstController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionNativeMobileFirstViewModel();

            var validationResult = new RequiredResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionNativeMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _nativeMobileFirstController.UpdateMobileFirstAsync(SolutionId, viewModel).ConfigureAwait(false) as NoContentResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionNativeMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotUpdateAsValidationInValid()
        {
            var viewModel = new UpdateSolutionNativeMobileFirstViewModel();

            var validationResult = new RequiredResult()
            {
                Required = { "mobile-first-design" }
            };

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionNativeMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var result = await _nativeMobileFirstController.UpdateMobileFirstAsync(SolutionId, viewModel).ConfigureAwait(false) as BadRequestObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result?.Value as UpdateSolutionBrowserMobileFirstResult)?.Required.Should()
                .BeEquivalentTo(new[] { "mobile-first-design" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionNativeMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
