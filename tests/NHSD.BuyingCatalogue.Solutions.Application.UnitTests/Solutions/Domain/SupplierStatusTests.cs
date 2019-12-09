using System;
using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
    public sealed class SupplierStatusTests
    {
        [Test]
        public void GivenSameValuesShouldBeEqual()
        {
            SupplierStatus.Draft.Should().Be(SupplierStatus.Draft);
        }

        [Test]
        public void GivenDifferentValuesShouldNotBeEqual()
        {
            SupplierStatus.Draft.Should().NotBe(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void GetAllShouldReturnAllValues()
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
        public void FromValueGivenValueOneShouldBeDraft()
        {
            Enumeration.FromValue<SupplierStatus>(1).Should().Be(SupplierStatus.Draft);
        }

        [Test]
        public void FromValueGivenValueTwoShouldBeAuthorityReview()
        {
            Enumeration.FromValue<SupplierStatus>(2).Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromValueGivenValueFourShouldThrow()
        {
            var expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromValue<SupplierStatus>(expectedValue);
            });

            exception.Message.Should().Be($"'{expectedValue}' is not a valid value in {typeof(SupplierStatus)}");
        }

        [Test]
        public void FromNameGivenNameAuthorityReviewShouldBeEqual()
        {
            Enumeration.FromName<SupplierStatus>("authorityreview").Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromNameGivenNameTestShouldThrow()
        {
            var expectedName = "Test";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromName<SupplierStatus>(expectedName);
            });

            exception.Message.Should().Be($"'{expectedName}' is not a valid name in {typeof(SupplierStatus)}");
        }

        [Test]
        public void CompareToGivenSupplierStatusDraftShouldBeEqual()
        {
            SupplierStatus.Draft.CompareTo(SupplierStatus.Draft).Should().Be(0);
        }

        [Test]
        public void ToStringGivenSupplierStatusDraftShouldBeEqual()
        {
            SupplierStatus.Draft.ToString().Should().Be("Draft");
        }
    }
}
