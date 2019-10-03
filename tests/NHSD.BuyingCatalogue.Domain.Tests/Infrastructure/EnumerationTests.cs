using System;
using NHSD.BuyingCatalogue.Domain.Infrastructure;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Domain.Tests.Infrastructure
{
    [TestFixture]
    public class EnumerationTests
    {
        [Test]
        public void GivenSameValues_ShouldBeEqual()
        {
            SizeType.Small.Equals(SizeType.Small);
        }

        [Test]
        public void GivenDifferentValues_ShouldNotBeEqual()
        {
            SizeType.Small.ShouldNotBe(SizeType.Large);
        }

        [Test]
        public void GetAll_ShouldReturnAllValues()
        {
            var expected = new[]
            {
                SizeType.Small,
                SizeType.Medium,
                SizeType.Large
            };

            var actual = Enumeration.GetAll<SizeType>();

            actual.ShouldBe(expected, true);
        }

        [Test]
        public void FromValue_GivenValueSmall_ShouldBeEqual()
        {
            Enumeration.FromValue<SizeType>(1).ShouldBe(SizeType.Small);
        }

        [Test]
        public void FromValue_GivenValueFour_ShouldThrow()
        {
            var expectedValue = 4;
            
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromValue<SizeType>(expectedValue);
            });

            exception.Message.ShouldBe($"'{expectedValue}' is not a valid value in {typeof(SizeType)}");
        }

        [Test]
        public void FromName_GivenNameMedium_ShouldBeEqual()
        {
            Enumeration.FromName<SizeType>("medium").ShouldBe(SizeType.Medium);            
        }

        [Test]
        public void FromName_GivenNameExtraLarge_ShouldThrow()
        {
            var expectedName = "extra large";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Enumeration.FromName<SizeType>(expectedName);
            });

            exception.Message.ShouldBe($"'{expectedName}' is not a valid name in {typeof(SizeType)}");
        }

        [Test]
        public void CompareTo_GivenSizeTypeSmall_ShouldBeEqual()
        {
            SizeType.Small.CompareTo(SizeType.Small).ShouldBe(0);   
        }

        [Test]
        public void ToString_GivenSizeTypeSmall_ShouldBeEqual()
        {
            SizeType.Small.ToString().ShouldBe("Small");
        }

        private sealed class SizeType : Enumeration
        {
            public static SizeType Small = new SizeType(1, "Small");
            public static SizeType Medium = new SizeType(2, "Medium");
            public static SizeType Large = new SizeType(3, "Large");

            private SizeType(int id, string name) : base(id, name)
            {
            }
        }
    }
}
