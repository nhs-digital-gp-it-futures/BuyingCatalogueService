using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    public sealed class MobileConnectionDetailsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private MobileConnectionDetailsController _controller;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new MobileConnectionDetailsController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionMobileConnectionDetailsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }
        [Test]
        public async Task ShouldGetOperatingSystems()
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileConnectionDetails == Mock.Of<IMobileConnectionDetails>(m =>
                        m.ConnectionType == new HashSet<string>() { "4G", "3G" } &&
                        m.Description == "desc" &&
                        m.MinimumConnectionSpeed == "1GBps")));

            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var connectionDetails = result.Value as GetMobileConnectionDetailsResult;
            connectionDetails.ConnectionType.Should().BeEquivalentTo(new[] { "4G", "3G" });
            connectionDetails.ConnectionRequirementsDescription.Should().Be("desc");
            connectionDetails.MinimumConnectionSpeed.Should().Be("1GBps");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetConnectionDetailsAreNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileConnectionDetails).Returns<IMobileConnectionDetails>(null);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientMock.Object);

            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var connectionDetails = (result.Value as GetMobileConnectionDetailsResult);
            connectionDetails.ConnectionType.Count().Should().Be(0);
            connectionDetails.ConnectionRequirementsDescription.Should().BeNull();
            connectionDetails.MinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result =
                (await _controller.GetMobileConnectionDetails(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var connectionDetails = (result.Value as GetMobileConnectionDetailsResult);
            connectionDetails.ConnectionType.Count().Should().Be(0);
            connectionDetails.ConnectionRequirementsDescription.Should().BeNull();
            connectionDetails.MinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateNativeMobileConnectionDetailsViewModel();

            var result =
                (await _controller.UpdateMobileConnectionDetails(SolutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Data == request), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("connection-requirements-description", "maxLength");
            var request = new UpdateNativeMobileConnectionDetailsViewModel();


            var result =
                (await _controller.UpdateMobileConnectionDetails(SolutionId, request).ConfigureAwait(false)) as
                BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["connection-requirements-description"].Should().Be("maxLength");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q =>
                        q.SolutionId == SolutionId && q.Data ==
                        request), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
