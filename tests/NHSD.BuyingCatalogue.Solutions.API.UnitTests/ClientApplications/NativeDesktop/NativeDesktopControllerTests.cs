using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
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
        public async Task ShouldGetNativeDesktopHardwareRequirementIsComplete(string hardware, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopHardwareRequirements == hardware))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.HardwareRequirements.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("                  ", false)]
        [TestCase("Some OS Summary", true)]
        public async Task ShouldGetNativeDesktopOperatingSystemsIsComplete(string description, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopOperatingSystemsDescription == description))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.OperatingSystems.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }


        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("3 Mbps", true)]
        public async Task ShouldGetNativeDesktopConnectivityDetailsIsComplete(string minimumConnectionSpeed, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopMinimumConnectionSpeed == minimumConnectionSpeed))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.ConnectionDetails.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, null, false)]
        [TestCase("", "  ", false)]
        [TestCase(" ", "", false)]
        [TestCase("Connectivity", null, true)]
        [TestCase(null, "Capability", true)]
        [TestCase("Connectivity", "Capability", true)]
        public async Task ShouldGetNativeDesktopThirdPartyIsComplete(string component, string capability, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c =>
                    c.NativeDesktopThirdParty == Mock.Of<INativeDesktopThirdParty>(t =>
                        t.ThirdPartyComponents == component && t.DeviceCapabilities == capability)))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.ThirdParty.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase("a", "b", "c", "d", true)]
        [TestCase("a", "b", "c", null, true)]
        [TestCase("a", "b", null, "d", false)]
        [TestCase("a", null, "c", "d", false)]
        [TestCase(null, "b", "c", "d", false)]
        [TestCase("a", null, null, "d", false)]
        [TestCase(null, null, "c", "d", false)]
        [TestCase(null, "b", null, "d", false)]
        [TestCase(null, null, null, "d", false)]
        public async Task ShouldGetNativeDesktopMemoryAndStorageIsComplete(string memory, string storage, string cpu, string resolution, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c =>
                    c.NativeDesktopMemoryAndStorage == Mock.Of<INativeDesktopMemoryAndStorage>(t =>
                        t.MinimumMemoryRequirement == memory &&
                        t.StorageRequirementsDescription == storage &&
                        t.MinimumCpu == cpu &&
                        t.RecommendedResolution == resolution)))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.MemoryAndStorage.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("some info", true)]
        public async Task ShouldGetNativeDesktopAdditionalInformationIsComplete(string information, bool isComplete)
        {
            var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>(c =>
                    c.NativeDesktopAdditionalInformation == information))
                .ConfigureAwait(false);

            nativeDesktopResult.NativeDesktopSections.AdditionalInformation.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
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
