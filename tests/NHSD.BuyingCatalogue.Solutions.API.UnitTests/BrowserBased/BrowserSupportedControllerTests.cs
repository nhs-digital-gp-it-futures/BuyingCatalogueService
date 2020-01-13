using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.BrowserBased
{
    [TestFixture]
    public sealed class BrowserSupportedControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private BrowsersSupportedController _browserSupportedController;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserSupportedController = new BrowsersSupportedController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionBrowsersSupportedCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetBrowsersSupported()
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                            c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                            c.MobileResponsive == true));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEquivalentTo(new string[] { "Chrome", "Edge" });
            browsersSupported.MobileResponsive.Should().Be("Yes");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyBrowsersSupported()
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileResponsive == true));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEmpty();
            browsersSupported.MobileResponsive.Should().Be("Yes");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        public async Task ShouldGetMobileResponsive(bool? mobileResponsive, string expectedMobileResponsive)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileResponsive == mobileResponsive));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);
            browsersSupported.MobileResponsive.Should().Be(expectedMobileResponsive);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetClientApplicationIsNull()
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);
            browsersSupported.MobileResponsive.Should().BeNull();
            browsersSupported.BrowsersSupported.Count().Should().Be(0);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetBrowsersSupportedResult).MobileResponsive.Should().BeNull();
            (result.Value as GetBrowsersSupportedResult).BrowsersSupported.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedBrowsersSupportedViewModel()
            {
                BrowsersSupported = new HashSet<string>() { "Edge"},
                MobileResponsive = "yes"
            };

            var result =
                (await _browserSupportedController.UpdateBrowsersSupportedAsync(SolutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                x => x.Send(
                    It.Is<UpdateSolutionBrowsersSupportedCommand>(c =>
                        c.SolutionId == SolutionId &&
                        !c.Data.BrowsersSupported.Any(x => x != "Edge") &&
                        c.Data.MobileResponsive == "yes"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("supported-browsers", "required");
            _resultDictionary.Add("mobile-responsive", "required");
            var request = new UpdateBrowserBasedBrowsersSupportedViewModel();

            var result =
                (await _browserSupportedController.UpdateBrowsersSupportedAsync(SolutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["supported-browsers"].Should().Be("required");
            validationResult["mobile-responsive"].Should().Be("required");

            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionBrowsersSupportedCommand>(c =>
                        !c.Data.BrowsersSupported.Any() &&
                        c.Data.MobileResponsive == null &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
