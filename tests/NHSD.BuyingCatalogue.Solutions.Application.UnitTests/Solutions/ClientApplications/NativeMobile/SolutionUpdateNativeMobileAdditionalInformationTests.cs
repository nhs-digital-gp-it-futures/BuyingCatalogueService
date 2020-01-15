using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeMobileAdditionalInformationTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateNativeMobileAdditionInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileAdditionalInformation("Some info").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileAdditionalInformation").Value<string>() == "Some info"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateNativeMobileAdditionalInformationToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("NativeMobileAdditionalInformation").Should().BeNullOrEmpty();
                });

            var validationResult = await UpdateNativeMobileAdditionalInformation(null).ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileAdditionalInformationAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{'ClientApplicationTypes' : [ 'native-mobile' ], 'NativeMobileFirstDesign': true, 'MobileOperatingSystems': { 'OperatingSystems': ['Windows', 'Linux'], 'OperatingSystemsDescription': 'For windows only version 10' }, 'MobileConnectionDetails': { 'ConnectionType': ['3G', '4G'], 'Description': 'A description', 'MinimumConnectionSpeed': '1GBps' }, 'MobileMemoryAndStorage': { 'Description': 'A description', 'MinimumMemoryRequirement': '1GB' }, 'NativeMobileAdditionalInformation': 'native mobile additional info' }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("NativeMobileAdditionalInformation").Value<string>().Should()
                        .Be("Updated Additional Info");

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile" });
                    json.SelectToken("NativeMobileFirstDesign").Value<bool>().Should().BeTrue();
                    json.ReadStringArray("MobileOperatingSystems.OperatingSystems").ShouldContainOnly(new List<string> { "Windows", "Linux" });
                    json.SelectToken("MobileOperatingSystems.OperatingSystemsDescription").Value<string>().Should().Be("For windows only version 10");
                    json.ReadStringArray("MobileConnectionDetails.ConnectionType").ShouldContainOnly(new List<string> { "3G", "4G" });
                    json.SelectToken("MobileConnectionDetails.Description").Value<string>().Should().Be("A description");
                    json.SelectToken("MobileConnectionDetails.MinimumConnectionSpeed").Value<string>().Should().Be("1GBps");
                    json.SelectToken("MobileMemoryAndStorage.Description").Value<string>().Should().Be("A description");
                    json.SelectToken("MobileMemoryAndStorage.MinimumMemoryRequirement").Value<string>().Should().Be("1GB");
                });

            var validationResult = await UpdateNativeMobileAdditionalInformation("Updated Additional Info").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeMobileAdditionalInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileAdditionalInformation(new string('a', 501)).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["additional-information"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeMobileAdditionalInformation("New Additional Info"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeMobileAdditionalInformation(string additionalInformation)
        {
            return await Context.UpdateSolutionNativeMobileAdditionalInformationHandler.Handle(
                new UpdateSolutionNativeMobileAdditionalInformationCommand(SolutionId,
                    new UpdateSolutionNativeMobileAdditionalInformationViewModel
                    {
                        NativeMobileAdditionalInformation = additionalInformation
                    }), new CancellationToken()).ConfigureAwait(false);
        }
    }
}
