using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionDocumentSectionTests
    {
        [Test]
        public void IfPopulated_AnswersHasDataIsFalse_ReturnsNull()
        {
            var mockSolutionDocument = Mock.Of<ISolutionDocument>();
            var section = new SolutionDocumentSection(mockSolutionDocument);

            section.IfPopulated().Should().BeNull();
        }

        [Test]
        public void IfPopulated_AnswersHasDataIsTrue_ReturnsSelf()
        {
            var mockSolutionDocument = Mock.Of<ISolutionDocument>(l => l.Name == "Document");
            var section = new SolutionDocumentSection(mockSolutionDocument);

            section.IfPopulated().Should().Be(section);
        }
    }
}
