using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionDocumentSectionAnswersTests
    {
        [Test]
        public void Constructor_ISolutionDocument_DocumentNameIsAssigned()
        {
            const string documentName = "SomeSolution.pdf";

            var mockSolutionDocument = Mock.Of<ISolutionDocument>(l => l.Name == documentName);
            var sectionAnswers = new SolutionDocumentSectionAnswers(mockSolutionDocument);

            sectionAnswers.Name.Should().Be(documentName);
        }

        [Test]
        public void Constructor_ISolutionDocument_NullObject_DocumentNameIsNull()
        {
            var sectionAnswers = new SolutionDocumentSectionAnswers(null);

            sectionAnswers.Name.Should().BeNull();
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("\t", false)]
        [TestCase("SomeSolution.pdf", true)]
        public void HasData_ReturnsExpectedValue(string name, bool expectedValue)
        {
            var mockSolutionDocument = Mock.Of<ISolutionDocument>(l => l.Name == name);
            var sectionAnswers = new SolutionDocumentSectionAnswers(mockSolutionDocument);

            sectionAnswers.HasData.Should().Be(expectedValue);
        }
    }
}
