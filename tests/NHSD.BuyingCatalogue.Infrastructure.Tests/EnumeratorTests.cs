using System;
using FluentAssertions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    public class EnumeratorTests
    {
        [Test]
        public void GivenSameValuesShouldBeEqual()
        {
            SizeType.Small.Equals(SizeType.Small);
        }

        [Test]
        public void GivenDifferentValuesShouldNotBeEqual()
        {
            SizeType.Small.Should().NotBe(SizeType.Large);
        }

        [Test]
        public void GivenNullValuesShouldNotBeEqual()
        {
            SizeType nullValue = null;
            SizeType.Small.Equals(nullValue).Should().BeFalse();
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
                SizeType.Large
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
            var expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumerator.FromValue<SizeType>(expectedValue);
            });

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
            var expectedName = "extra large";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Enumerator.FromName<SizeType>(expectedName);
            });

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
            public static SizeType Small = new(1, "Small");
            public static SizeType Medium = new(2, "Medium");
            public static SizeType Large = new(3, "Large");

            private SizeType(int id, string name) : base(id, name)
            {
            }
        }
    }
}
