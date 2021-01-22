using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class BrowserSupportedControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private BrowsersSupportedController browserSupportedController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            browserSupportedController = new BrowsersSupportedController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(x => x.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionBrowsersSupportedCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetBrowsersSupported()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" }
                && c.MobileResponsive == true;

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await browserSupportedController.GetBrowsersSupportedAsync(SolutionId) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browsersSupported = result.Value as GetBrowsersSupportedResult;

            browsersSupported.BrowsersSupported.Should().BeEquivalentTo("Chrome", "Edge");
            browsersSupported.MobileResponsive.Should().Be("Yes");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetEmptyBrowsersSupported()
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.MobileResponsive == true));

            var result = await browserSupportedController.GetBrowsersSupportedAsync(SolutionId) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browsersSupported = result.Value as GetBrowsersSupportedResult;

            browsersSupported.BrowsersSupported.Should().BeEmpty();
            browsersSupported.MobileResponsive.Should().Be("Yes");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [TestCase(null, null)]
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        public async Task ShouldGetMobileResponsive(bool? mobileResponsive, string expectedMobileResponsive)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.MobileResponsive == mobileResponsive));

            var result = await browserSupportedController.GetBrowsersSupportedAsync(SolutionId) as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browsersSupported = result.Value as GetBrowsersSupportedResult;
            browsersSupported.MobileResponsive.Should().Be(expectedMobileResponsive);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetClientApplicationIsNull()
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var result = await browserSupportedController.GetBrowsersSupportedAsync(SolutionId) as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browsersSupported = result.Value as GetBrowsersSupportedResult;
            browsersSupported.MobileResponsive.Should().BeNull();
            browsersSupported.BrowsersSupported.Count().Should().Be(0);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await browserSupportedController.GetBrowsersSupportedAsync(SolutionId) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            (result.Value as GetBrowsersSupportedResult).MobileResponsive.Should().BeNull();
            (result.Value as GetBrowsersSupportedResult).BrowsersSupported.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedBrowsersSupportedViewModel
            {
                BrowsersSupported = new HashSet<string> { "Edge" },
                MobileResponsive = "yes",
            };

            var result = await browserSupportedController.UpdateBrowsersSupportedAsync(
                SolutionId,
                request) as NoContentResult;

            Expression<Func<UpdateSolutionBrowsersSupportedCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data.BrowsersSupported.All(s => s == "Edge")
                && c.Data.MobileResponsive == "yes";

            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("supported-browsers", "required");
            resultDictionary.Add("mobile-responsive", "required");
            var request = new UpdateBrowserBasedBrowsersSupportedViewModel();

            var result = await browserSupportedController.UpdateBrowsersSupportedAsync(
                SolutionId,
                request) as BadRequestObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["supported-browsers"].Should().Be("required");
            validationResult["mobile-responsive"].Should().Be("required");

            Expression<Func<UpdateSolutionBrowsersSupportedCommand, bool>> match = c =>
                !c.Data.BrowsersSupported.Any()
                && c.Data.MobileResponsive == null
                && c.SolutionId == SolutionId;

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
