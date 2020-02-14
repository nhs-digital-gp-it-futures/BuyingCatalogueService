using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Domain
{
    [TestFixture]
    internal sealed class LearnMoreTests
    {
        [Test]
        public void Constructor_String_InitializesDocumentName()
        {
            const string documentName = "TheDocument.pdf";

            var learnMore = new LearnMore(documentName);

            learnMore.DocumentName.Should().Be(documentName);
        }
    }
}
