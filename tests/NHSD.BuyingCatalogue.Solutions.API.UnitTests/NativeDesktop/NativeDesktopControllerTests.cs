using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeDesktopController _nativeDesktopController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeDesktopController = new NativeDesktopController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnResult()
        {
            var result = await _nativeDesktopController.GetNativeDesktopAsync(SolutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var nativeDesktopResult = result.Value as NativeDesktopResult;
            nativeDesktopResult.Should().NotBeNull();
            nativeDesktopResult.NativeDesktopSections.Should().NotBeNull();

            var operatingSystems = nativeDesktopResult.NativeDesktopSections.OperatingSystems;
            AssertSectionMandatoryAndComplete(operatingSystems, true, false);

            var connectionDetails = nativeDesktopResult.NativeDesktopSections.ConnectionDetails;
            AssertSectionMandatoryAndComplete(connectionDetails, true, false);

            var memoryAndStorage = nativeDesktopResult.NativeDesktopSections.MemoryAndStorage;
            AssertSectionMandatoryAndComplete(memoryAndStorage, true, false);

            var thirdParty = nativeDesktopResult.NativeDesktopSections.ThirdParty;
            AssertSectionMandatoryAndComplete(thirdParty, false, false);

            var hardwareRequirements = nativeDesktopResult.NativeDesktopSections.HardwareRequirements;
            AssertSectionMandatoryAndComplete(hardwareRequirements, false, false);

            var additionalInformation = nativeDesktopResult.NativeDesktopSections.AdditionalInformation;
            AssertSectionMandatoryAndComplete(additionalInformation, false, false);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _nativeDesktopController.GetNativeDesktopAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var nativeDesktopResult = result.Value as NativeDesktopResult;
            nativeDesktopResult.Should().NotBeNull();
            nativeDesktopResult.NativeDesktopSections.Should().NotBeNull();

            var operatingSystems = nativeDesktopResult.NativeDesktopSections.OperatingSystems;
            AssertSectionMandatoryAndComplete(operatingSystems, true, false);

            var connectionDetails = nativeDesktopResult.NativeDesktopSections.ConnectionDetails;
            AssertSectionMandatoryAndComplete(connectionDetails, true, false);

            var memoryAndStorage = nativeDesktopResult.NativeDesktopSections.MemoryAndStorage;
            AssertSectionMandatoryAndComplete(memoryAndStorage, true, false);

            var thirdParty = nativeDesktopResult.NativeDesktopSections.ThirdParty;
            AssertSectionMandatoryAndComplete(thirdParty, false, false);

            var hardwareRequirements = nativeDesktopResult.NativeDesktopSections.HardwareRequirements;
            AssertSectionMandatoryAndComplete(hardwareRequirements, false, false);

            var additionalInformation = nativeDesktopResult.NativeDesktopSections.AdditionalInformation;
            AssertSectionMandatoryAndComplete(additionalInformation, false, false);
        }

        [Test]
        public async Task ShouldGetNativeDesktopStaticData()
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>()).ConfigureAwait(false);

            nativeDesktopResult.Should().NotBeNull();
            nativeDesktopResult.NativeDesktopSections.Should().NotBeNull();

            var operatingSystems = nativeDesktopResult.NativeDesktopSections.OperatingSystems;
            AssertSectionMandatoryAndComplete(operatingSystems, true, false);

            var connectionDetails = nativeDesktopResult.NativeDesktopSections.ConnectionDetails;
            AssertSectionMandatoryAndComplete(connectionDetails, true, false);

            var memoryAndStorage = nativeDesktopResult.NativeDesktopSections.MemoryAndStorage;
            AssertSectionMandatoryAndComplete(memoryAndStorage, true, false);

            var thirdParty = nativeDesktopResult.NativeDesktopSections.ThirdParty;
            AssertSectionMandatoryAndComplete(thirdParty, false, false);

            var hardwareRequirements = nativeDesktopResult.NativeDesktopSections.HardwareRequirements;
            AssertSectionMandatoryAndComplete(hardwareRequirements, false, false);

            var additionalInformation = nativeDesktopResult.NativeDesktopSections.AdditionalInformation;
            AssertSectionMandatoryAndComplete(additionalInformation, false, false);
        }

        [TestCase(null, false)]
        [TestCase("Some Hardware", true)]
        public async Task ShouldGetNativeMobileHardwareRequirementIsComplete(string hardware, bool isComplete)
        {
            var nativeMobileResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopHardwareRequirements == hardware))
                .ConfigureAwait(false);

            nativeMobileResult.NativeDesktopSections.HardwareRequirements.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private async Task<NativeDesktopResult> GetNativeDesktopSectionAsync(IClientApplication clientApplication)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientApplication);

            var result =
                (await _nativeDesktopController.GetNativeDesktopAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()));

            return result.Value as NativeDesktopResult;
        }

        private static void AssertSectionMandatoryAndComplete(DashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}
