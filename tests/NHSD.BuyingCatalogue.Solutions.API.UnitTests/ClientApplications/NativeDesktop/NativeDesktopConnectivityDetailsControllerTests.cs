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
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class NativeDesktopConnectivityDetailsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private NativeDesktopConnectivityDetailsController nativeDesktopConnectivityDetailsController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            nativeDesktopConnectivityDetailsController = new NativeDesktopConnectivityDetailsController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionNativeDesktopConnectivityDetailsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = await nativeDesktopConnectivityDetailsController.GetConnectivity(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopConnectivityDetailsResult>();
            result.Value.As<GetNativeDesktopConnectivityDetailsResult>().NativeDesktopMinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task UpdateValidConnectivityDetails()
        {
            var request = new UpdateNativeDesktopConnectivityDetailsViewModel
            {
                NativeDesktopMinimumConnectionSpeed = "6Mbps",
            };

            var result = await nativeDesktopConnectivityDetailsController.UpdatedConnectivity(
                SolutionId,
                request) as NoContentResult;

            Expression<Func<UpdateSolutionNativeDesktopConnectivityDetailsCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.NativeDesktopMinimumConnectionSpeed == "6Mbps";

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("minimum-connection-speed", "required");
            var request = new UpdateNativeDesktopConnectivityDetailsViewModel { NativeDesktopMinimumConnectionSpeed = null };

            var result = await nativeDesktopConnectivityDetailsController.UpdatedConnectivity(
                SolutionId,
                request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(1);
            validationResult["minimum-connection-speed"].Should().Be("required");

            Expression<Func<UpdateSolutionNativeDesktopConnectivityDetailsCommand, bool>> match = c =>
                c.NativeDesktopMinimumConnectionSpeed == null
                && c.SolutionId == SolutionId;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
