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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class BrowserAdditionalInformationControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private BrowserAdditionalInformationController browserAdditionalInformationController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            browserAdditionalInformationController = new BrowserAdditionalInformationController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateBrowserBasedAdditionalInformationCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        public async Task ShouldGetBrowserAdditionalInformation(string information)
        {
            mediatorMock
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.AdditionalInformation == information));

            var result = await browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browserAdditionalInformation = result.Value as GetBrowserAdditionalInformationResult;

            Assert.NotNull(browserAdditionalInformation);
            browserAdditionalInformation.AdditionalInformation.Should().Be(information);

            mediatorMock.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browserAdditionalInformation = result.Value as GetBrowserAdditionalInformationResult;

            Assert.NotNull(browserAdditionalInformation);
            browserAdditionalInformation.AdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedAdditionalInformationViewModel { AdditionalInformation = "Some Additional Info" };
            var result = await browserAdditionalInformationController.UpdateAdditionalInformationAsync(
                SolutionId,
                request) as NoContentResult;

            Expression<Func<UpdateBrowserBasedAdditionalInformationCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.AdditionalInformation == "Some Additional Info";

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("additional-information", "maxLength");
            var request = new UpdateBrowserBasedAdditionalInformationViewModel { AdditionalInformation = new string('a', 500) };

            var result = await browserAdditionalInformationController.UpdateAdditionalInformationAsync(
                SolutionId,
                request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(1);
            validationResult["additional-information"].Should().Be("maxLength");

            Expression<Func<UpdateBrowserBasedAdditionalInformationCommand, bool>> match = c =>
                c.AdditionalInformation == new string('a', 500)
                && c.SolutionId == SolutionId;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
