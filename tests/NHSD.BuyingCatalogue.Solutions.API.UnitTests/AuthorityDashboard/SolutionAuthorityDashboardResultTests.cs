using System.Collections.Generic;
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

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.AuthorityDashboard
{
    [TestFixture]
    public sealed class SolutionAuthorityDashboardResultTests
    {
        private const string SolutionId = "Sln1";
        private Mock<IMediator> _mockMediator;
        private SolutionsController _solutionsController;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public void NullSolutionShouldReturnEmptyAuthorityDashboardResult()
        {
            var dashboardAuthority = new SolutionAuthorityDashboardResult(null);
            dashboardAuthority.Id.Should().BeNull();
            dashboardAuthority.Name.Should().BeNull();
            dashboardAuthority.SolutionAuthorityDashboardSections.Should().BeNull();
        }

        [TestCase(0, "INCOMPLETE")]
        [TestCase(1, "COMPLETE")]
        [TestCase(2, "COMPLETE")]
        public async Task ShouldGetAuthorityDashboardCompleteCapabilities(int capabilityCount, string result)
        {
            var claimedCapabilityMock = new Mock<IClaimedCapability>();
            var claimedCapabilities = new List<IClaimedCapability>();
            
            for (int i = 0; i < capabilityCount; i++)
            {
                claimedCapabilities.Add(claimedCapabilityMock.Object);
            }

            var dashboardAuthorityResult =
                await GetSolutionAuthorityDashboardSectionAsync(Mock.Of<ISolution>(s =>
                        s.Capabilities == claimedCapabilities))
                    .ConfigureAwait(false);

            dashboardAuthorityResult.SolutionAuthorityDashboardSections.Should().NotBeNull();
            dashboardAuthorityResult.SolutionAuthorityDashboardSections.Capabilities.Status.Should().Be(result);
        }

        [TestCase(0, 0, "INCOMPLETE")]
        [TestCase(0, 1, "INCOMPLETE")]
        [TestCase(1, 0, "INCOMPLETE")]
        [TestCase(1, 1, "COMPLETE")]
        [TestCase(1, 2, "COMPLETE")]
        public async Task ShouldGetAuthorityDashboardCompleteEpics(int capabilityCount, int epicCount, string result)
        {
            var claimedCapabilityMock = new Mock<IClaimedCapability>();
            var claimedCapabilities = new List<IClaimedCapability>();
            
            for (int capabilityIndex = 0; capabilityIndex < capabilityCount; capabilityIndex++)
            {
                var claimedCapabilityEpicMock = new Mock<IClaimedCapabilityEpic>();
                var claimedCapabilityEpics = new List<IClaimedCapabilityEpic>();
            
                for (int index = 0; index < epicCount; index++)
                {
                    claimedCapabilityEpics.Add(claimedCapabilityEpicMock.Object);
                }

                claimedCapabilityMock.SetupGet(x => x.ClaimedEpics).Returns(claimedCapabilityEpics);
                claimedCapabilities.Add(claimedCapabilityMock.Object);
            }

            var dashboardAuthorityResult =
                await GetSolutionAuthorityDashboardSectionAsync(Mock.Of<ISolution>(s =>
                        s.Capabilities == claimedCapabilities))
                    .ConfigureAwait(false);

            dashboardAuthorityResult.SolutionAuthorityDashboardSections.Should().NotBeNull();
            dashboardAuthorityResult.SolutionAuthorityDashboardSections.Epics.Status.Should().Be(result);
        }

        [TestCase(null, null)]
        [TestCase(SolutionId, null)]
        [TestCase(null, "Bob")]
        [TestCase(SolutionId, "Bob")]
        public async Task ShouldReturnNameId(string id, string name)
        {
            var dashboardResult =
                await GetSolutionAuthorityDashboardSectionAsync(Mock.Of<ISolution>(s => s.Id == id && s.Name == name))
                    .ConfigureAwait(false);
            dashboardResult.Id.Should().Be(id);
            dashboardResult.Name.Should().Be(name);
        }

        private async Task<SolutionAuthorityDashboardResult> GetSolutionAuthorityDashboardSectionAsync(
            ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result =
                (await _solutionsController.AuthorityDashboard(SolutionId).ConfigureAwait(false))
                .Result as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as SolutionAuthorityDashboardResult;
        }
    }
}
