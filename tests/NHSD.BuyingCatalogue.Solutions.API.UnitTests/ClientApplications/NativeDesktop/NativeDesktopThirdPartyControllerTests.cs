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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class NativeDesktopThirdPartyControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private NativeDesktopThirdPartyController nativeDesktopThirdPartyController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            nativeDesktopThirdPartyController = new NativeDesktopThirdPartyController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionNativeDesktopThirdPartyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("Component", "Cabability")]
        [TestCase("Component", "")]
        [TestCase("", "Cabability")]
        [TestCase("       ", "")]
        [TestCase("", "         ")]
        [TestCase(null, null)]
        public async Task PopulatedThirdPartyShouldReturnCorrectThirdParty(string components, string capabilities)
        {
            Expression<Func<INativeDesktopThirdParty, bool>> nativeDesktopThirdParty = t =>
                t.ThirdPartyComponents == components
                && t.DeviceCapabilities == capabilities;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.NativeDesktopThirdParty == Mock.Of(nativeDesktopThirdParty);

            Expression<Func<GetClientApplicationBySolutionIdQuery, bool>> match = q => q.Id == SolutionId;

            mediatorMock
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await nativeDesktopThirdPartyController.GetThirdParty(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopThirdPartyResult>();

            var thirdPartyResult = result.Value as GetNativeDesktopThirdPartyResult;

            Assert.NotNull(thirdPartyResult);
            thirdPartyResult.ThirdPartyComponents.Should().Be(components);
            thirdPartyResult.DeviceCapabilities.Should().Be(capabilities);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = await nativeDesktopThirdPartyController.GetThirdParty(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopThirdPartyResult>();

            var thirdPartyResult = result.Value as GetNativeDesktopThirdPartyResult;

            Assert.NotNull(thirdPartyResult);
            thirdPartyResult.ThirdPartyComponents.Should().BeNull();
            thirdPartyResult.DeviceCapabilities.Should().BeNull();
        }

        [Test]
        public async Task UpdateValidThirdPartyDetails()
        {
            var request = new UpdateNativeDesktopThirdPartyViewModel
            {
                ThirdPartyComponents = "New Component",
                DeviceCapabilities = "New Capability",
            };

            var result = await nativeDesktopThirdPartyController.UpdatedThirdParty(SolutionId, request) as NoContentResult;

            Expression<Func<UpdateSolutionNativeDesktopThirdPartyCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data.ThirdPartyComponents == "New Component"
                && c.Data.DeviceCapabilities == "New Capability";

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("third-party-components", "maxLength");
            resultDictionary.Add("device-capabilities", "maxLength");
            var request = new UpdateNativeDesktopThirdPartyViewModel();

            var result = await nativeDesktopThirdPartyController.UpdatedThirdParty(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["third-party-components"].Should().Be("maxLength");
            validationResult["device-capabilities"].Should().Be("maxLength");

            Expression<Func<UpdateSolutionNativeDesktopThirdPartyCommand, bool>> match = c =>
                c.Data.ThirdPartyComponents == request.ThirdPartyComponents
                && c.Data.DeviceCapabilities == request.DeviceCapabilities
                && c.SolutionId == SolutionId;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
