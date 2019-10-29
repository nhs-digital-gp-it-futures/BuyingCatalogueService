using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerGetByIdTestsSolutionDescription : SolutionsControllerGetByIdTestsBase
    {
        [Test]
        public async Task ShouldRepresentSolutionDescriptionSectionStaticProperties()
        {
            var solutionDescriptionSection = await GetSolutionDescriptionSectionAsync(null, null, null);

            solutionDescriptionSection.Mandatory.Should().BeEquivalentTo("summary");
            solutionDescriptionSection.Id.Should().Be("solution-description");
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
        }

        [Test]
        public async Task ShouldRepresentSolutionDescriptionSectionData()
        {
            var solutionDescriptionSection = await GetSolutionDescriptionSectionAsync("summary", "description", "link");

            solutionDescriptionSection.Data.Summary.Should().Be("summary");
            solutionDescriptionSection.Data.Description.Should().Be("description");
            solutionDescriptionSection.Data.Link.Should().Be("link");
        }

        [Test]
        public async Task ShouldRepresentSolutionDescriptionSectionEmpty()
        {
            var solutionDescriptionSection = await GetSolutionDescriptionSectionAsync(null, null, null);

            solutionDescriptionSection.Data.Summary.Should().BeNull();
            solutionDescriptionSection.Data.Description.Should().BeNull();
            solutionDescriptionSection.Data.Link.Should().BeNull();
        }

        [TestCase(null, null, null, "INCOMPLETE")]
        [TestCase(null, "description", null, "INCOMPLETE")]
        [TestCase(null, "description", "link", "INCOMPLETE")]
        [TestCase(null, null, "link", "INCOMPLETE")]
        [TestCase("summary", null, null, "COMPLETE")]
        [TestCase("summary", "description", null, "COMPLETE")]
        [TestCase("summary", null, "link", "COMPLETE")]

        [TestCase("", "", "    ", "INCOMPLETE")]
        [TestCase("", "description", "", "INCOMPLETE")]
        [TestCase(" ", "description", "link", "INCOMPLETE")]
        [TestCase("    ", "", "link", "INCOMPLETE")]
        [TestCase("summary", "", "", "COMPLETE")]
        [TestCase("summary", "description", "", "COMPLETE")]
        [TestCase("summary", "    ", "link", "COMPLETE")]
        public async Task ShouldBeCompleteOnlyIfSummaryComplete(string summary, string description, string link, string expectedStatus)
        {
            var solutionDescriptionSection = await GetSolutionDescriptionSectionAsync(summary, description, link);

            solutionDescriptionSection.Status.Should().Be(expectedStatus);
        }

        private async Task<SolutionDescriptionSection> GetSolutionDescriptionSectionAsync(
            string summary, string description, string link)
        {

            return (await GetSolutionViewModel(summary: summary,
                    description: description,
                    link: link))
                .MarketingData
                .Sections.First(s => s.Id == "solution-description") as SolutionDescriptionSection;
        }
    }
}
