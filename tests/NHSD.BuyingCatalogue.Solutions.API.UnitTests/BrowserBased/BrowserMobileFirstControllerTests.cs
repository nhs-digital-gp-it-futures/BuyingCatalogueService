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
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserMobileFirstController = new BrowserMobileFirstController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionBrowserMobileFirstCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
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
            var request = new UpdateBrowserBasedMobileFirstViewModel { MobileFirstDesign = "Yes" };
            var result = (await _browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, request).ConfigureAwait(false))
                as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(c =>
                        c.MobileFirstDesign == "Yes" &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldNotUpdateAsValidationInValid()
        {
            _resultDictionary.Add("mobile-first-design", "required");
            var request = new UpdateBrowserBasedMobileFirstViewModel { MobileFirstDesign = null };
            var result =
                (await _browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["mobile-first-design"].Should().Be("required");

            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionBrowserMobileFirstCommand>(c =>
                        c.MobileFirstDesign == null &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
