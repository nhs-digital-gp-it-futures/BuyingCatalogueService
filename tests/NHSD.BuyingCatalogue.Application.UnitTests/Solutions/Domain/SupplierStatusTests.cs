using System;
using FluentAssertions;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Infrastructure;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Domain.Tests.Solutions
{
    [TestFixture]
    public sealed class SupplierStatusTests
    {
        [Test]
        public void GivenSameValues_ShouldBeEqual()
        {
            SupplierStatus.Draft.Should().Be(SupplierStatus.Draft);
        }

        [Test]
        public void GivenDifferentValues_ShouldNotBeEqual()
        {
            SupplierStatus.Draft.Should().NotBe(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void GetAll_ShouldReturnAllValues()
        {
            var expected = new[]
            {
                SupplierStatus.Draft,
                SupplierStatus.AuthorityReview
            };

            var actual = Enumeration.GetAll<SupplierStatus>();

            actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Test]
        public void FromValue_GivenValueOne_ShouldBeDraft()
        {
            Enumeration.FromValue<SupplierStatus>(1).Should().Be(SupplierStatus.Draft);
        }

        [Test]
        public void FromValue_GivenValueTwo_ShouldBeAuthorityReview()
        {
            Enumeration.FromValue<SupplierStatus>(2).Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromValue_GivenValueFour_ShouldThrow()
        {
            var expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromValue<SupplierStatus>(expectedValue);
            });

            exception.Message.Should().Be($"'{expectedValue}' is not a valid value in {typeof(SupplierStatus)}");
        }

        [Test]
        public void FromName_GivenNameAuthorityReview_ShouldBeEqual()
        {
            Enumeration.FromName<SupplierStatus>("authorityreview").Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromName_GivenNameTest_ShouldThrow()
        {
            var expectedName = "Test";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromName<SupplierStatus>(expectedName);
            });

            exception.Message.Should().Be($"'{expectedName}' is not a valid name in {typeof(SupplierStatus)}");
        }

        [Test]
        public void CompareTo_GivenSupplierStatusDraft_ShouldBeEqual()
        {
            SupplierStatus.Draft.CompareTo(SupplierStatus.Draft).Should().Be(0);
        }

        [Test]
        public void ToString_GivenSupplierStatusDraft_ShouldBeEqual()
        {
            SupplierStatus.Draft.ToString().Should().Be("Draft");
        }
    }
}
