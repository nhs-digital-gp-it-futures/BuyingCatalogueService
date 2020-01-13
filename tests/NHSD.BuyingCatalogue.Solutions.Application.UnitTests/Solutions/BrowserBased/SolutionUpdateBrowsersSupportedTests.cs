using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.BrowserBased
{
    [TestFixture]
    internal sealed class SolutionUpdateBrowsersSupportedTests : ClientApplicationTestsBase
    {

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes")
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication).ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Edge", "Google Chrome" }).Count() == 2
                && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool>() == true
            ), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupportedAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile", "browser-based" });
                    json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Google Chrome", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            var validationResult = await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes")
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateEmptySolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowsersSupported(new HashSet<string>())
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["supported-browsers"].Should().Be("required");
            results["mobile-responsive"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var viewModel = new Mock<IUpdateBrowserBasedBrowsersSupportedData>();
            var trimmedViewModel = Mock.Of<IUpdateBrowserBasedBrowsersSupportedData>();
            viewModel.Setup(x => x.Trim()).Returns(trimmedViewModel);

            var command = new UpdateSolutionBrowsersSupportedCommand("Sln1", viewModel.Object);
            viewModel.Verify(x => x.Trim(), Times.Once);
            command.Data.IsSameOrEqualTo(trimmedViewModel);
        }

        private async Task<ISimpleResult> UpdateBrowsersSupported(HashSet<string> browsersSupported, string mobileResponsive = null)
        {
            var trimmedData = Mock.Of<IUpdateBrowserBasedBrowsersSupportedData>(t =>
                t.BrowsersSupported == browsersSupported && t.MobileResponsive == mobileResponsive);

            var data = new Mock<IUpdateBrowserBasedBrowsersSupportedData>();
            data.Setup(s => s.Trim()).Returns(trimmedData);
            
            return await Context.UpdateSolutionBrowsersSupportedHandler.Handle(
                new UpdateSolutionBrowsersSupportedCommand("Sln1", data.Object),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
