using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public class SolutionsControllerGetDashboardAuthorityTests
    {
        private Mock<IMediator> _mockMediator;
        private SolutionsController _solutionsController;
        private const string SolutionId = "Sln1";

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _solutionsController.DashboardAuthority(SolutionId).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as SolutionDashboardAuthorityResult).Id.Should().BeNull();
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionDashboardAuthoritySection()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDashboardAuthoritySections(null));
        }

        [Test]
        public void NullSolutionShouldReturnEmptyDashboardAuthorityResult()
        {
            var dashboardAuthority = new SolutionDashboardAuthorityResult(null);
            dashboardAuthority.Id.Should().BeNull();
            dashboardAuthority.Name.Should().BeNull();
            dashboardAuthority.SolutionDashboardAuthoritySections.Should().BeNull();
        }

        [TestCase(null, null)]
        [TestCase(SolutionId, null)]
        [TestCase(null, "Bob")]
        [TestCase(SolutionId, "Bob")]
        public async Task ShouldReturnNameId(string id, string name)
        {
            var dashboardResult =
                await GetSolutionDashboardAuthoritySectionAsync(Mock.Of<ISolution>(s => s.Id == id && s.Name == name))
                    .ConfigureAwait(false);
            dashboardResult.Id.Should().Be(id);
            dashboardResult.Name.Should().Be(name);
        }

        [Test]
        public async Task ShouldReturnSolutionDashboardAuthorityStaticData()
        {
            var dashboardResult = await GetSolutionDashboardAuthoritySectionAsync(Mock.Of<ISolution>()).ConfigureAwait(false);
            var solutionDashboardSections = dashboardResult.SolutionDashboardAuthoritySections;

            solutionDashboardSections.Should().NotBeNull();
            solutionDashboardSections.Capabilities.Should().NotBeNull();
            solutionDashboardSections.Capabilities.Requirement.Should().Be("Mandatory");
            solutionDashboardSections.Capabilities.Status.Should().Be("INCOMPLETE");
        }

        [TestCase(new string[0], "INCOMPLETE")]
        [TestCase(new[]{"      "}, "INCOMPLETE")]
        [TestCase(new[] { "", " " }, "INCOMPLETE")]
        [TestCase(new[]{"Capability1"}, "COMPLETE")]
        [TestCase(new[] { "     Capability1 " }, "COMPLETE")]
        [TestCase(new[] { "Capability1", "Capability2" }, "COMPLETE")]
        public async Task ShouldGetDashboardAuthorityCompleteCapabilities(string[] capabilities, string result)
        {
            var dashboardAuthorityResult =
                await GetSolutionDashboardAuthoritySectionAsync(Mock.Of<ISolution>(s => s.Capabilities == capabilities))
                    .ConfigureAwait(false);

            dashboardAuthorityResult.SolutionDashboardAuthoritySections.Should().NotBeNull();
            dashboardAuthorityResult.SolutionDashboardAuthoritySections.Capabilities.Status.Should().Be(result);
        }

        private async Task<SolutionDashboardAuthorityResult> GetSolutionDashboardAuthoritySectionAsync(ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.DashboardAuthority(SolutionId).ConfigureAwait(false)).Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as SolutionDashboardAuthorityResult;
        }
    }
}
