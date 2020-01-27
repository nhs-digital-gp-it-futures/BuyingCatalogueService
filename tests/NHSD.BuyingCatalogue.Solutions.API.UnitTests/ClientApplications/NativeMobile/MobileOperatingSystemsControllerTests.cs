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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    public sealed class MobileOperatingSystemsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private MobileOperatingSystemsController _mobileOperatingSystemsController;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mobileOperatingSystemsController = new MobileOperatingSystemsController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionMobileOperatingSystemsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetOperatingSystems()
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileOperatingSystems == Mock.Of<IMobileOperatingSystems>(m =>
                        m.OperatingSystems == new HashSet<string>() {"Windows", "IOS"} &&
                        m.OperatingSystemsDescription == "desc")));

            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = result.Value as GetMobileOperatingSystemsResult;
            operatingSystem.OperatingSystems.Should().BeEquivalentTo(new[] {"Windows", "IOS"});
            operatingSystem.OperatingSystemsDescription.Should().Be("desc");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetOperatingSystemIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileOperatingSystems).Returns<IMobileOperatingSystems>(null);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientMock.Object);

            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = (result.Value as GetMobileOperatingSystemsResult);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = (result.Value as GetMobileOperatingSystemsResult);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateNativeMobileOperatingSystemsViewModel();

            var result =
                (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.Data == request), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("operating-systems-description", "maxLength");
            _resultDictionary.Add("operating-systems", "required");

            var request = new UpdateNativeMobileOperatingSystemsViewModel();

            var result = (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, request).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["operating-systems"].Should().Be("required");
            validationResult["operating-systems-description"].Should().Be("maxLength");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.Data ==
                        request), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
