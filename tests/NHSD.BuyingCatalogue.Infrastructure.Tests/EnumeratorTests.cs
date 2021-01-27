using System;
using FluentAssertions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    internal sealed class EnumeratorTests
    {
        [Test]
        public void GivenSameValuesShouldBeEqual()
        {
            SizeType.Small.Should().Be(SizeType.Small);
        }

        [Test]
        public void GivenDifferentValuesShouldNotBeEqual()
        {
            SizeType.Small.Should().NotBe(SizeType.Large);
        }

        [Test]
        public void GivenNullValuesShouldNotBeEqual()
        {
            SizeType.Small.Equals(null).Should().BeFalse();
        }

        [Test]
        public void GivenNonEnumerableValuesShouldNotBeEqual()
        {
            SizeType.Small.Should().NotBe("Elephant");
        }

        [Test]
        public void GivenPrimitiveValuesShouldNotBeEqual()
        {
            SizeType.Small.Should().NotBe(4);
        }

        [Test]
        public void GetAllShouldReturnAllValues()
        {
            var expected = new[]
            {
                SizeType.Small,
                SizeType.Medium,
                SizeType.Large,
            };

            var actual = Enumerator.GetAll<SizeType>();

            actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Test]
        public void FromValueGivenValueSmallShouldBeEqual()
        {
            Enumerator.FromValue<SizeType>(1).Should().Be(SizeType.Small);
        }

        [Test]
        public void FromValueGivenValueFourShouldThrow()
        {
            const int expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(
                () => _ = Enumerator.FromValue<SizeType>(expectedValue));

            // ReSharper disable once PossibleNullReferenceException
            exception.Message.Should().Be($"'{expectedValue}' is not a valid value in {typeof(SizeType)}");
        }

        [Test]
        public void FromNameGivenNameMediumShouldBeEqual()
        {
            Enumerator.FromName<SizeType>("medium").Should().Be(SizeType.Medium);
        }

        [Test]
        public void FromNameGivenNameExtraLargeShouldThrow()
        {
            const string expectedName = "extra large";

            var exception = Assert.Throws<InvalidOperationException>(
                () => _ = Enumerator.FromName<SizeType>(expectedName));

            // ReSharper disable once PossibleNullReferenceException
            exception.Message.Should().Be($"'{expectedName}' is not a valid name in {typeof(SizeType)}");
        }

        [Test]
        public void ToStringGivenSizeTypeSmallShouldBeEqual()
        {
            SizeType.Small.ToString().Should().Be("Small");
        }

        [Test]
        public void GetHashCodeShouldBeEqualToIdHashCode()
        {
            SizeType.Small.GetHashCode().Should().Be(1.GetHashCode());
        }

        private sealed class SizeType : Enumerator
        {
            public static readonly SizeType Small = new(1, "Small");

            public static readonly SizeType Medium = new(2, "Medium");

            public static readonly SizeType Large = new(3, "Large");

            private SizeType(int id, string name)
                : base(id, name)
            {
            }
        }
    }
}
