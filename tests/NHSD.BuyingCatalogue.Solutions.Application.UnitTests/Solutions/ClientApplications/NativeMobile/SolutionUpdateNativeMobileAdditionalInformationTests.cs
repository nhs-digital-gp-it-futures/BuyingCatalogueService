using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

            var validationResult = await UpdateNativeMobileAdditionalInformation("Some info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileAdditionalInformation").Value<string>() == "Some info";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateNativeMobileAdditionalInformationToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("NativeMobileAdditionalInformation").Should().BeNullOrEmpty();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeMobileAdditionalInformation();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileAdditionalInformationAndNothingElse()
        {
            const string clientApplicationJson = "{'ClientApplicationTypes' : [ 'native-mobile' ], "
                + "'NativeMobileFirstDesign': true, "
                + "'MobileOperatingSystems': { 'OperatingSystems': ['Windows', 'Linux'], 'OperatingSystemsDescription': 'For windows only version 10' }, "
                + "'MobileConnectionDetails': { 'ConnectionType': ['3G', '4G'], 'Description': 'A description', 'MinimumConnectionSpeed': '1GBps' }, "
                + "'MobileMemoryAndStorage': { 'Description': 'A description', 'MinimumMemoryRequirement': '1GB' }, "
                + "'NativeMobileAdditionalInformation': 'native mobile additional info' }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("NativeMobileAdditionalInformation")?
                    .Value<string>()
                    .Should()
                    .Be("Updated Additional Info");

                json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile" });
                json.SelectToken("NativeMobileFirstDesign")?.Value<bool>().Should().BeTrue();
                json.ReadStringArray("MobileOperatingSystems.OperatingSystems").ShouldContainOnly(
                    new List<string> { "Windows", "Linux" });

                json.SelectToken("MobileOperatingSystems.OperatingSystemsDescription")?.Value<string>().Should().Be(
                    "For windows only version 10");

                json.ReadStringArray("MobileConnectionDetails.ConnectionType").ShouldContainOnly(
                    new List<string> { "3G", "4G" });

                json.SelectToken("MobileConnectionDetails.Description")?.Value<string>().Should().Be("A description");
                json.SelectToken("MobileConnectionDetails.MinimumConnectionSpeed")?.Value<string>().Should().Be("1GBps");
                json.SelectToken("MobileMemoryAndStorage.Description")?.Value<string>().Should().Be("A description");
                json.SelectToken("MobileMemoryAndStorage.MinimumMemoryRequirement")?.Value<string>().Should().Be("1GB");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeMobileAdditionalInformation("Updated Additional Info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeMobileAdditionalInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileAdditionalInformation(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["additional-information"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeMobileAdditionalInformation("New Additional Info"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeMobileAdditionalInformation(string additionalInformation = null)
        {
            return await Context.UpdateSolutionNativeMobileAdditionalInformationHandler.Handle(
                new UpdateSolutionNativeMobileAdditionalInformationCommand(SolutionId, additionalInformation),
                CancellationToken.None);
        }
    }
}
