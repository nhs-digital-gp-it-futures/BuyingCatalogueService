using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class NativeMobileControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeMobileController _nativeMobileController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeMobileController = new NativeMobileController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _nativeMobileController.GetNativeMobileAsync(SolutionId).ConfigureAwait(false)) as NotFoundResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldGetNativeMobileStaticData()
        {
            var nativeMobileResult =
                await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>()).ConfigureAwait(false);

            nativeMobileResult.Should().NotBeNull();
            nativeMobileResult.NativeMobileSections.Should().NotBeNull();

            var mobileOperatingSystems = nativeMobileResult.NativeMobileSections.MobileOperatingSystems;
            AssertSectionMandatoryAndComplete(mobileOperatingSystems, true, false);

            var mobileFirst = nativeMobileResult.NativeMobileSections.MobileFirst;
            AssertSectionMandatoryAndComplete(mobileFirst, true, false);

            var mobileMemoryStorage = nativeMobileResult.NativeMobileSections.MobileMemoryStorage;
            AssertSectionMandatoryAndComplete(mobileMemoryStorage, true, false);

            var mobileConnectionDetails = nativeMobileResult.NativeMobileSections.MobileConnectionDetails;
            AssertSectionMandatoryAndComplete(mobileConnectionDetails,false, false);

            var mobileComponentDeviceCapabilities =
                nativeMobileResult.NativeMobileSections.MobileComponentsDeviceCapabilities;
            AssertSectionMandatoryAndComplete(mobileComponentDeviceCapabilities, false, false);

            var mobileHardwareRequirements = nativeMobileResult.NativeMobileSections.MobileHardwareRequirements;
            AssertSectionMandatoryAndComplete(mobileHardwareRequirements, false, false);

            var mobileAdditionalInformation = nativeMobileResult.NativeMobileSections.MobileAdditionalInformation;
            AssertSectionMandatoryAndComplete(mobileAdditionalInformation, false, false);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetMobileOperatingSystemsIsRequired(bool isOperatingSystemsEmpty)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c =>
                c.MobileOperatingSystems ==
                Mock.Of<IMobileOperatingSystems>(m => m.OperatingSystems == (isOperatingSystemsEmpty ? new HashSet<string>() {"IOS"} : new HashSet<string>())))).ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileOperatingSystems.Status.Should()
                .Be(isOperatingSystemsEmpty ? "COMPLETE" : "INCOMPLETE");
        }

        private async Task<NativeMobileResult> GetNativeMobileSectionAsync(IClientApplication clientApplication)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientApplication);

            var result =
                (await _nativeMobileController.GetNativeMobileAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);

            return result.Value as NativeMobileResult;
        }

        private static void AssertSectionMandatoryAndComplete(DashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}
