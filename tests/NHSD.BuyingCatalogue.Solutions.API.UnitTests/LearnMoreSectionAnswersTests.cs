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
        public void Constructor_ISolutionDocument_DocumentNameIsAssigned()
        {
            const string documentName = "SomeSolution.pdf";

            var mockSolutionDocument = Mock.Of<ISolutionDocument>(l => l.Name == documentName);
            var sectionAnswers = new LearnMoreSectionAnswers(mockSolutionDocument);

            sectionAnswers.DocumentName.Should().Be(documentName);
        }

        [Test]
        public void Constructor_ISolutionDocument_NullObject_DocumentNameIsNull()
        {
            var sectionAnswers = new LearnMoreSectionAnswers(null);

            sectionAnswers.DocumentName.Should().BeNull();
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("\t", false)]
        [TestCase("SomeSolution.pdf", true)]
        public void HasData_ReturnsExpectedValue(string name, bool expectedValue)
        {
            var mockSolutionDocument = Mock.Of<ISolutionDocument>(l => l.Name == name);
            var sectionAnswers = new LearnMoreSectionAnswers(mockSolutionDocument);

            sectionAnswers.HasData.Should().Be(expectedValue);
        }
    }
}
