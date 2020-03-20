using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class LearnMoreSectionTests
    {
        [Test]
        public void IfPopulated_AnswersHasDataIsFalse_ReturnsNull()
        {
            var mockLearnMore = Mock.Of<ISolutionDocument>();
            var section = new LearnMoreSection(mockLearnMore);

            section.IfPopulated().Should().BeNull();
        }

        [Test]
        public void IfPopulated_AnswersHasDataIsTrue_ReturnsSelf()
        {
            var mockLearnMore = Mock.Of<ISolutionDocument>(l => l.Name == "Document");
            var section = new LearnMoreSection(mockLearnMore);

            section.IfPopulated().Should().Be(section);
        }
    }
}
