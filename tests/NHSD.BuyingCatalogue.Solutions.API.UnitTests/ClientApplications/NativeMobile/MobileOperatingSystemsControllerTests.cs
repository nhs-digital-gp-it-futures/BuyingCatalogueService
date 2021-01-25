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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class MobileOperatingSystemsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private MobileOperatingSystemsController mobileOperatingSystemsController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            mobileOperatingSystemsController = new MobileOperatingSystemsController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<UpdateSolutionMobileOperatingSystemsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetOperatingSystems()
        {
            Expression<Func<IMobileOperatingSystems, bool>> mobileOperatingSystems = m =>
                m.OperatingSystems == new HashSet<string> { "Windows", "IOS" }
                && m.OperatingSystemsDescription == "desc";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MobileOperatingSystems == Mock.Of(mobileOperatingSystems);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var operatingSystem = result.Value as GetMobileOperatingSystemsResult;

            Assert.NotNull(operatingSystem);
            operatingSystem.OperatingSystems.Should().BeEquivalentTo("Windows", "IOS");
            operatingSystem.OperatingSystemsDescription.Should().Be("desc");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetOperatingSystemIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileOperatingSystems).Returns<IMobileOperatingSystems>(null);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientMock.Object);

            var result = await mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var operatingSystem = result.Value as GetMobileOperatingSystemsResult;

            Assert.NotNull(operatingSystem);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var operatingSystem = result.Value as GetMobileOperatingSystemsResult;

            Assert.NotNull(operatingSystem);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateNativeMobileOperatingSystemsViewModel();

            var result = await mobileOperatingSystemsController.UpdateMobileOperatingSystems(
                SolutionId,
                request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileOperatingSystemsCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("operating-systems-description", "maxLength");
            resultDictionary.Add("operating-systems", "required");

            var request = new UpdateNativeMobileOperatingSystemsViewModel();

            var result = await mobileOperatingSystemsController.UpdateMobileOperatingSystems(
                SolutionId,
                request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["operating-systems"].Should().Be("required");
            validationResult["operating-systems-description"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileOperatingSystemsCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
