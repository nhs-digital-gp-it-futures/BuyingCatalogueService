using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeDesktopController _nativeDesktopController;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeDesktopController = new NativeDesktopController(_mockMediator.Object);
        }

        [Test]
        public void ShouldReturnResult()
        {
            var result = _nativeDesktopController.GetNativeDesktopAsync() as ObjectResult;
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

        //[Test]
        //public async Task ShouldReturnEmpty()
        //{
        //    var result = (await _nativeDesktopController.GetNativeDesktopAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

        //    result.StatusCode.Should().Be((int)HttpStatusCode.OK);

        //    var nativeDesktopResult = result.Value as NativeDesktopResult;
        //    nativeDesktopResult.Should().NotBeNull();
        //    nativeDesktopResult.NativeDesktopSections.Should().NotBeNull();

        //    var operatingSystems = nativeDesktopResult.NativeDesktopSections.OperatingSystems;
        //    AssertSectionMandatoryAndComplete(operatingSystems, true, false);

        //    var connectionDetails = nativeDesktopResult.NativeDesktopSections.ConnectionDetails;
        //    AssertSectionMandatoryAndComplete(connectionDetails, true, false);

        //    var memoryAndStorage = nativeDesktopResult.NativeDesktopSections.MemoryAndStorage;
        //    AssertSectionMandatoryAndComplete(memoryAndStorage, true, false);

        //    var thirdParty = nativeDesktopResult.NativeDesktopSections.ThirdParty;
        //    AssertSectionMandatoryAndComplete(thirdParty, false, false);

        //    var hardwareRequirements = nativeDesktopResult.NativeDesktopSections.HardwareRequirements;
        //    AssertSectionMandatoryAndComplete(hardwareRequirements, false, false);

        //    var additionalInformation = nativeDesktopResult.NativeDesktopSections.AdditionalInformation;
        //    AssertSectionMandatoryAndComplete(additionalInformation, false, false);
        //}

        //[Test]
        //public async Task ShouldGetNativeDesktopStaticData()
        //{
        //    var nativeDesktopResult = await GetNativeDesktopSectionAsync(Mock.Of<IClientApplication>()).ConfigureAwait(false);

        //    nativeDesktopResult.Should().NotBeNull();
        //    nativeDesktopResult.NativeDesktopSections.Should().NotBeNull();

        //    var operatingSystems = nativeDesktopResult.NativeDesktopSections.OperatingSystems;
        //    AssertSectionMandatoryAndComplete(operatingSystems, true, false);

        //    var connectionDetails = nativeDesktopResult.NativeDesktopSections.ConnectionDetails;
        //    AssertSectionMandatoryAndComplete(connectionDetails, true, false);

        //    var memoryAndStorage = nativeDesktopResult.NativeDesktopSections.MemoryAndStorage;
        //    AssertSectionMandatoryAndComplete(memoryAndStorage, true, false);

        //    var thirdParty = nativeDesktopResult.NativeDesktopSections.ThirdParty;
        //    AssertSectionMandatoryAndComplete(thirdParty, false, false);

        //    var hardwareRequirements = nativeDesktopResult.NativeDesktopSections.HardwareRequirements;
        //    AssertSectionMandatoryAndComplete(hardwareRequirements, false, false);

        //    var additionalInformation = nativeDesktopResult.NativeDesktopSections.AdditionalInformation;
        //    AssertSectionMandatoryAndComplete(additionalInformation, false, false);
        //}

        //private async Task<NativeDesktopResult> GetNativeDesktopSectionAsync(IClientApplication clientApplication)
        //{
        //    _mockMediator
        //        .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
        //            It.IsAny<CancellationToken>())).ReturnsAsync(clientApplication);

        //    var result =
        //        (await _nativeDesktopController.GetNativeDesktopAsync(SolutionId)
        //            .ConfigureAwait(false)) as ObjectResult;
        //    result.StatusCode.Should().Be((int)HttpStatusCode.OK);

        //    _mockMediator.Verify(
        //        m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
        //            It.IsAny<CancellationToken>()));

        //    return result.Value as NativeDesktopResult;
        //}

        private static void AssertSectionMandatoryAndComplete(DashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}
