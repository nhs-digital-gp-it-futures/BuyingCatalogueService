using FluentAssertions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    internal sealed class BooleanExtensionsTests
    {
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        [TestCase(null, null)]
        public void ShouldMapToYesNoStringToTrue(bool? input, string result)
        {
            input.ToYesNoString().Should().Be(result);
        }
    }
}
