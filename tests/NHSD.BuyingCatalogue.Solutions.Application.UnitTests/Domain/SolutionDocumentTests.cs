using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Domain
{
    [TestFixture]
    internal sealed class SolutionDocumentTests
    {
        [Test]
        public void Constructor_String_InitializesDocumentName()
        {
            const string documentName = "TheDocument.pdf";

            var solutionDocument = new SolutionDocument(documentName);

            solutionDocument.Name.Should().Be(documentName);
        }
    }
}
