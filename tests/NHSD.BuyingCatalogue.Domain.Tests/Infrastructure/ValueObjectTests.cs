using System.Collections.Generic;
using FluentAssertions;
using NHSD.BuyingCatalogue.Domain.Infrastructure;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Domain.Tests.Infrastructure
{
    [TestFixture]
    public sealed class ValueObjectTests
    {
        [Test]
        public void GivenSameValues_ShouldReturnTrue()
        {
            var size1 = new Size(1, 2);
            var size2 = new Size(1, 2);

            size1.Equals(size2).Should().BeTrue();
        }

        [Test]
        public void GivenDifferentValues_ShouldReturnFalse()
        {
            var size1 = new Size(1, 2);
            var size2 = new Size(2, 1);

            size1.Equals(size2).Should().BeFalse();
        }

        private class Size : ValueObject
        {
            private int Width { get; }
            private int Height { get; }

            public Size(int width, int height)
            {
                Width = width;
                Height = height;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Width;
                yield return Height;
            }
        }
    }
}
