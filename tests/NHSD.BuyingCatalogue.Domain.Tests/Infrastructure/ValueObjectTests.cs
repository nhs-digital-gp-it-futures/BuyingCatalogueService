using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Infrastructure;
using NUnit.Framework;
using Shouldly;

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

            size1.Equals(size2).ShouldBeTrue();
        }

        [Test]
        public void GivenDifferentValues_ShouldReturnFalse()
        {
            var size1 = new Size(1, 2);
            var size2 = new Size(2, 1);

            size1.Equals(size2).ShouldBeFalse();
        }

        private class Size : ValueObject
        {
            public int Width { get; private set; }
            public int Height { get; private set; }

            private Size() { }

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
