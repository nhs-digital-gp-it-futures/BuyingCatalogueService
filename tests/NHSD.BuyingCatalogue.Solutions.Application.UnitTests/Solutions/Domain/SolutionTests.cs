using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
    internal sealed class SolutionTests
    {
        [Test]
        public void GivenSolutionCheckSupplierStatusShouldBeEqualToDraft()
        {
            var expected = SupplierStatus.Draft;

            var solution = new Solution();

            solution.SupplierStatus.Should().Be(expected);
        }
    }
}
