using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class GetRoadMapBySolutionIdTests
    {
        private const string SolutionId = "Sln1";
        private TestContext context;
        private GetRoadMapBySolutionIdQuery query;
        private CancellationToken cancellationToken;
        private string roadMapDescription = "Some road map description";
        private Mock<IRoadMapResult> mockResult;
        private bool solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            query = new GetRoadMapBySolutionIdQuery(SolutionId);
            cancellationToken = CancellationToken.None;

            context.MockSolutionRepository
                .Setup(r => r.GetRoadMapBySolutionIdAsync(SolutionId, cancellationToken))
                .ReturnsAsync(() => mockResult.Object);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, cancellationToken))
                .ReturnsAsync(() => solutionExists);

            mockResult = new Mock<IRoadMapResult>();
            mockResult.Setup(m => m.Summary).Returns(() => roadMapDescription);
        }

        [TestCase("Some description")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetRoadMapDescription(string description)
        {
            roadMapDescription = description;
            var result = await context.GetRoadMapBySolutionIdHandler.Handle(query, cancellationToken);
            result.Summary.Should().Be(roadMapDescription);
        }

        [Test]
        public async Task EmptyRoadMapResultReturnsDefaultRoadMap()
        {
            roadMapDescription = null;

            var roadMap = await context.GetRoadMapBySolutionIdHandler.Handle(
                new GetRoadMapBySolutionIdQuery(SolutionId),
                cancellationToken);

            roadMap.Should().NotBeNull();
            roadMap.Should().BeEquivalentTo(new RoadMapDto());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            solutionExists = false;

            Assert.ThrowsAsync<NotFoundException>(() => context.GetRoadMapBySolutionIdHandler.Handle(
               query,
               cancellationToken));

            context.MockSolutionRepository.Verify(r => r.CheckExists(SolutionId, cancellationToken));
            context.MockSolutionRepository.VerifyNoOtherCalls();
        }
    }
}
