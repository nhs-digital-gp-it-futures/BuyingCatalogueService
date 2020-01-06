using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.BrowserBased
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

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task ShouldGetMobileFirst(bool? mobileFirstDesign, string response)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                        c.MobileFirstDesign == mobileFirstDesign));

            var result = (await _browserMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileFirst = (result.Value as GetBrowserMobileFirstResult);

            mobileFirst.MobileFirstDesign.Should().Be(response);

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

            var result = (await _browserMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileFirst = (result.Value as GetBrowserMobileFirstResult);
            mobileFirst.MobileFirstDesign.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _browserMobileFirstController.GetMobileFirstAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileFirst = (result.Value as GetBrowserMobileFirstResult);
            mobileFirst.MobileFirstDesign.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionBrowserMobileFirstViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

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

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "mobile-first-design", "required" } });
            validationResult.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

            var result = await _browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, viewModel).ConfigureAwait(false) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["mobile-first-design"].Should().Be("required");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowserMobileFirstViewModel == viewModel),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
