using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
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
    public sealed class BrowserAdditionalInformationControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private BrowserAdditionalInformationController _browserAdditionalInformationController;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _browserAdditionalInformationController = new BrowserAdditionalInformationController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateBrowserBasedAdditionalInformationCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        public async Task ShouldGetBrowserAdditionalInformation(string information)
        {
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.AdditionalInformation == information));

            var result = (await _browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browserAdditionalInformation = result.Value as GetBrowserAdditionalInformationResult;

            browserAdditionalInformation.AdditionalInformation.Should().Be(information);
            _mediatorMock.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result =
                (await _browserAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var browserAdditionalInformation = result.Value as GetBrowserAdditionalInformationResult;
            browserAdditionalInformation.AdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request =
                new UpdateBrowserBasedAdditionalInformationViewModel() { AdditionalInformation = "Some Additional Info" };
            var result =
                (await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateBrowserBasedAdditionalInformationCommand>(c =>
                        c.SolutionId == SolutionId && c.AdditionalInformation == "Some Additional Info"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("additional-information", "maxLength");
            var request =
                new UpdateBrowserBasedAdditionalInformationViewModel { AdditionalInformation = new string('a', 500) };

            var result =
                (await _browserAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["additional-information"].Should().Be("maxLength");

            _mediatorMock.Verify(x => x.Send(
                    It.Is<UpdateBrowserBasedAdditionalInformationCommand>(c =>
                        c.AdditionalInformation == new string('a', 500) &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
