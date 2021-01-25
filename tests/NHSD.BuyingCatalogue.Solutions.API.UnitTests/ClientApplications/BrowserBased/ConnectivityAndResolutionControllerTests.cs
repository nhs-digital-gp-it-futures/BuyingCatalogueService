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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class ConnectivityAndResolutionControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private ConnectivityAndResolutionController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            controller = new ConnectivityAndResolutionController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionConnectivityAndResolutionCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task UpdateConnectivityResult()
        {
            var request = new UpdateBrowserBasedConnectivityAndResolutionViewModel();

            var result = await controller.UpdateConnectivityAndResolutionAsync(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionConnectivityAndResolutionCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task InvalidRequiredResultForCommandReturnsValidationDetails()
        {
            resultDictionary.Add("minimum-connection-speed", "required");
            var request = new UpdateBrowserBasedConnectivityAndResolutionViewModel();

            var result = await controller.UpdateConnectivityAndResolutionAsync(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(1);
            validationResult["minimum-connection-speed"].Should().Be("required");

            Expression<Func<UpdateSolutionConnectivityAndResolutionCommand, bool>> match = c =>
                c.Data.MinimumConnectionSpeed == null
                && c.Data.MinimumDesktopResolution == null
                && c.Id == SolutionId;

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [TestCase(null, null)]
        [TestCase("Connection Speed", null)]
        [TestCase(null, "Resolution")]
        [TestCase("Connection Speed", "Resolution")]
        public async Task ShouldGetConnectivityAndResolution(string connection, string resolution)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MinimumConnectionSpeed == connection
                && c.MinimumDesktopResolution == resolution;

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await controller.GetConnectivityAndResolution(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var connectivityResult = result.Value as GetConnectivityAndResolutionResult;

            Assert.NotNull(connectivityResult);
            connectivityResult.MinimumConnectionSpeed.Should().Be(connection);
            connectivityResult.MinimumDesktopResolution.Should().Be(resolution);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GetInvalidSolutionReturnsEmpty()
        {
            var result = await controller.GetConnectivityAndResolution("unknownId") as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<GetConnectivityAndResolutionResult>();
            result.Value.As<GetConnectivityAndResolutionResult>().MinimumConnectionSpeed.Should().BeNull();
            result.Value.As<GetConnectivityAndResolutionResult>().MinimumDesktopResolution.Should().BeNull();
        }
    }
}
