using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeMobileFirstTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateNativeMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileFirst("yes");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId && JToken.Parse(r.ClientApplication)
                    .SelectToken("NativeMobileFirstDesign")
                    .Value<bool>();

            Context.MockSolutionRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldNotUpdateEmptyMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileFirst();
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["mobile-first-design"].Should().Be("required");
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileFirstAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, "
                + "'HardwareRequirements': 'New Hardware', "
                + "'AdditionalInformation': 'New Additional Info', "
                + "'MobileFirstDesign': true, "
                + "'NativeMobileFirstDesign': true }";

            SetUpMockSolutionRepositoryGetByIdAsync(
                clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(
                    new List<string> { "browser-based", "native-mobile" });

                json.ReadStringArray("BrowsersSupported").ShouldContainOnly(
                    new List<string> { "Mozilla Firefox", "Edge" });

                json.SelectToken("MobileResponsive")?.Value<bool>().Should().BeFalse();
                json.SelectToken("Plugins.Required")?.Value<bool>().Should().BeTrue();
                json.SelectToken("Plugins.AdditionalInformation")?.Value<string>().Should().Be("lorem ipsum");
                json.SelectToken("HardwareRequirements")?.Value<string>().Should().Be("New Hardware");
                json.SelectToken("AdditionalInformation")?.Value<string>().Should().Be("New Additional Info");
                json.SelectToken("MobileFirstDesign")?.Value<bool>().Should().BeTrue();
                json.SelectToken("NativeMobileFirstDesign")?.Value<bool>().Should().BeFalse();
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeMobileFirst("no");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeMobileFirst("yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeMobileFirst(string mobileFirstDesign = null)
        {
            return await Context.UpdateSolutionNativeMobileFirstHandler.Handle(
                new UpdateSolutionNativeMobileFirstCommand(SolutionId, mobileFirstDesign),
                CancellationToken.None);
        }
    }
}
