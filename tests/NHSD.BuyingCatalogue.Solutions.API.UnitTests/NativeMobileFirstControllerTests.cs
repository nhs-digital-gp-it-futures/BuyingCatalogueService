using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
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

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task ShouldGetMobileFirst(bool? mobileFirstDesign, string response)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                        c.NativeMobileFirstDesign == mobileFirstDesign));

            var result = (await _nativeMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileFirst = (result?.Value as GetNativeMobileFirstResult);

            mobileFirst?.MobileFirstDesign.Should().Be(response);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetClientApplicationIsNull()
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var result = (await _nativeMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileFirst = (result?.Value as GetNativeMobileFirstResult);
            mobileFirst?.MobileFirstDesign.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _nativeMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as NotFoundResult;
            result?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
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
            (result?.Value as UpdateFormRequiredResult)?.Required.Should()
                .BeEquivalentTo(new[] { "mobile-first-design" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionNativeMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
