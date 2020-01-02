using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeMobile
{
    [TestFixture]
    public sealed class MobileThirdPartyControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private MobileThirdPartyController _mobileThirdPartyController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mobileThirdPartyController = new MobileThirdPartyController(_mockMediator.Object);
        }

        [TestCase(null, null)]
        [TestCase("Components", null)]
        [TestCase(null, "Capabilities")]
        [TestCase("Components", "Capabilities")]
        public async Task ShouldGetMobileThirdParty(string thirdPartyComponents, string deviceCapabilities)
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                c.MobileThirdParty == Mock.Of<IMobileThirdParty>(m =>
                    m.ThirdPartyComponents == thirdPartyComponents && m.DeviceCapabilities == deviceCapabilities)));

            var result = (await _mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId)
                .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileThirdParty = result.Value as GetMobileThirdPartyResult;

            mobileThirdParty.ThirdPartyComponents.Should().Be(thirdPartyComponents);
            mobileThirdParty.DeviceCapabilities.Should().Be(deviceCapabilities);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task ShouldGetClientApplicationIsNull()
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var result =
                (await _mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId).ConfigureAwait(false)) as
                ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileThirdParty = (result.Value as GetMobileThirdPartyResult);
            mobileThirdParty.ThirdPartyComponents.Should().BeNull();
            mobileThirdParty.DeviceCapabilities.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result =
                (await _mobileThirdPartyController.GetNativeMobileThirdParty(SolutionId).ConfigureAwait(false)) as
                ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var mobileThirdParty = (result.Value as GetMobileThirdPartyResult);
            mobileThirdParty.ThirdPartyComponents.Should().BeNull();
            mobileThirdParty.DeviceCapabilities.Should().BeNull();
        }
    }
}
