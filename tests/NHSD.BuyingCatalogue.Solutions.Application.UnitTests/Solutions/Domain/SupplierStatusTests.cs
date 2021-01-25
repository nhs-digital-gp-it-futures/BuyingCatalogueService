using System;
using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
    internal sealed class SupplierStatusTests
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
                SupplierStatus.AuthorityReview,
            };

            var actual = Enumerator.GetAll<SupplierStatus>();

            actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Test]
        public void FromValueGivenValueOneShouldBeDraft()
        {
            Enumerator.FromValue<SupplierStatus>(1).Should().Be(SupplierStatus.Draft);
        }

        [Test]
        public void FromValueGivenValueTwoShouldBeAuthorityReview()
        {
            Enumerator.FromValue<SupplierStatus>(2).Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromValueGivenValueFourShouldThrow()
        {
            const int expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Enumerator.FromValue<SupplierStatus>(expectedValue);
            });

            exception.Message.Should().Be($"'{expectedValue}' is not a valid value in {typeof(SupplierStatus)}");
        }

        [Test]
        public void FromNameGivenNameAuthorityReviewShouldBeEqual()
        {
            Enumerator.FromName<SupplierStatus>("authorityReview").Should().Be(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromNameGivenNameTestShouldThrow()
        {
            const string expectedName = "Test";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Enumerator.FromName<SupplierStatus>(expectedName);
            });

            exception.Message.Should().Be($"'{expectedName}' is not a valid name in {typeof(SupplierStatus)}");
        }

        [Test]
        public void ToStringGivenSupplierStatusDraftShouldBeEqual()
        {
            SupplierStatus.Draft.ToString().Should().Be("Draft");
        }
    }
}
