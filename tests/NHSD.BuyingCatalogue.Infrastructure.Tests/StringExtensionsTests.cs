using FluentAssertions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    internal sealed class StringExtensionsTests
    {
        [TestCase("Yes")]
        [TestCase("yes")]
        [TestCase("yEs")]
        [TestCase("yES")]
        [TestCase("YES")]
        public void ShouldMapYesToTrue(string yes)
        {
            yes.ToBoolean().Should().BeTrue();
        }

        [TestCase("No")]
        [TestCase("no")]
        [TestCase("nO")]
        [TestCase("NO")]
        public void ShouldMapNoToFalse(string no)
        {
            no.ToBoolean().Should().BeFalse();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Non")]
        [TestCase("Elephant")]
        [TestCase("Y")]
        [TestCase("N")]
        [TestCase("Yup")]
        [TestCase("Nooaaa")]
        [TestCase("Aye")]
        [TestCase("War and Peace")]
        public void ShouldMapAnythingElseToOther(string other)
        {
            other.ToBoolean().Should().BeNull();
        }
    }
}
