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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class BrowserMobileFirstControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private BrowserMobileFirstController browserMobileFirstController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            browserMobileFirstController = new BrowserMobileFirstController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionBrowserMobileFirstCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task ShouldGetMobileFirst(bool? mobileFirstDesign, string response)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.MobileFirstDesign == mobileFirstDesign));

            var result = await browserMobileFirstController.GetMobileFirstAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileFirst = result.Value as GetBrowserMobileFirstResult;

            Assert.NotNull(mobileFirst);
            mobileFirst.MobileFirstDesign.Should().Be(response);

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

            var result = await browserMobileFirstController.GetMobileFirstAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileFirst = result.Value as GetBrowserMobileFirstResult;

            Assert.NotNull(mobileFirst);
            mobileFirst.MobileFirstDesign.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await browserMobileFirstController.GetMobileFirstAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileFirst = result.Value as GetBrowserMobileFirstResult;

            Assert.NotNull(mobileFirst);
            mobileFirst.MobileFirstDesign.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedMobileFirstViewModel { MobileFirstDesign = "Yes" };
            var result = await browserMobileFirstController.UpdateMobileFirstAsync(SolutionId, request) as NoContentResult;

            Expression<Func<UpdateSolutionBrowserMobileFirstCommand, bool>> match = c =>
                c.MobileFirstDesign == "Yes"
                && c.SolutionId == SolutionId;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldNotUpdateAsValidationInValid()
        {
            resultDictionary.Add("mobile-first-design", "required");
            var request = new UpdateBrowserBasedMobileFirstViewModel { MobileFirstDesign = null };
            var result = await browserMobileFirstController.UpdateMobileFirstAsync(
                SolutionId,
                request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(1);
            validationResult["mobile-first-design"].Should().Be("required");

            Expression<Func<UpdateSolutionBrowserMobileFirstCommand, bool>> match = c =>
                c.MobileFirstDesign == null
                && c.SolutionId == SolutionId;

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
