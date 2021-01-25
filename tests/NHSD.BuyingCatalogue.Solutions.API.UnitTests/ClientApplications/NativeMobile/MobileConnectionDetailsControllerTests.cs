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
    internal sealed class MobileConnectionDetailsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private MobileConnectionDetailsController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            controller = new MobileConnectionDetailsController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionMobileConnectionDetailsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetOperatingSystems()
        {
            Expression<Func<IMobileConnectionDetails, bool>> mobileConnectionDetails = m =>
                m.ConnectionType == new HashSet<string> { "4G", "3G" }
                && m.Description == "desc"
                && m.MinimumConnectionSpeed == "1GBps";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MobileConnectionDetails == Mock.Of(mobileConnectionDetails);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await controller.GetMobileConnectionDetails(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var connectionDetails = result.Value as GetMobileConnectionDetailsResult;

            Assert.NotNull(connectionDetails);
            connectionDetails.ConnectionType.Should().BeEquivalentTo("4G", "3G");
            connectionDetails.ConnectionRequirementsDescription.Should().Be("desc");
            connectionDetails.MinimumConnectionSpeed.Should().Be("1GBps");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetConnectionDetailsAreNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileConnectionDetails).Returns<IMobileConnectionDetails>(null);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientMock.Object);

            var result = await controller.GetMobileConnectionDetails(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var connectionDetails = result.Value as GetMobileConnectionDetailsResult;

            Assert.NotNull(connectionDetails);
            connectionDetails.ConnectionType.Count().Should().Be(0);
            connectionDetails.ConnectionRequirementsDescription.Should().BeNull();
            connectionDetails.MinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await controller.GetMobileConnectionDetails(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var connectionDetails = result.Value as GetMobileConnectionDetailsResult;

            Assert.NotNull(connectionDetails);
            connectionDetails.ConnectionType.Count().Should().Be(0);
            connectionDetails.ConnectionRequirementsDescription.Should().BeNull();
            connectionDetails.MinimumConnectionSpeed.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateNativeMobileConnectionDetailsViewModel();

            var result = await controller.UpdateMobileConnectionDetails(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileConnectionDetailsCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("connection-requirements-description", "maxLength");
            var request = new UpdateNativeMobileConnectionDetailsViewModel();

            var result = await controller.UpdateMobileConnectionDetails(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["connection-requirements-description"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileConnectionDetailsCommand>(q => q.SolutionId == SolutionId && q.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
