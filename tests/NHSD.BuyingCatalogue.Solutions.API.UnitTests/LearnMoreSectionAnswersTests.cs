using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class LearnMoreSectionAnswersTests
    {
        [Test]
        public void Constructor_ILearnMore_DocumentNameIsAssigned()
        {
            const string documentName = "SomeSolution.pdf";

            var mockLearnMore = Mock.Of<ILearnMore>(l => l.DocumentName == documentName);
            var sectionAnswers = new LearnMoreSectionAnswers(mockLearnMore);

            sectionAnswers.DocumentName.Should().Be(documentName);
        }

        [Test]
        public void Constructor_ILearnMore_NullObject_DocumentNameIsNull()
        {
            var sectionAnswers = new LearnMoreSectionAnswers(null);

            sectionAnswers.DocumentName.Should().BeNull();
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("\t", false)]
        [TestCase("SomeSolution.pdf", true)]
        public void HasData_ReturnsExpectedValue(string documentName, bool expectedValue)
        {
            var mockLearnMore = Mock.Of<ILearnMore>(l => l.DocumentName == documentName);
            var sectionAnswers = new LearnMoreSectionAnswers(mockLearnMore);

            sectionAnswers.HasData.Should().Be(expectedValue);
        }
    }
}
