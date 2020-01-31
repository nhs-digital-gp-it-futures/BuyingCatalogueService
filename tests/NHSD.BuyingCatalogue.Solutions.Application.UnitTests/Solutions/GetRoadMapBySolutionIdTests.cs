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
        private TestContext _context;
        private GetRoadMapBySolutionIdQuery _query;
        private CancellationToken _cancellationToken;
        private const string _solutionId = "Sln1";
        private string _roadMapDescription = "Some roadmap description";
        private Mock<IRoadMapResult> _mockResult;
        private bool _solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _query = new GetRoadMapBySolutionIdQuery(_solutionId);
            _cancellationToken = new CancellationToken();

            _context.MockSolutionDetailRepository.Setup(r => r.GetRoadMapBySolutionIdAsync(_solutionId, _cancellationToken))
                .ReturnsAsync(() => _mockResult.Object);
            _context.MockSolutionRepository.Setup(r => r.CheckExists(_solutionId, _cancellationToken)).ReturnsAsync(() => _solutionExists);

            _mockResult = new Mock<IRoadMapResult>();
            _mockResult.Setup(m => m.Summary).Returns(() => _roadMapDescription);
        }

        [TestCase("Some description")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetRoadMapDescription(string description)
        {
            _roadMapDescription = description;
            var result = await _context.GetRoadMapBySolutionIdHandler.Handle(_query, _cancellationToken).ConfigureAwait(false);
            result.Summary.Should().Be(_roadMapDescription);
        }

        [Test]
        public async Task EmptyRoadmapResultReturnsDefaultRoadMap()
        {
            _roadMapDescription = null;

            var roadMap = await _context.GetRoadMapBySolutionIdHandler.Handle(
                new GetRoadMapBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            roadMap.Should().NotBeNull();
            roadMap.Should().BeEquivalentTo(new RoadMapDto());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _solutionExists = false;
            var exception = Assert.ThrowsAsync<NotFoundException>( () => _context.GetRoadMapBySolutionIdHandler.Handle(_query, _cancellationToken));

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_solutionId, _cancellationToken), Times.Once);
            _context.MockSolutionRepository.VerifyNoOtherCalls();
            _context.MockSolutionDetailRepository.VerifyNoOtherCalls();
        }
    }
}
