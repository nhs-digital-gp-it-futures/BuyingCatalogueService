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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class MobileThirdPartyControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private MobileThirdPartyController mobileThirdPartyController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            mobileThirdPartyController = new MobileThirdPartyController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateSolutionMobileThirdPartyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase(null, null)]
        [TestCase("Components", null)]
        [TestCase(null, "Capabilities")]
        [TestCase("Components", "Capabilities")]
        public async Task ShouldGetMobileThirdParty(string thirdPartyComponents, string deviceCapabilities)
        {
            Expression<Func<IMobileThirdParty, bool>> mobileThirdPartyInstance = m =>
                m.ThirdPartyComponents == thirdPartyComponents
                && m.DeviceCapabilities == deviceCapabilities;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MobileThirdParty == Mock.Of(mobileThirdPartyInstance);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileThirdParty = result.Value as GetMobileThirdPartyResult;

            Assert.NotNull(mobileThirdParty);
            mobileThirdParty.ThirdPartyComponents.Should().Be(thirdPartyComponents);
            mobileThirdParty.DeviceCapabilities.Should().Be(deviceCapabilities);

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

            var result = await mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileThirdParty = result.Value as GetMobileThirdPartyResult;

            Assert.NotNull(mobileThirdParty);
            mobileThirdParty.ThirdPartyComponents.Should().BeNull();
            mobileThirdParty.DeviceCapabilities.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var mobileThirdParty = result.Value as GetMobileThirdPartyResult;

            Assert.NotNull(mobileThirdParty);
            mobileThirdParty.ThirdPartyComponents.Should().BeNull();
            mobileThirdParty.DeviceCapabilities.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateNativeMobileThirdPartyViewModel();

            var result = await mobileThirdPartyController.UpdateNativeMobileThirdParty(
                SolutionId,
                request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileThirdPartyCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("third-party-components", "maxLength");
            resultDictionary.Add("device-capabilities", "maxLength");

            var request = new UpdateNativeMobileThirdPartyViewModel();

            var result = await mobileThirdPartyController.UpdateNativeMobileThirdParty(
                SolutionId,
                request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(2);
            resultValue["third-party-components"].Should().Be("maxLength");
            resultValue["device-capabilities"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionMobileThirdPartyCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
