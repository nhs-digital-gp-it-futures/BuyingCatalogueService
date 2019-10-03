using System;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NHSD.BuyingCatalogue.Domain.Infrastructure;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Domain.Tests.Solutions
{
    [TestFixture]
    public sealed class SupplierStatusTests
    {
        [Test]
        public void GivenSameValues_ShouldBeEqual()
        {
            SupplierStatus.Draft.ShouldBe(SupplierStatus.Draft);
        }

        [Test]
        public void GivenDifferentValues_ShouldNotBeEqual()
        {
            SupplierStatus.Draft.ShouldNotBe(SupplierStatus.AuthorityReview);
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

            actual.ShouldBe(expected, true);
        }

        [Test]
        public void FromValue_GivenValueOne_ShouldBeDraft()
        {
            Enumeration.FromValue<SupplierStatus>(1).ShouldBe(SupplierStatus.Draft);
        }

        [Test]
        public void FromValue_GivenValueTwo_ShouldBeAuthorityReview()
        {
            Enumeration.FromValue<SupplierStatus>(2).ShouldBe(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromValue_GivenValueFour_ShouldThrow()
        {
            var expectedValue = 4;

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromValue<SupplierStatus>(expectedValue);
            });

            exception.Message.ShouldBe($"'{expectedValue}' is not a valid value in {typeof(SupplierStatus)}");
        }

        [Test]
        public void FromName_GivenNameAuthorityReview_ShouldBeEqual()
        {
            Enumeration.FromName<SupplierStatus>("authorityreview").ShouldBe(SupplierStatus.AuthorityReview);
        }

        [Test]
        public void FromName_GivenNameTest_ShouldThrow()
        {
            var expectedName = "Test";

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var actual = Enumeration.FromName<SupplierStatus>(expectedName);
            });

            exception.Message.ShouldBe($"'{expectedName}' is not a valid name in {typeof(SupplierStatus)}");
        }

        [Test]
        public void CompareTo_GivenSupplierStatusDraft_ShouldBeEqual()
        {
            SupplierStatus.Draft.CompareTo(SupplierStatus.Draft).ShouldBe(0);
        }

        [Test]
        public void ToString_GivenSupplierStatusDraft_ShouldBeEqual()
        {
            SupplierStatus.Draft.ToString().ShouldBe("Draft");
        }
    }
}
