using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    public sealed class ValueObjectTests
    {
        private Size _size1 = new Size(1, 2);

        [Test]
        public void GivenSameValues_ShouldReturnTrue()
        {
            var size2 = new Size(1, 2);

            _size1.Equals(size2).Should().BeTrue();
            (_size1 == size2).Should().BeTrue();
        }

        [Test]
        public void GivenDifferentValues_ShouldReturnFalse()
        {
            var size2 = new Size(2, 1);

            _size1.Equals(size2).Should().BeFalse();
            (_size1 != size2).Should().BeTrue();
        }

        [Test]
        public void GivenNullComparison_ShouldReturnFalse()
        {
            ValueObject size2 = null;
            _size1.Equals(size2).Should().BeFalse();
            (_size1 == size2).Should().BeFalse();
            (size2 == _size1).Should().BeFalse();
        }

        [Test]
        public void GivenTwoNulls_ShouldReturnTrue()
        {
            _size1 = null;
            ValueObject size2 = null;
            (_size1 == size2).Should().BeTrue();
        }

        [Test]
        public void GivenInvalidObjectType_ShouldReturnFalse()
        {
            _size1.Equals("Yo Heave Ho").Should().BeFalse();
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
