using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerGetByIdTestsBrowsersSupported : SolutionsControllerGetByIdTestsBase
    {
        [Test]
        public async Task ShouldRepresentBrowsersSupportedSectionStaticProperties()
        {
            var browsersSupportedSection = await GetBrowserSupportedSectionAsync(new[] {"Edge", "Google Chrome"}, true);

            browsersSupportedSection.Mandatory.Should().BeEquivalentTo("supported-browsers", "mobile-responsive");
            browsersSupportedSection.Id.Should().Be("browsers-supported");
            browsersSupportedSection.Requirement.Should().Be("Mandatory");
        }

        [Test]
        public async Task ShouldRepresentBrowsersSupportedSectionSupportedBrowsers()
        {
            var browsersSupportedSection = await GetBrowserSupportedSectionAsync(new[] {"Edge", "Google Chrome"}, null);

            browsersSupportedSection.Data.SupportedBrowsers.Should().BeEquivalentTo(new[] {"Edge", "Google Chrome"});
        }

        [TestCase(true, "yes")]
        [TestCase(false, "no")]
        [TestCase(null, null)]
        public async Task ShouldRepresentBrowsersSupportedSectionMobileResponsive(bool? mobileResponsive,
            string expectedResult)
        {
            var browsersSupportedSection =
                await GetBrowserSupportedSectionAsync(new[] {"Edge", "Google Chrome"}, mobileResponsive);

            browsersSupportedSection.Data.MobileResponsive.Should().Be(expectedResult);
        }

        [Test]
        public async Task ShouldRepresentBrowsersSupportedSectionEmpty()
        {
            var browsersSupportedSection = await GetBrowserSupportedSectionAsync(new string[0], null);

            browsersSupportedSection.Data.SupportedBrowsers.Should().BeEmpty();
            browsersSupportedSection.Data.MobileResponsive.Should().BeNull();
        }

        [TestCase(false, false, "INCOMPLETE")]
        [TestCase(false, true, "INCOMPLETE")]
        [TestCase(false, null, "INCOMPLETE")]
        [TestCase(true, false, "COMPLETE")]
        [TestCase(true, true, "COMPLETE")]
        [TestCase(true, null, "INCOMPLETE")]
        public async Task ShouldBeCompleteOnlyIfBothSectionsComplete(bool someBrowsersSupported, bool? mobileResponsive,
            string expectedStatus)
        {
            var browsersSupported = someBrowsersSupported ? new[] {"Edge", "Google Chrome"} : new string[0];

            var browsersSupportedSection = await GetBrowserSupportedSectionAsync(browsersSupported, mobileResponsive);

            browsersSupportedSection.Status.Should().Be(expectedStatus);
        }

        private async Task<BrowsersSupportedSection> GetBrowserSupportedSectionAsync(
            IEnumerable<string> browsersSupported, bool? mobileResponsive)
        {

            return (await GetSolutionViewModel(clientApplicationTypes: new[] {"browser-based"},
                    browsersSupported: browsersSupported, mobileResponsive: mobileResponsive))
                .MarketingData
                .Sections.First(s => s.Id == "client-application-types")
                .Sections.First(s => s.Id == "browser-based")
                .Sections.First(s => s.Id == "browsers-supported") as BrowsersSupportedSection;
        }
    }
}
