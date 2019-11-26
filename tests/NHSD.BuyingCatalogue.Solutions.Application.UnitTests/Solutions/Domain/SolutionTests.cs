using FluentAssertions;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Domain
{
    [TestFixture]
	public class SolutionTests
	{
        [Test]
        public void GivenSolution_CheckSupplierStatus_ShouldBeEqualToDraft()
        {
            //Arrange
            var expected = SupplierStatus.Draft;

            //Act
            var solution = new Solution();

            //Assert
            solution.SupplierStatus.Should().Be(expected);
        }
    }
}