using System;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.AuthorityDashboard
{
    [TestFixture]
    internal sealed class SolutionAuthorityDashboardSectionsTests
    {
        [Test]
        public void NullSolutionShouldThrowNullExceptionAuthorityDashboardSection()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new SolutionAuthorityDashboardSections(null));
        }

        [Test]
        public void ShouldReturnSolutionAuthorityDashboardStaticData()
        {
            var dashboardResult = new SolutionAuthorityDashboardSections(Mock.Of<ISolution>());

            dashboardResult.Should().NotBeNull();
            dashboardResult.Capabilities.Should().NotBeNull();
            dashboardResult.Capabilities.Requirement.Should().Be("Mandatory");
            dashboardResult.Capabilities.Status.Should().Be("INCOMPLETE");
        }
    }
}
